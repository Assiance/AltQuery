using System.Collections.Generic;
using AltQuery.Helpers;
using AltQuery.Models.Configuration;
using AltQuery.Models.Constants;
using AltQuery.Models.Enums;
using AltQuery.Models.Search;
using AltQuery.Services.Interfaces;

namespace AltQuery.Services
{
    public class AltQueryComposer : IAltQueryComposer
    {
        private readonly string _notSymbol;
        private readonly Dictionary<string, ComparisonOperatorTypes> _comparisonOperators;
        private readonly Dictionary<string, LogicalOperatorTypes> _logicalOperators;

        public AltQueryComposer(AltQueryOptions options)
        {
            _comparisonOperators = OperatorHelper.GenerateComparisonOperatorDictionary(options.ComparisonOperatorOptions);
            _logicalOperators = OperatorHelper.GenerateLogicalOperatorDictionary(options.LogicalOperatorOptions);
            _notSymbol = OperatorHelper.GetNotSymbol(_logicalOperators);
        }

        public string ToQuery(SearchModel searchModel)
        {
            string query = string.Empty;
            foreach (var filterOption in searchModel.FilterOptions)
            {
                query = ApplyLogical(filterOption, query);

                query = ApplyNegation(filterOption, query);

                query = ApplyLeftGrouping(filterOption, query);

                query = ApplyField(query, filterOption);

                query = ApplyComparison(filterOption, query);

                query = ApplyValue(query, filterOption);

                query = ApplyRightGrouping(filterOption, query);
            }

            return query.Trim();
        }

        private string ApplyLogical(FilterOption filterOption, string query)
        {
            if (filterOption.Operator.Logical != null)
            {
                var logicalOperatorType = _logicalOperators[filterOption.Operator.Logical];
                query += $" {LogicalOperatorOptions.QuerySyntaticValues[logicalOperatorType]}";
            }

            return query;
        }

        private string ApplyNegation(FilterOption filterOption, string query)
        {
            if (filterOption.Operator.Negation != null)
            {
                var negation = filterOption.Operator.Negation;
                negation = negation.Replace(_notSymbol, LogicalOperatorOptions.QuerySyntaticValues[LogicalOperatorTypes.Not]);
                query += $" {negation}";
            }

            return query;
        }

        private static string ApplyLeftGrouping(FilterOption filterOption, string query)
        {
            if (filterOption.Operator.Grouping != null &&
                filterOption.Operator.Grouping.StartsWith(SpecialCharacters.LeftParentheses))
            {
                query += $"{filterOption.Operator.Grouping}";
            }

            return query;
        }

        private static string ApplyField(string query, FilterOption filterOption)
        {
            query += $" {SearchModel.SearchPrefix}.{filterOption.Field}";
            return query;
        }

        private string ApplyComparison(FilterOption filterOption, string query)
        {
            if (filterOption.Operator.Comparison != null)
            {
                var comparisonOperatorType = _comparisonOperators[filterOption.Operator.Comparison];
                query += $" {ComparisonOperatorOptions.QuerySyntaticValues[comparisonOperatorType]}";
            }

            return query;
        }

        private static string ApplyValue(string query, FilterOption filterOption)
        {
            query += $" {filterOption.Value}";
            return query;
        }

        private static string ApplyRightGrouping(FilterOption filterOption, string query)
        {
            if (filterOption.Operator.Grouping != null &&
                filterOption.Operator.Grouping.StartsWith(SpecialCharacters.RightParentheses))
            {
                query += $"{filterOption.Operator.Grouping}";
            }

            return query;
        }
    }
}
