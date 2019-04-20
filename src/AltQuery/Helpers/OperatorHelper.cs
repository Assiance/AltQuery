using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltQuery.Models.Configuration;
using AltQuery.Models.Enums;

namespace AltQuery.Helpers
{
    public static class OperatorHelper
    {
        public static Dictionary<string, ComparisonOperatorTypes> GenerateComparisonOperatorDictionary(ComparisonOperatorOptions comparisons)
        {
            var props = new ComparisonOperatorOptions().GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return props.ToDictionary(
                pInfo => pInfo.GetValue(comparisons).ToString(),
                pInfo => Enum.Parse<ComparisonOperatorTypes>(pInfo.Name));
        }

        public static Dictionary<string, LogicalOperatorTypes> GenerateLogicalOperatorDictionary(LogicalOperatorOptions logicals)
        {
            var props = new LogicalOperatorOptions().GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return props.ToDictionary(
                pInfo => pInfo.GetValue(logicals).ToString(),
                pInfo => Enum.Parse<LogicalOperatorTypes>(pInfo.Name));
        }

        public static string GetNotSymbol(Dictionary<string, LogicalOperatorTypes> logicalOperators)
        {
            return logicalOperators.First(x => x.Value == LogicalOperatorTypes.Not).Key;;
        }
    }
}
