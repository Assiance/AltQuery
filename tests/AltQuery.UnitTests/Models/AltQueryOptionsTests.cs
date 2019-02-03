using System.Reflection;
using AltQuery.Models.Configuration;
using FluentAssertions;
using Xunit;

namespace AltQuery.UnitTests.Models
{
    public class AltQueryOptionsTests
    {
        // *REMINDER* When Adding additional public properties to AltQueryOptions remember to update code and tests where it is used.

        [Fact]
        public void AltQueryOptions_Property_Count_Test()
        {
            //Arrange
            // Act
            var props = new AltQueryOptions().GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // Assert
            props.Should().HaveCount(5);
        }
    }
}
