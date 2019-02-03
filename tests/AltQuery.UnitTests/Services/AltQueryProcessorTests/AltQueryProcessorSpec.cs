using AltQuery.Services;
using AltQuery.Services.Interfaces;
using Moq;

namespace AltQuery.UnitTests.Services.AltQueryProcessorTests
{
    public class AltQueryProcessorSpec
    {
        protected Mock<IAltQueryScriptService> ScriptService { get; set; }

        public AltQueryProcessorSpec()
        {
            ScriptService = new Mock<IAltQueryScriptService>();
        }

        public AltQueryProcessor CreateSut()
        {
            return new AltQueryProcessor(ScriptService.Object);
        }
    }
}
