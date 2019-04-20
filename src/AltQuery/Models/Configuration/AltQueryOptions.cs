using System;
using System.Collections.Generic;
using System.Reflection;

namespace AltQuery.Models.Configuration
{
    public class AltQueryOptions : ICloneable
    {
        public bool GetCallingAssemblyOnInit { get; set; }

        public bool ColdStartOnInit { get; set; }

        public ComparisonOperatorOptions ComparisonOperatorOptions { get; set; }

        public LogicalOperatorOptions LogicalOperatorOptions { get; set; }

        public IList<Assembly> Assemblies { get; set; }

        public AltQueryOptions()
        {
            ColdStartOnInit = false;
            GetCallingAssemblyOnInit = true;
            ComparisonOperatorOptions = new ComparisonOperatorOptions();
            LogicalOperatorOptions = new LogicalOperatorOptions();
            Assemblies = new List<Assembly>();
        }

        public object Clone()
        {
            return new AltQueryOptions()
            {
                GetCallingAssemblyOnInit = GetCallingAssemblyOnInit,
                ColdStartOnInit = ColdStartOnInit,
                ComparisonOperatorOptions = ComparisonOperatorOptions,
                LogicalOperatorOptions = LogicalOperatorOptions,
                Assemblies = Assemblies
            };
        }
    }
}