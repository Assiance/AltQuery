using System.Collections.Generic;
using AltQuery.Models.Enums;

namespace AltQuery.Models.Configuration
{
    public class ComparisonOperatorOptions
    {
        public string Equal { get; set; }

        public string NotEqual { get; set; }

        public string GreaterThan { get; set; }

        public string GreaterThanOrEqual { get; set; }

        public string LessThan { get; set; }

        public string LessThanOrEqual { get; set; }

        // public string Contains { get; set; }

        // public string StartsWith { get; set; }

        // public string EndsWith { get; set; }

        // public string In { get; set; }

        // A way to implement Case-insensitive searches

        public static readonly Dictionary<ComparisonOperatorTypes, string> QuerySyntaticValues = new Dictionary<ComparisonOperatorTypes, string>()
        {
            { ComparisonOperatorTypes.Equal, "==" },
            { ComparisonOperatorTypes.NotEqual, "!=" },
            { ComparisonOperatorTypes.GreaterThan, ">" },
            { ComparisonOperatorTypes.GreaterThanOrEqual, ">=" },
            { ComparisonOperatorTypes.LessThan, "<" },
            { ComparisonOperatorTypes.LessThanOrEqual, "<=" }
        };

        public ComparisonOperatorOptions()
        {
            Equal = "eq";
            NotEqual = "ne";
            GreaterThan = "gt";
            GreaterThanOrEqual = "ge";
            LessThan = "lt";
            LessThanOrEqual = "le";
        }
    }
}