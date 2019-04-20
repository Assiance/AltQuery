using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AltQuery.Models.Search;
using Moq;
using Xunit;

namespace AltQuery.UnitTests.Services.AltQueryProcessorTests
{
    public class ApplyAsyncTests : AltQueryProcessorSpec
    {
        [Fact]
        public async Task ApplyAsync_Query_Should_Call_Parser_ToSearchModel_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            ParserService.Setup(x => x.ToSearchModel(query));
            ScriptService.Setup(x =>
                x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                .ReturnsAsync(obj => obj != null);

            // Act
            var sut = CreateSut();
            await sut.ApplyAsync(query, new List<object>());

            // Assert
            ParserService.Verify(x => x.ToSearchModel(query), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_Query_Should_Call_Composer_ToQuery_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            ComposerService.Setup(x => x.ToQuery(It.IsAny<SearchModel>()));
            ScriptService.Setup(x =>
                    x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                .ReturnsAsync(obj => obj != null);

            // Act
            var sut = CreateSut();
            await sut.ApplyAsync(query, new List<object>());

            // Assert
            ComposerService.Verify(x => x.ToQuery(It.IsAny<SearchModel>()), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_Query_Should_Call_Script_EvaluateAsync_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            ParserService.Setup(x => x.ToSearchModel(query));
            ScriptService.Setup(x =>
                    x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                .ReturnsAsync(obj => obj != null);

            // Act
            var sut = CreateSut();
            await sut.ApplyAsync(query, new List<object>());

            // Assert
            ScriptService.Verify(x => x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_Query_Should_Filter_Results_Correctly_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            var listToSearch = new List<MockObject>()
            {
                new MockObject() { Num = 1 },
                new MockObject() { Num = 2 },
                new MockObject() { Num = 3 }
            };

            ParserService.Setup(x => x.ToSearchModel(query));
            ScriptService.Setup(x =>
                    x.EvaluateAsync<Func<MockObject, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                    .ReturnsAsync(obj => obj.Num < 3);

            // Act
            var sut = CreateSut();
            var results = await sut.ApplyAsync(query, listToSearch);

            // Assert
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task ApplyAsync_SearchModel_Should_Call_Composer_ToQuery_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            ComposerService.Setup(x => x.ToQuery(It.IsAny<SearchModel>()));
            ScriptService.Setup(x =>
                    x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                .ReturnsAsync(obj => obj != null);

            // Act
            var sut = CreateSut();
            await sut.ApplyAsync(It.IsAny<SearchModel>(), new List<object>());

            // Assert
            ComposerService.Verify(x => x.ToQuery(It.IsAny<SearchModel>()), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_SearchModel_Should_Call_Script_EvaluateAsync_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            ParserService.Setup(x => x.ToSearchModel(query));
            ScriptService.Setup(x =>
                    x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                .ReturnsAsync(obj => obj != null);

            // Act
            var sut = CreateSut();
            await sut.ApplyAsync(It.IsAny<SearchModel>(), new List<object>());

            // Assert
            ScriptService.Verify(x => x.EvaluateAsync<Func<object, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_SearchModel_Should_Filter_Results_Correctly_When_Invoked()
        {
            // Arrange
            var query = "MOCK-QUERY";
            var listToSearch = new List<MockObject>()
            {
                new MockObject() { Num = 1 },
                new MockObject() { Num = 2 },
                new MockObject() { Num = 3 }
            };

            ParserService.Setup(x => x.ToSearchModel(query));
            ScriptService.Setup(x =>
                    x.EvaluateAsync<Func<MockObject, bool>>(It.IsAny<string>(), null, null, default(CancellationToken)))
                .ReturnsAsync(obj => obj.Num < 3);

            // Act
            var sut = CreateSut();
            var results = await sut.ApplyAsync(query, listToSearch);

            // Assert
            Assert.Equal(2, results.Count());
        }
    }

    class MockObject
    {
        public int Num { get; set; }
    }
}
