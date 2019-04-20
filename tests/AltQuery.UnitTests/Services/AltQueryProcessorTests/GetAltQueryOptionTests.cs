using System.Collections.Generic;
using System.Reflection;
using AltQuery.Models.Configuration;
using AltQuery.Services;
using FluentAssertions;
using Xunit;

namespace AltQuery.UnitTests.Services.AltQueryProcessorTests
{
    public class GetAltQueryOptionTests : AltQueryProcessorSpec
    {
        [Fact]
        public void GetAltQueryOption_Should_Clone_AltQueryOptions_When_Called()
        {
            // Arrange
            var options = new AltQueryOptions()
            {
                GetCallingAssemblyOnInit = false,
                ColdStartOnInit = true,
                Assemblies = new List<Assembly>() { Assembly.GetEntryAssembly() },
            };

            // Act
            var sut = new AltQueryProcessor(options);
            var clonedOptions = sut.GetAltQueryOptions();

            // Grabbing the private property
            var prop = sut.GetType().GetProperty(nameof(AltQueryOptions), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo getter = prop.GetGetMethod(nonPublic: true);
            var privateOptions = getter.Invoke(sut, null) as AltQueryOptions;

            // Assert
            clonedOptions.Should().BeEquivalentTo(options);
            clonedOptions.Should().NotBeSameAs(options);
            clonedOptions.Should().BeEquivalentTo(privateOptions);
            clonedOptions.Should().NotBeSameAs(privateOptions);
        }
    }
}
