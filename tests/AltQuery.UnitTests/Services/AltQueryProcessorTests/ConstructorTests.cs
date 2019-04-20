using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AltQuery.Models.Configuration;
using AltQuery.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace AltQuery.UnitTests.Services.AltQueryProcessorTests
{
    public class ConstructorTests : AltQueryProcessorSpec
    {
        [Fact]
        public void Default_Ctor_Should_Set_DefaultOptions_When_Called()
        {
            // Arrange

            // Act
            var processor = CreateSut();
            
            // Assert
            var options = processor.GetAltQueryOptions();
            options.ColdStartOnInit.Should().Be(false);
            options.GetCallingAssemblyOnInit.Should().Be(true);
            options.ComparisonOperatorOptions.Should().BeEquivalentTo(new ComparisonOperatorOptions()
            {
                Equal = "eq",
                NotEqual = "ne",
                GreaterThan = "gt",
                GreaterThanOrEqual = "ge",
                LessThan = "lt",
                LessThanOrEqual = "le"
            });

            options.LogicalOperatorOptions.Should().BeEquivalentTo(new LogicalOperatorOptions()
            {
                And = "and",
                Or = "or",
                Not = "not"
            });
        }

        [Fact]
        public void Default_Ctor_Should_Add_Calling_Assembly_When_Called()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            ScriptService.Setup(x => x.AddReferences(assembly)).Verifiable();

            // Act
            var processor = CreateSut();

            // Assert
            processor.GetAltQueryOptions().Assemblies.Should().HaveCount(1);
            processor.GetAltQueryOptions().Assemblies.First().Should().BeSameAs(Assembly.GetExecutingAssembly());
            ScriptService.Verify(x => x.AddReferences(assembly), Times.Once);
        }

        [Fact]
        public void Default_Ctor_Should_Warm_ScriptService_When_Called()
        {
            // Arrange
            ScriptService.Setup(x => x.AddReferences(Assembly.GetExecutingAssembly())).Verifiable();
            ScriptService.Setup(x => x.EvaluateAsync(string.Empty, null, null, default(CancellationToken))).Verifiable();

            // Act
            CreateSut();

            // Assert
            ScriptService.Verify(x => x.EvaluateAsync(string.Empty, null, null, default(CancellationToken)), Times.Once);
        }

        [Fact]
        public void Alt_Ctor_Should_Set_Modify_DefaultOptions_When_Called()
        {
            // Arrange
            var modifiedOptions = new AltQueryOptions
            {
                ColdStartOnInit = true,
                GetCallingAssemblyOnInit = false,
                ComparisonOperatorOptions = {Equal = "mock eq"},
                LogicalOperatorOptions = {And = "mock and"},
                Assemblies = new List<Assembly>()
                {
                    Assembly.GetEntryAssembly()
                }
            };

            // Act
            var processor = new AltQueryProcessor(modifiedOptions, ScriptService.Object);

            // Assert
            var options = processor.GetAltQueryOptions();
            options.ColdStartOnInit.Should().Be(true);
            options.GetCallingAssemblyOnInit.Should().Be(false);
            options.ComparisonOperatorOptions.Should().BeEquivalentTo(new ComparisonOperatorOptions()
            {
                Equal = "mock eq",
                NotEqual = "ne",
                GreaterThan = "gt",
                GreaterThanOrEqual = "ge",
                LessThan = "lt",
                LessThanOrEqual = "le"
            });

            options.LogicalOperatorOptions.Should().BeEquivalentTo(new LogicalOperatorOptions()
            {
                And = "mock and",
                Or = "or",
                Not = "not"
            });

            options.Assemblies.Should().Contain(Assembly.GetEntryAssembly());

        }

        [Fact]
        public void Alt_Ctor_Should_Add_Calling_Assembly_When_GetCallingAssemblyOnInit_Is_True()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            ScriptService.Setup(x => x.AddReferences(assembly)).Verifiable();

            // Act
            var processor = new AltQueryProcessor(new AltQueryOptions(), ScriptService.Object);

            // Assert
            processor.GetAltQueryOptions().Assemblies.Should().HaveCount(1);
            processor.GetAltQueryOptions().Assemblies.First().Should().BeSameAs(Assembly.GetExecutingAssembly());
            ScriptService.Verify(x => x.AddReferences(assembly), Times.Once);
        }

        [Fact]
        public void Alt_Ctor_Should_Not_Add_Assembly_When_GetCallingAssemblyOnInit_Is_False()
        {
            // Arrange
            var modifiedOptions = new AltQueryOptions()
            {
                GetCallingAssemblyOnInit = false
            };
            ScriptService.Setup(x => x.AddReferences(It.IsAny<Assembly>())).Verifiable();

            // Act
            var processor = new AltQueryProcessor(modifiedOptions, ScriptService.Object);

            // Assert
            processor.GetAltQueryOptions().Assemblies.Should().HaveCount(0);
            ScriptService.Verify(x => x.AddReferences(It.IsAny<Assembly>()), Times.Never);
        }

        [Fact]
        public void Alt_Ctor_Should_Warm_ScriptService_When_ColdStartOnInit_Is_False()
        {
            // Arrange
            ScriptService.Setup(x => x.EvaluateAsync(string.Empty, null, null, default(CancellationToken))).Verifiable();

            // Act
            new AltQueryProcessor(new AltQueryOptions(), ScriptService.Object);

            // Assert
            ScriptService.Verify(x => x.EvaluateAsync(string.Empty, null, null, default(CancellationToken)), Times.Once);
        }

        [Fact]
        public void Alt_Ctor_Should_Not_Warm_ScriptService_When_ColdStartOnInit_Is_True()
        {
            // Arrange
            var modifiedOptions = new AltQueryOptions()
            {
                ColdStartOnInit = true
            };
            ScriptService.Setup(x => x.EvaluateAsync(string.Empty, null, null, default(CancellationToken))).Verifiable();

            // Act
            new AltQueryProcessor(modifiedOptions, ScriptService.Object);

            // Assert
            ScriptService.Verify(x => x.EvaluateAsync(string.Empty, null, null, default(CancellationToken)), Times.Never);
        }

        [Fact]
        public void Alt_Ctor_Should_Throw_Exception_When_AltQueryOptions_Is_Null()
        {
            // Arrange

            // Act
            Action act = () => new AltQueryProcessor(null, ScriptService.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
