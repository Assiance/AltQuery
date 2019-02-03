namespace AltQuery.Models.Configuration
{
    public class LogicalOperatorOptions
    {
        public string And { get; set; }

        public string Or { get; set; }

        public string Not { get; set; }

        public LogicalOperatorOptions()
        {
            And = "and";
            Or = "or";
            Not = "not";
        }
    }
}