using AltQuery.Services;
using AltQuery.Services.Interfaces;
using Moq;

namespace AltQuery.UnitTests.Services.AltQueryProcessorTests
{
    public class AltQueryProcessorSpec
    {
        protected Mock<IAltQueryScriptService> ScriptService { get; set; }
        protected Mock<IAltQueryParser> ParserService { get; set; }
        protected Mock<IAltQueryComposer> ComposerService { get; set; }

        public AltQueryProcessorSpec()
        {
            ScriptService = new Mock<IAltQueryScriptService>();
            ParserService = new Mock<IAltQueryParser>();
            ComposerService = new Mock<IAltQueryComposer>();
        }

        public AltQueryProcessor CreateSut()
        {
            return new AltQueryProcessor(ScriptService.Object, ParserService.Object, ComposerService.Object);
        }
    }
}
