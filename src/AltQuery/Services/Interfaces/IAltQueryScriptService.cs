using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AltQuery.Services.Interfaces
{
    public interface IAltQueryScriptService
    {
        void AddReferences(params Assembly[] assemblies);

        Task<T> EvaluateAsync<T>(string code, object globals = null, Type globalsType = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<object> EvaluateAsync(string code, object globals = null, Type globalsType = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
