using AltQuery.Models.Configuration;
using AltQuery.Services;

namespace AltQuery.UnitTests.Services.AltQueryParserTests
{
    public class AltQueryParserSpec
    {
        public AltQueryParser CreateSut(AltQueryOptions options = null)
        {
            return new AltQueryParser(options ?? new AltQueryOptions());
        }
    }
}
