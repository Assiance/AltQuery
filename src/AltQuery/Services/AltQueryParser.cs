using AltQuery.Models.Configuration;
using AltQuery.Models.Constants;
using AltQuery.Models.Enums;
using AltQuery.Models.Search;
using AltQuery.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using AltQuery.Helpers;

namespace AltQuery.Services
{
    public class AltQueryParser : IAltQueryParser
    {
        private static readonly string QUOTE = "'";
        private static readonly string DOUBLE_QUOTE = "\\\"";

        private readonly string _notSymbol;
        private readonly Dictionary<string, ComparisonOperatorTypes> _comparisonOperators;
        private readonly Dictionary<string, LogicalOperatorTypes> _logicalOperators;

        public AltQueryParser(AltQueryOptions options)
        {
            _comparisonOperators = OperatorHelper.GenerateComparisonOperatorDictionary(options.ComparisonOperatorOptions);
            _logicalOperators = OperatorHelper.GenerateLogicalOperatorDictionary(options.LogicalOperatorOptions);
            _notSymbol = OperatorHelper.GetNotSymbol(_logicalOperators);
        }

        public SearchModel ToSearchModel(string query)
        {
            var searchModel = new SearchModel
            {
                FilterOptions = ParseFilterOptions(query)
            };

            // #2 check Field if is searchable

            return searchModel;
        }

        private List<FilterOption> ParseFilterOptions(string query)
        {
            query = ReplaceSpecialCharactersWithTokens(query);
            var initialparts = query.Split(" ");

            var filterOptions = new List<FilterOption>();

            var modifiedParts = initialparts.ToList();
            while (modifiedParts.Count > 0)
            {
                modifiedParts = RemoveLeftParts(modifiedParts, out string[] leftParts);
                modifiedParts = RemoveRightParts(modifiedParts, out string[] rightParts);
                var filters = ConstructFilterOptions(leftParts, rightParts);

                filterOptions.AddRange(filters);
            }

            return filterOptions;
        }

        private static string ReplaceSpecialCharactersWithTokens(string query)
        {
            return query
                .Replace("\\\'", SpecialCharacterTokens.Quote)
                .Replace("\"", SpecialCharacterTokens.DoubleQuote);
        }

        private List<string> RemoveLeftParts(List<string> parts, out string[] leftParts)
        {
            var comparisonIndex = GetComparisonOperatorsIndex(parts);

            var beginningToComparisonOperator = parts.Take(comparisonIndex + 1).ToList();
            parts.RemoveRange(0, comparisonIndex + 1);

            leftParts = beginningToComparisonOperator.ToArray();
            return parts;
        }

        private List<string> RemoveRightParts(List<string> parts, out string[] rightParts)
        {
            if (parts.First().StartsWith(QUOTE))
            {
                var endingQuoteIndex = parts.FindIndex(x => x.EndsWith(QUOTE));
                var beginningQuoteToEndingQuote = parts.Take(endingQuoteIndex + 1).ToList();

                parts.RemoveRange(0, endingQuoteIndex + 1);

                rightParts = beginningQuoteToEndingQuote.ToArray();
                return parts;
            }
            else
            { 
                var nextItemAfterComparison = parts.Take(1).ToList();
                parts.RemoveAt(0);

                rightParts = nextItemAfterComparison.ToArray();

                return parts;
            }
        }

        private IEnumerable<FilterOption> ConstructFilterOptions(string[] leftParts, string[] rightParts)
        {
            var filterOptions = new List<FilterOption>();
            var comparisonPart = leftParts[leftParts.Length - 1];

            var modifiedLeftParts = RemoveLogicalOperator(leftParts, out string logicalPart);
            modifiedLeftParts = RemoveNegationOperator(modifiedLeftParts, out string negationPart);
            modifiedLeftParts = RemoveLeftGroupingOperator(modifiedLeftParts, out string groupingPart);

            var modifiedRightParts = groupingPart == null
                ? RemoveRightGroupingOperator(rightParts, out groupingPart)
                : rightParts;

            modifiedRightParts = ReplaceFirstLastSingleQouteWithDouble(modifiedRightParts);
            modifiedRightParts = TrimFirstLastSpecialCharacters(modifiedRightParts);

            modifiedLeftParts = ReplaceTokensWithSpecialCharacters(modifiedLeftParts);
            modifiedRightParts = ReplaceTokensWithSpecialCharacters(modifiedRightParts);

            filterOptions.Add(new FilterOption()
            {
                Field = modifiedLeftParts[0],
                Operator = new OperatorModel()
                {
                    Comparison = comparisonPart,
                    Logical = logicalPart,
                    Negation = negationPart,
                    Grouping = groupingPart
                },
                Value = string.Join(" ", modifiedRightParts)
            });

            return filterOptions;
        }

