using AltQuery.Models.Configuration;
using AltQuery.Models.Search;
using AltQuery.UnitTests.Builders;
using FluentAssertions;
using Xunit;

namespace AltQuery.UnitTests.Services.AltQueryComposerTests
{
    public class ToQueryTests : AltQueryComposerSpec
    {
        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_EqualTo_Operator()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "eq", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_NotEqual_Operator()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "ne", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property != 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_GreaterThan_Operator()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "gt", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property > 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_GreaterThan_Or_EqualTo_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "ge", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property >= 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_LessThan_Operator()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "lt", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property < 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_LessThan_Or_EqualTo_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "le", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property <= 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Custom_EqualTo_Operator()
        {
            // arrange
            var moddedOptions = new AltQueryOptions()
            {
                ComparisonOperatorOptions = new ComparisonOperatorOptions()
                {
                    Equal = "MOCK-EQUAL"
                }
            };

            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "MOCK-EQUAL", "5")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == 5";

            // Act
            var result = CreateSut(moddedOptions).ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Multiline_String_Value()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"another mock value\"")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == \"another mock value\"";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Multiline_String_Value_With_Grouping_Characters()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"(another(( mock)) ()value)\"")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == \"(another(( mock)) ()value)\"";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Escaped_Quote_Character_Value()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"'another's mock 'valu'e'\"")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == \"'another's mock 'valu'e'\"";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Escaped_Double_Quote_Character_Value()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "eq", "\"\\\"another\\\"s mock \\\"valu\\\"e\\\"\"")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == \"\\\"another\\\"s mock \\\"valu\\\"e\\\"\"";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuerys_When_Composing_And_Operator()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("propertyX", "le", "5")
                .AndStatment("propertyY", "gt", "1")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.propertyX <= 5 && {SearchModel.SearchPrefix}.propertyY > 1";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuerys_When_Composing_Or_Operator()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("propertyX", "le", "5")
                .OrStatment("propertyY", "gt", "1")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.propertyX <= 5 || {SearchModel.SearchPrefix}.propertyY > 1";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuerys_When_Composing_Using_Grouping_Operators()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("propertyX", "ne", "2", null, "(")
                .OrStatment("propertyY", "gt", "5", "(", "not")
                .AndStatment("propertyZ", "lt", "10", "))")
                .Build();
            var expected = $"( {SearchModel.SearchPrefix}.propertyX != 2 || !( {SearchModel.SearchPrefix}.propertyY > 5 && {SearchModel.SearchPrefix}.propertyZ < 10))";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuerys_When_Composing_Custom_And_Operator()
        {
            // arrange
            var moddedOptions = new AltQueryOptions()
            {
                LogicalOperatorOptions = new LogicalOperatorOptions()
                {
                    And = "MOCK-AND"
                }
            };

            var searchModel = new SearchModelBuilder()
                .WithStatment("propertyX", "le", "5")
                .WithStatment("propertyY", "gt", "1", "MOCK-AND")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.propertyX <= 5 && {SearchModel.SearchPrefix}.propertyY > 1";

            // Act
            var result = CreateSut(moddedOptions).ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Not_GreaterThan_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, null, "not")
                .Build();
            var expected = $"! {SearchModel.SearchPrefix}.property > 5";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Not_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, "(", "not")
                .AndStatment("property", "lt", "10", ")")
                .Build();
            var expected = $"!( {SearchModel.SearchPrefix}.property > 5 && {SearchModel.SearchPrefix}.property < 10)";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Not_Not_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, null, "not (not")
                .AndStatment("property", "lt", "10", ")")
                .Build();
            var expected = $"! (! {SearchModel.SearchPrefix}.property > 5 && {SearchModel.SearchPrefix}.property < 10)";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_Not_Not_With_Grouping_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "gt", "5", null, ")", "not ((not")
                .AndStatment("property", "lt", "10", ")")
                .Build();
            var expected = $"! ((! {SearchModel.SearchPrefix}.property > 5) && {SearchModel.SearchPrefix}.property < 10)";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Return_Correct_LinqQuery_When_Composing_EqualTo_12_And_Not_Not_GreaterThan_5_And_LessThan_10_Statement()
        {
            // arrange
            var searchModel = new SearchModelBuilder()
                .WithStatment("property", "eq", "12")
                .WithStatment("property", "gt", "5", "and", null, "not (not")
                .AndStatment("property", "lt", "10", ")")
                .Build();
            var expected = $"{SearchModel.SearchPrefix}.property == 12 && ! (! {SearchModel.SearchPrefix}.property > 5 && {SearchModel.SearchPrefix}.property < 10)";

            // Act
            var result = CreateSut().ToQuery(searchModel);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
