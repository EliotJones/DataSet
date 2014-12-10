namespace EliotJones.DataSet.Tests.Tests
{
    using EliotJones.DataSet.Enums;
    using EliotJones.DataSet.Tests.Factories;
    using EliotJones.DataSet.Tests.POCOs;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultMappingResolverTests
    {
        DefaultMappingResolver defaultMappingResolver = new DefaultMappingResolver();
        DataTableParserSettings defaultDataTableParserSettings = new DataTableParserSettings();

        [Fact]
        public void GetPropertyMappings_NullDataTable_ThrowsNullReferenceException()
        {
            Assert.Throws(typeof(NullReferenceException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(null, defaultDataTableParserSettings));
        }

        [Fact]
        public void GetPropertyMappings_NullSettings_ThrowsNullReferenceException()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns();

            Assert.Throws(typeof(NullReferenceException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, null));
        }

        [Fact]
        public void GetPropertyMappings_NullSettingsAndDataTable_ThrowsNullReferenceException()
        {
            Assert.Throws(typeof(NullReferenceException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(null, null));
        }

        [Fact]
        public void GetPropertyMappings_DataTableWithoutColumns_ReturnsEmptyCollection()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_DataTableWithUnnamedColumns_ReturnsEmptyCollection()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(null);

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_MappedClassHasNoProperties_ReturnsEmptyCollection()
        {
            string anyName = "random name";

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(anyName);

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoProperties>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_OnePropertyWithMatchingColumn_ReturnsCollectionWithOneResult()
        {
            SimpleOnePropertyNoIdNoMappings poco = new SimpleOnePropertyNoIdNoMappings();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoMappings>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Fact]
        public void GetPropertyMappings_OnePropertyWithMatchingColumn_ReturnsCollectionWithCorrectResult()
        {
            SimpleOnePropertyNoIdNoMappings poco = new SimpleOnePropertyNoIdNoMappings();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoMappings>(dt, defaultDataTableParserSettings);

            ExtendedPropertyInfo propInfo = results.First();

            Assert.True(propInfo.FieldName == "PropertyOne");

            Assert.True(propInfo.ColumnIndex == 0);
        }

        [Theory, 
        InlineData("propertyOnE"), 
        InlineData("PROPERTYONE"), 
        InlineData("propertyone")]
        public void GetPropertyMappings_OnePropertyWithMatchingColumnIncorrectCase_ReturnsCollectionWithOneResult(string columnName)
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columnName);

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoMappings>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Theory, 
        InlineData("propertyOne", "PropertyTwo"), 
        InlineData("PropertyOne", "PropertyTwo"), 
        InlineData("PropertyTwo", "PropertyOne")]
        public void GetPropertyMappings_TwoPropertiesWithMatchingColumn_ReturnsCollectionWithTwoResults(string column1, string column2)
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(column1, column2);

            var results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 2);
        }

        [Fact]
        public void GetPropertyMappings_TwoPropertiesOneMatching_MissingMappingHandlingCausesError()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            DataTableParserSettings dtps = new DataTableParserSettings { MissingMappingHandling = MissingMappingHandling.Error };

            Assert.Throws(typeof(Exception), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, dtps));
        }

        [Fact]
        public void GetPropertyMappings_TwoPropertiesOneMatching_MissingMappingHandlingIgnores()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            DataTableParserSettings dtps = new DataTableParserSettings { MissingMappingHandling = MissingMappingHandling.Ignore };

            var results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, dtps);

            Assert.True(results.Count == 1);
        }
    }
}
