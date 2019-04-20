using AltQuery.Models.Configuration;
using AltQuery.UnitTests.Builders;
using FluentAssertions;
using Xunit;

namespace AltQuery.UnitTests.Services.AltQueryParserTests
{
    public class ToSearchModelTests : AltQueryParserSpec
    {
        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_EqualTo_Operator()
        {
            // arrange
            var query = "property eq 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "eq", "5")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_NotEqual_Operator()
        {
            // arrange
            var query = "property ne 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "ne", "5")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_GreaterThan_Operator()
        {
            // arrange
            var query = "property gt 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "gt", "5")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_GreaterThan_Or_EqualTo_Statement()
        {
            // arrange
            var query = "property ge 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "ge", "5")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_LessThan_Operator()
        {
            // arrange
            var query = "property lt 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "lt", "5")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_LessThan_Or_EqualTo_Statement()
        {
            // arrange
            var query = "property le 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "le", "5")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Custom_EqualTo_Operator()
        {
            // arrange
            var moddedOptions = new AltQueryOptions()
            {
                ComparisonOperatorOptions = new ComparisonOperatorOptions()
                {
                    Equal = "MOCK-EQUAL"
                }
            };

            var query = "property MOCK-EQUAL 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "MOCK-EQUAL", "5")
                .Build();

            // Act
            var result = CreateSut(moddedOptions).ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Multiline_String_Value()
        {
            // arrange
            var query = "property eq 'another mock value'";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"another mock value\"")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Multiline_String_Value_With_Grouping_Characters()
        {
            // arrange
            var query = "property eq '(another(( mock)) ()value)'";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"(another(( mock)) ()value)\"")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Escaped_Quote_Character_Value()
        {
            // arrange
            var query = "property eq '\\'another\\'s mock \\'valu\\'e\\''";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"'another's mock 'valu'e'\"")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Escaped_Double_Quote_Character_Value()
        {
            // arrange
            var query = "property eq '\"another\"s mock \"valu\"e\"'";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"\\\"another\\\"s mock \\\"valu\\\"e\\\"\"")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOptions_When_Parsing_And_Operator()
        {
            // arrange
            var query = "propertyX le 5 and propertyY gt 1";
            var expected = new SearchModelBuilder()
                .WithStatment("propertyX", "le", "5")
                .AndStatment("propertyY", "gt", "1")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOptions_When_Parsing_Or_Operator()
        {
            // arrange
            var query = "propertyX le 5 or propertyY gt 1";
            var expected = new SearchModelBuilder()
                .WithStatment("propertyX", "le", "5")
                .OrStatment("propertyY", "gt", "1")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOptions_When_Parsing_Using_Grouping_Operators()
        {
            // arrange
            var query = "(propertyX ne 2 or not (propertyY gt 5 and propertyZ lt 10))";
            var expected = new SearchModelBuilder()
                .WithStatment("propertyX", "ne", "2", null, "(")
                .OrStatment("propertyY", "gt", "5", "(", "not")
                .AndStatment("propertyZ", "lt", "10", "))")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOptions_When_Parsing_Custom_And_Operator()
        {
            // arrange
            var moddedOptions = new AltQueryOptions()
            {
                LogicalOperatorOptions = new LogicalOperatorOptions()
                {
                    And = "MOCK-AND"
                }
            };

            var query = "propertyX le 5 MOCK-AND propertyY gt 1";
            var expected = new SearchModelBuilder()
                .WithStatment("propertyX", "le", "5")
                .WithStatment("propertyY", "gt", "1", "MOCK-AND")
                .Build();

            // Act
            var result = CreateSut(moddedOptions).ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Not_GreaterThan_Statement()
        {
            // arrange
            var query = "not property gt 5";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, null, "not")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Not_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var query = "not (property gt 5 and property lt 10)";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, "(", "not")
                .AndStatment("property", "lt", "10", ")")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Not_Not_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var query = "not (not property gt 5 and property lt 10)";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, null, "not (not")
                .AndStatment("property", "lt", "10", ")")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_Not_Not_With_Grouping_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var query = "not ((not property gt 5) and property lt 10)";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, ")", "not ((not")
                .AndStatment("property", "lt", "10", ")")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_FilterOption_When_Parsing_EqualTo_12_And_Not_Not_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var query = "property eq 12 and not (not property gt 5 and property lt 10)";
            var expected = new SearchModelBuilder()
                .WithStatment("property", "eq", "12")
                .WithStatment("property", "gt", "5", "and", null, "not (not")
                .AndStatment("property", "lt", "10", ")")
                .Build();

            // Act
            var result = CreateSut().ToSearchModel(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
