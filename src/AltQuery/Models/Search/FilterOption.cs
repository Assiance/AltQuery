namespace AltQuery.Models.Search
{
    public class FilterOption
    {
        public string Field { get; set; }

        public OperatorModel Operator { get; set; }

        public object Value { get; set; }
    }
}