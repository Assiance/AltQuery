using AltQuery.Models.Configuration;
using AltQuery.Services;

namespace AltQuery.UnitTests.Services.AltQueryComposerTests
{
    public class AltQueryComposerSpec
    {
        public AltQueryComposer CreateSut(AltQueryOptions options = null)
        {
            return new AltQueryComposer(options ?? new AltQueryOptions());
        }
    }
}
