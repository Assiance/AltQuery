using System.Collections.Generic;

namespace AltQuery.Models.Search
{
    public class SearchModel
    {
        public const string SearchPrefix = "AltQueryObject";

        public IList<FilterOption> FilterOptions { get; set; }

        public SearchModel()
        {
            FilterOptions = new List<FilterOption>();
        }
    }
}
