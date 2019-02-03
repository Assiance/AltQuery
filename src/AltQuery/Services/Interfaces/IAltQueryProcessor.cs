using System.Collections.Generic;
using System.Reflection;
using AltQuery.Models.Configuration;
using AltQuery.Models.Search;

namespace AltQuery.Services.Interfaces
{
    public interface IAltQueryProcessor
    {
        void Apply<T>(string query, IEnumerable<T> list) where T : class;

        void Apply<T>(SearchModel searchModel, IEnumerable<T> list) where T : class;

        AltQueryOptions GetAltQueryOptions();

        void AddReferences(IEnumerable<Assembly> assemblies);
    }
}
