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