using System.Collections.Generic;
using AltQuery.Models.Enums;

namespace AltQuery.Models.Configuration
{
    public class LogicalOperatorOptions
    {
        public string And { get; set; }

        public string Or { get; set; }

        public string Not { get; set; }

        public static readonly Dictionary<LogicalOperatorTypes, string> QuerySyntaticValues = new Dictionary<LogicalOperatorTypes, string>()
        {
            { LogicalOperatorTypes.And, "&&" },
            { LogicalOperatorTypes.Or, "||" },
            { LogicalOperatorTypes.Not, "!" }
        };

        public LogicalOperatorOptions()
        {
            And = "and";
            Or = "or";
            Not = "not";
        }
    }
}