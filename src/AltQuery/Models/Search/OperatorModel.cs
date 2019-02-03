namespace AltQuery.Models.Search
{
    public class OperatorModel
    {
        public string Comparison { get; set; }

        public string Logical { get; set; }

        public string Grouping { get; set; }

        public bool ApplyNegation { get; set; }
    }
}