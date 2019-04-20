using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AltQuery.Models.Configuration;
using AltQuery.Models.Enums;
using AltQuery.Models.Search;
using AltQuery.Services.Interfaces;

namespace AltQuery.Services
{
    public class AltQueryProcessor : IAltQueryProcessor
    {
        private AltQueryOptions AltQueryOptions { get; }

        private readonly IAltQueryScriptService _scriptService;
        private readonly IAltQueryParser _parserService;
        private readonly IAltQueryComposer _composerService;

        public AltQueryProcessor(IAltQueryScriptService scriptService = null, IAltQueryParser parserService = null, IAltQueryComposer composerService = null)
        {
            AltQueryOptions = new AltQueryOptions();

            _scriptService = scriptService ?? new AltQueryScriptService();
            _parserService = parserService ?? new AltQueryParser(AltQueryOptions);
            _composerService = composerService ?? new AltQueryComposer(AltQueryOptions);


            AddAssembliesViaOptions(AltQueryOptions.Assemblies);
            WarmScriptEngineViaOptions();
        }

        public AltQueryProcessor(AltQueryOptions options, IAltQueryScriptService scriptService = null, IAltQueryParser parserService = null)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(AltQueryOptions));
            }

            AltQueryOptions = new AltQueryOptions
            {
                GetCallingAssemblyOnInit = options.GetCallingAssemblyOnInit,
                ColdStartOnInit = options.ColdStartOnInit,
                ComparisonOperatorOptions = options.ComparisonOperatorOptions,
                LogicalOperatorOptions = options.LogicalOperatorOptions,
                Assemblies = options.Assemblies
            };

            _scriptService = scriptService ?? new AltQueryScriptService();
            _parserService = parserService ?? new AltQueryParser(AltQueryOptions);

            AddAssembliesViaOptions(AltQueryOptions.Assemblies);
            WarmScriptEngineViaOptions();
        }

        public void Configure(AltQueryOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(AltQueryOptions));
            }

            AltQueryOptions.GetCallingAssemblyOnInit = options.GetCallingAssemblyOnInit;
            AltQueryOptions.ColdStartOnInit = options.ColdStartOnInit;
            AltQueryOptions.ComparisonOperatorOptions = options.ComparisonOperatorOptions;
            AltQueryOptions.LogicalOperatorOptions = options.LogicalOperatorOptions;
            AltQueryOptions.Assemblies = options.Assemblies;

            AddAssembliesViaOptions(AltQueryOptions.Assemblies);
            WarmScriptEngineViaOptions();
        }

        /*
            ** Can use CSharpScript to generate filter
            ** Can implement Post and Get versions
            ** Apply can use string or search model
            ** Find a way launch script engine on startup because it takes a while (maybe run a dummy script on startup)
            ** operators should be configurable
            ** Assemblies should be configurable
            Implement In by creating a list of Ids and passing to script as param
            Must convert correctly by type (enums, decimal 30 == 30m)
            ** Must convert '' to \'\'
            ** Must convert operators (or to ||)
            May need to be able to explicitly mark none operators... for example the client has a property named 'And' and it interferes with the parser. The user can mark the property with #And to denote a property
            Add custom sort/filter methods
            _sieveProcessor.Apply(sieveModel, questions, applyFiltering: false, applySorting: false, applyPagination: true); ?pageSize, nvm it will be part of pageModel, 
            Should only be able to search certain specified properties [Sieve(CanFilter = true, CanSort = true, Name = "created")]
            ErrorHandling
        */

        public async Task<IEnumerable<T>> ApplyAsync<T>(string query, IEnumerable<T> listToSearch) where T : class
        {
            var expression = await FormExpressionAsync<T>(query);
            return listToSearch.Where(expression);
        }

        public async Task<IEnumerable<T>> ApplyAsync<T>(SearchModel searchModel, IEnumerable<T> listToSearch) where T : class
        {
            var linqQuery = _composerService.ToQuery(searchModel);
            var expression = await _scriptService.EvaluateAsync<Func<T, bool>>($"{SearchModel.SearchPrefix} => {linqQuery}");
            return listToSearch.Where(expression);
        }

        public async Task<Func<T, bool>> FormExpressionAsync<T>(string query) where T : class
        {
            var searchModel = _parserService.ToSearchModel(query);
            var linqQuery = _composerService.ToQuery(searchModel);
            return await _scriptService.EvaluateAsync<Func<T, bool>>($"{SearchModel.SearchPrefix} => {linqQuery}");
        }

        public AltQueryOptions GetAltQueryOptions()
        {
            return AltQueryOptions.Clone() as AltQueryOptions;
        }

        public void AddReferences(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            AltQueryOptions.Assemblies = AltQueryOptions.Assemblies.Concat(assemblies).Distinct().ToList();
            _scriptService.AddReferences(AltQueryOptions.Assemblies.ToArray());
        }

        private void AddAssembliesViaOptions(IList<Assembly> assemblies)
        {
            if (AltQueryOptions.GetCallingAssemblyOnInit)
            {
                assemblies.Add(GetInitialAssembly());
            }

            AddReferences(assemblies.ToArray());
        }

        private void WarmScriptEngineViaOptions()
        {
            if (!AltQueryOptions.ColdStartOnInit)
            {
                _scriptService.EvaluateAsync(string.Empty);
            }
        }

        private static Assembly GetInitialAssembly()
        {
            StackFrame[] frames = new StackTrace().GetFrames();
            var initialAssembly = frames.Select(frame => frame?.GetMethod()?.ReflectedType?.Assembly)
                .Distinct()
                .Skip(1)
                .First();

            return initialAssembly;
        }
    }
}
