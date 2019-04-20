using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AltQuery.Models.Configuration;
using AltQuery.Models.Search;

namespace AltQuery.Services.Interfaces
{
    public interface IAltQueryProcessor
    {
        Task<IEnumerable<T>> ApplyAsync<T>(string query, IEnumerable<T> list) where T : class;

        Task<IEnumerable<T>> ApplyAsync<T>(SearchModel searchModel, IEnumerable<T> listToSearch) where T : class;

        Task<Func<T, bool>> FormExpressionAsync<T>(string query) where T : class;

        AltQueryOptions GetAltQueryOptions();

        void AddReferences(IEnumerable<Assembly> assemblies);
    }
}
