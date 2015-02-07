namespace EliotJones.DataTable.Tests.Unit.Tests
{
    using EliotJones.DataTable.Tests.Unit.Factories;
    using EliotJones.DataTable.Tests.Unit.POCOs;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultDataTableResolverTests
    {
        private IDataTypeConverter dataTypeConverter = new TestConverter();
        private DefaultDataTableResolver dataTableResolver = new DefaultDataTableResolver();
        private DataTableParserSettings dataTableParserSettings = new DataTableParserSettings();

        [Fact]
        public void ToObjects_NullDataTable_ThrowsException()
        {
            Assert.Throws(typeof(NullReferenceException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(null, dataTypeConverter, CreateEmptyPropertyMappings(), dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullDataTypeConverter_ThrowsException()
        {
            Assert.Throws(typeof(NullReferenceException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(new DataTable(), null, CreateEmptyPropertyMappings(), dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullMappings_ThrowsException()
        {
            Assert.Throws(typeof(NullReferenceException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(new DataTable(), dataTypeConverter, null, dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullArguments_ThrowsException()
        {
            Assert.Throws(typeof(NullReferenceException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(null, null, null, null));
        }

        [Fact]
        public void ToObjects_EmptyDataTable_ReturnsEmptyEnumerableOfCorrectType()
        {
            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(new DataTable(), dataTypeConverter, CreateEmptyPropertyMappings(), dataTableParserSettings);

            Assert.Equal(0, results.Count());
        }

        [Fact]
        public void ToObjects_DataTableWithOneRowCorrectTypes_ReturnsEnumerableWithCorrectResult()
        {
            var mappings = CreatePropertyMappingsDirectlyMatchingObject(typeof(SimpleNoIdNoAttributes));

            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            foreach (var mapping in mappings)
            {
                mapping.ColumnIndex = dt.Columns.IndexOf(mapping.FieldName);
            }

            dt.Rows.Add(1, "string");

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappings, dataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Fact]
        public void ToObjects_DataTableWithManyRowsMatchingColumns_ReturnsEnumerableWithCorrectResult()
        {
            int rows = 1000;

            List<SimpleNoIdNoAttributes> objects = new List<SimpleNoIdNoAttributes>(capacity: rows);

            for (int i = 0; i < rows; i++)
            {
                objects.Add(new SimpleNoIdNoAttributes
                    {
                        PropertyOne = i + 1,
                        PropertyTwo = "Property" + i
                    });
            }

            var mappings = CreatePropertyMappingsDirectlyMatchingObject(typeof(SimpleNoIdNoAttributes));

            DataTable dt = DataTableFactory.GenerateDataTableFilledWithObjects<SimpleNoIdNoAttributes>(objects);

            foreach (var mapping in mappings)
            {
                mapping.ColumnIndex = dt.Columns.IndexOf(mapping.FieldName);
            }

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappings, dataTableParserSettings);

            Assert.True(results.Count == rows);
        }

        private IEnumerable<ExtendedPropertyInfo> CreateEmptyPropertyMappings()
        {
            return new List<ExtendedPropertyInfo>();
        }

        private IEnumerable<ExtendedPropertyInfo> CreatePropertyMappingsDirectlyMatchingObject(Type type)
        {
            List<ExtendedPropertyInfo> returnList = new List<ExtendedPropertyInfo>();

            foreach (var p in type.GetProperties())
            {
                returnList.Add(new ExtendedPropertyInfo(p.Name, p, -1));
            }

            return returnList;
        }

        private class TestConverter : IDataTypeConverter
        {
            public object FieldToObject(object field, Type type, DataTableParserSettings settings)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }
                return null;
            }
        }
    }
}
