using AltQuery.Models.Enums;
using AltQuery.Models.Search;

namespace AltQuery.UnitTests.Builders
{
    public class SearchModelBuilder
    {
        private readonly SearchModel _searchModel;

        public SearchModelBuilder()
        {
            _searchModel = new SearchModel();
        }

        public SearchModelBuilder AddFilterOption(FilterOption filterOption)
        {
            _searchModel.FilterOptions.Add(filterOption);
            return this;
        }

        public SearchModelBuilder WithStatment(string field, string comparison, string value, string logical = null, string grouping = null, string negation = null)
        {
            _searchModel.FilterOptions.Add(new FilterOption()
            {
                Field = field,
                Operator = new OperatorModel()
                {
                    Logical = logical,
                    Comparison = comparison,
                    Grouping = grouping,
                    Negation = negation
                },
                Value = value
            });
            return this;
        }

        public SearchModelBuilder AndStatment(string field, string comparison, string value, string grouping = null, string negation = null)
        {
            _searchModel.FilterOptions.Add(new FilterOption()
            {
                Field = field,
                Operator = new OperatorModel()
                {
                    Logical = LogicalOperatorTypes.And.ToString().ToLower(),
                    Comparison = comparison,
                    Grouping = grouping,
                    Negation = negation
                },
                Value = value
            });
            return this;
        }

        public SearchModelBuilder OrStatment(string field, string comparison, string value, string grouping = null, string negation = null)
        {
            _searchModel.FilterOptions.Add(new FilterOption()
            {
                Field = field,
                Operator = new OperatorModel()
                {

                    Logical = LogicalOperatorTypes.Or.ToString().ToLower(),
                    Comparison = comparison,
                    Grouping = grouping,
                    Negation = negation
                },
                Value = value
            });
            return this;
        }

        public SearchModel Build()
        {
            return _searchModel;
        }
    }
}