        private static string GetCharacters(char targetCharacter, string part)
        {
            var characters = part.Where(partCharacter => partCharacter == targetCharacter).Select(t => t).ToArray();

            return characters.Length > 0 ? new string(characters) : null;
        }

        private static string[] ReplaceTokensWithSpecialCharacters(string[] parts)
        {
            return parts.Select(x => x.Replace(SpecialCharacterTokens.Quote, QUOTE).Replace(SpecialCharacterTokens.DoubleQuote, DOUBLE_QUOTE)).ToArray();
        }

        private int GetComparisonOperatorsIndex(IList<string> parts)
        {
            for (int i = 0; i < parts.Count(); i++)
            {
                _comparisonOperators.TryGetValue(parts[i], out ComparisonOperatorTypes comparisonType);

                if (comparisonType != ComparisonOperatorTypes.None)
                {
                    return i;
                }
            }

            return -1;
        }

        private string[] RemoveLogicalOperator(string[] parts, out string logicalPart)
        {
            logicalPart = null;

            for (int i = 0; i < parts.Length; i++)
            {
                _logicalOperators.TryGetValue(parts[i], out LogicalOperatorTypes logicalType);

                if (logicalType != LogicalOperatorTypes.None && logicalType != LogicalOperatorTypes.Not)
                {
                    logicalPart = parts[i];

                    var modifiedParts = parts.ToList();
                    modifiedParts.RemoveAt(i);
                    return modifiedParts.ToArray();
                }
            }

            return parts;
        }

        private string[] RemoveNegationOperator(string[] parts, out string negationPart)
        {
            negationPart = null;

            for (int i = 0; i < parts.Length; i++)
            {
                var currentPart = parts[i];

                if (currentPart == _notSymbol)
                {
                    var nextPart = parts[i + 1];
                    var modifiedParts = parts.ToList();

                    var nextPartIsNegation = nextPart.Replace(SpecialCharacters.LeftParentheses.ToString(), string.Empty) == _notSymbol;
                    if (nextPartIsNegation)
                    {
                        negationPart = $"{currentPart} {nextPart}";
                        modifiedParts.RemoveRange(i, 2);
                    }
                    else
                    {
                        negationPart = currentPart;
                        modifiedParts.RemoveAt(i);
                    }

                    return modifiedParts.ToArray();
                }
            }

            return parts;
        }

        private string[] RemoveLeftGroupingOperator(string[] leftParts, out string groupingPart)
        {
            groupingPart = null;

            var hasLeftGrouping = leftParts[0].StartsWith(SpecialCharacters.LeftParentheses);
            if (hasLeftGrouping)
            {
                groupingPart = GetCharacters(SpecialCharacters.LeftParentheses, leftParts[0]);
                leftParts[0] = leftParts[0].Remove(0, groupingPart.Length);
            }

            return leftParts;
        }

        private string[] RemoveRightGroupingOperator(string[] rightParts, out string groupingPart)
        {
            groupingPart = null;

            var hasRightGrouping = rightParts.Last().EndsWith(SpecialCharacters.RightParentheses);
            if (hasRightGrouping)
            {
                groupingPart = GetCharacters(SpecialCharacters.RightParentheses, new string(rightParts[0].Reverse().ToArray()));
                rightParts[0] = rightParts[0].Remove(rightParts[0].Length - groupingPart.Length, groupingPart.Length);
            }

            return rightParts;
        }

        public string[] ReplaceFirstLastSingleQouteWithDouble(string[] array)
        {
            array[0] = array.First().Replace("'", SpecialCharacters.DoubleQoute);
            array[array.Length - 1] = array.Last().Replace("'", SpecialCharacters.DoubleQoute);

            return array;
        }

        public string[] TrimFirstLastSpecialCharacters(string[] array)
        {
            array[0] = array.First().TrimStart(SpecialCharacters.LeftParentheses);
            array[array.Length - 1] = array.Last().TrimEnd(SpecialCharacters.RightParentheses);

            return array;
        }
    }
}
