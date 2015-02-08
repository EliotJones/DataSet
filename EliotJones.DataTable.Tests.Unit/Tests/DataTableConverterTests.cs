namespace EliotJones.DataTable.Tests.Unit.Tests
{
    using EliotJones.DataTable.DataTypeConverter;
    using EliotJones.DataTable.Enums;
    using EliotJones.DataTable.MappingResolvers;
    using EliotJones.DataTable.Tests.Unit.POCOs;
    using EliotJones.DataTable.Tests.Unit.TestStubs;
    using System;
    using System.Data;
    using System.Linq;
    using Xunit;

    public class DataTableConverterTests
    {
        private DataTableParserSettings defaultSettings = new DataTableParserSettings();
        private MappingResolver defaultMappingResolver = new TestMappingResolver();
        private IDataTableResolver defaultDataTableResolver = new TestDataTableResolver();
        private IDataTypeConverter defaultDataTypeConverter = new TestConverter();

        [Fact]
        public void ConvertToType_NullDataTableWithNullErrorSetting_ThrowsArgumentNullException()
        {
            var dataTableConverter = GetDataTableConverterWithCustomSettings(new DataTableParserSettings
                {
                    NullInputHandling = NullInputHandling.Error
                });

            Assert.Throws<ArgumentNullException>(() => dataTableConverter.ConvertToType<SimpleNoIdNoAttributes>(null));
        }

        [Fact]
        public void ConvertToType_NullDataTableWithNullReturnSetting_ReturnsNull()
        {
            var dataTableConverter = GetDataTableConverterWithCustomSettings(new DataTableParserSettings
            {
                NullInputHandling = NullInputHandling.ReturnNull
            });

            var results = dataTableConverter.ConvertToType<SimpleNoIdNoAttributes>(null);

            Assert.Null(results);
        }

        [Fact]
        public void ConvertToType_EmptyDataTableWithEmptyReturnSetting_ReturnsEmptyEnumerable()
        {
            var dataTableConverter = GetDataTableConverterWithCustomSettings(new DataTableParserSettings {
                EmptyInputHandling = EmptyInputHandling.ReturnEmptyEnumerable
            });

            var results = dataTableConverter.ConvertToType<SimpleNoIdNoAttributes>(new DataTable());

            Assert.True(results.Count() == 0);
        }

        [Fact]
        public void ConvertToType_EmptyDataTableWithErrorSetting_ThrowsInvalidOperationException()
        {
            var dataTableConverter = GetDataTableConverterWithCustomSettings(new DataTableParserSettings
            {
                EmptyInputHandling = EmptyInputHandling.Error
            });

            Assert.Throws<InvalidOperationException>(() => dataTableConverter.ConvertToType<SimpleNoIdNoAttributes>(new DataTable()));
        }

        [Fact]
        public void ConvertToType_EmptyDataTableWithNullSetting_ThrowsException()
        {
            var dataTableConverter = GetDataTableConverterWithCustomSettings(new DataTableParserSettings
            {
                EmptyInputHandling = EmptyInputHandling.ReturnNull
            });

            var results = dataTableConverter.ConvertToType<SimpleNoIdNoAttributes>(new DataTable());

            Assert.Null(results);
        }

        [Fact]
        public void ConvertToType_DataTableWithRow_ReturnsList()
        {
            var dataTableConverter = GetDefaultDataTableConverter();

            var dt = new DataTable();

            dt.Columns.Add("StringColumn", typeof(string));

            dt.Rows.Add("String");

            var results = dataTableConverter.ConvertToType<SimpleNoIdNoAttributes>(dt);

            Assert.NotNull(results);
        }

        private DataTableConverter GetDefaultDataTableConverter()
        {
            return new DataTableConverter(defaultSettings, defaultMappingResolver, defaultDataTableResolver, defaultDataTypeConverter);
        }

        private DataTableConverter GetDataTableConverterWithCustomSettings(DataTableParserSettings settings)
        {
            return new DataTableConverter(settings, defaultMappingResolver, defaultDataTableResolver, defaultDataTypeConverter);
        }
    }
}
