using AltQuery.Models.Search;

namespace AltQuery.Services.Interfaces
{
    public interface IAltQueryComposer
    {
        string ToQuery(SearchModel searchModel);
    }
}
