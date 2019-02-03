using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AltQuery.Models.Configuration;
using AltQuery.Models.Search;
using AltQuery.Services.Interfaces;

namespace AltQuery.Services
{
    public class AltQueryProcessor : IAltQueryProcessor
    {
        private AltQueryOptions AltQueryOptions { get; }

        private readonly IAltQueryScriptService _scriptService;

        public AltQueryProcessor(IAltQueryScriptService scriptService = null)
        {
            _scriptService = scriptService ?? new AltQueryScriptService();

            AltQueryOptions = new AltQueryOptions();

            AddAssembliesViaOptions();
            WarmScriptEngineViaOptions();
        }

        public AltQueryProcessor(AltQueryOptions options, IAltQueryScriptService scriptService = null)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(AltQueryOptions));
            }

            _scriptService = scriptService ?? new AltQueryScriptService();

            AltQueryOptions = new AltQueryOptions
            {
                GetCallingAssemblyOnInit = options.GetCallingAssemblyOnInit,
                ColdStartOnInit = options.ColdStartOnInit,
                ComparisonOperatorOptions = options.ComparisonOperatorOptions,
                LogicalOperatorOptions = options.LogicalOperatorOptions,
                Assemblies = options.Assemblies
            };

            AddAssembliesViaOptions();
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

            AddAssembliesViaOptions();
            WarmScriptEngineViaOptions();
        }

        /*
            Can use CSharpScript to generate filter
            Can implement Post and Get versions
            Apply can use string or search model
            * Implement In by creating a list of Ids and passing to script as param
            * Find a way launch script engine on startup because it takes a while (maybe run a dummy script on startup)
            * Must convert correctly by type (enums, decimal 30 == 30m)
            Must convert '' to \'\'
            Must convert operators (or to ||)
            * May need to be able to explicitly mark none operators... for example the client has a property named 'And' and it interferes with the parser. The user can mark the property with #And to denote a property
            * Add custom sort/filter methods
            * operators should be configurable
            * Assemblies should be configurable
            _sieveProcessor.Apply(sieveModel, questions, applyFiltering: false, applySorting: false, applyPagination: true); ?pageSize, nvm it will be part of pageModel, 
            Should only be able to search certain specified properties [Sieve(CanFilter = true, CanSort = true, Name = "created")]
        */

        public void Apply<T>(string query, IEnumerable<T> listToSearch) where T : class
        {
            // split phrase into array
            var parts = query.Split(" ");
            // get first comparison operator
            // var firstOp = parts.First(x => IsComparisonOperator(x)).Select((x, index) => new { x, index });
            // get elements to the left and one to the right
            // #1 if first element to the right starts with ' find the next element that ends with ' and not \'
            // #2 check Field if is
            // find next comparison operator
            // get elements to the left that weren't captured by previous operator and one to the right
            // #1
            // if not, apply negation
            // if (not, apply negation and grouping "("
            // if not and (not create a dummy clause with null field and value, then apply negation, grouping and operator is applicable
            // get next comparison

        }

        public void Apply<T>(SearchModel searchModel, IEnumerable<T> list) where T : class
        {
            throw new NotImplementedException();
        }

        public AltQueryOptions GetAltQueryOptions()
        {
            return AltQueryOptions.Clone() as AltQueryOptions;
        }

        private void AddAssembliesViaOptions()
        {
            if (AltQueryOptions.GetCallingAssemblyOnInit)
            {
                AltQueryOptions.Assemblies.Add(GetInitialAssembly());
            }

            _scriptService.AddReferences(AltQueryOptions.Assemblies.ToArray());
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
