using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AltQuery.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace AltQuery.Services
{
    public class AltQueryScriptService : IAltQueryScriptService
    {
        public ScriptOptions ScriptOptions { get; set; } = ScriptOptions.Default;

        public void AddReferences(params Assembly[] assemblies)
        {
            ScriptOptions = ScriptOptions.Default.AddReferences(assemblies);
        }

        public Task<T> EvaluateAsync<T>(string code, object globals = null, Type globalsType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return CSharpScript.EvaluateAsync<T>(code, ScriptOptions, globals, globalsType, cancellationToken);
        }

        public Task<object> EvaluateAsync(string code, object globals = null, Type globalsType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return CSharpScript.EvaluateAsync<object>(code, ScriptOptions, globals, globalsType, cancellationToken);
        }
    }
}
