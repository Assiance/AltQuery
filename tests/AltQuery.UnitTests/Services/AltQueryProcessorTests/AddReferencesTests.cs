using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltQuery.Models.Configuration;
using AltQuery.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace AltQuery.UnitTests.Services.AltQueryProcessorTests
{
    public class AddReferencesTests : AltQueryProcessorSpec
    {
        [Fact]
        public void AddReferences_Should_Add_References_To_AltQueryOptions_Assemblies_When_Called()
        {
            // Arrange

            // Act
            var sut = CreateSut();
            sut.AddReferences(new List<Assembly>() {Assembly.GetEntryAssembly()});

            // Assert
            var options = sut.GetAltQueryOptions();
            options.Assemblies.Should().Contain(Assembly.GetEntryAssembly());
        }

        [Fact]
        public void AddReferences_Should_Not_Add_Duplicate_References_To_AltQueryOptions_Assemblies_When_Called()
        {
            // Arrange

            // Act
            var sut = CreateSut();
            sut.AddReferences(new List<Assembly>() { Assembly.GetEntryAssembly(), Assembly.GetEntryAssembly(), Assembly.GetEntryAssembly() });

            // Assert
            var options = sut.GetAltQueryOptions();
            options.Assemblies.Where(x => x == Assembly.GetEntryAssembly()).Should().HaveCount(1);
        }

        [Fact]
        public void AddReferences_Should_Call_AddReferences_On_ScriptService_When_Called()
        {
            // Arrange
            var options = new AltQueryOptions() {GetCallingAssemblyOnInit = false};
            var assemblies = new List<Assembly>() {Assembly.GetEntryAssembly()}.ToArray();

            ScriptService.Setup(x => x.AddReferences(assemblies)).Verifiable();

            // Act
            var sut = new AltQueryProcessor(options, ScriptService.Object);
            sut.AddReferences(assemblies);
            
            // Assert
            ScriptService.Verify(x => x.AddReferences(assemblies), Times.Once);
        }

        [Fact]
        public void AddReferences_Should_Throw_Exception_When_Assemblies_Is_Null()
        {
            // Arrange

            // Act
            var sut = CreateSut();
            Action act = () => sut.AddReferences(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
