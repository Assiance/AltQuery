using AltQuery.Models.Search;

namespace AltQuery.Services.Interfaces
{
    public interface IAltQueryParser
    {
        SearchModel ToSearchModel(string query);
    }
}
