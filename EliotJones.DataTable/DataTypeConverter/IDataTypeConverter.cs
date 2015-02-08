namespace EliotJones.DataTable.DataTypeConverter
{
    using System;

    public interface IDataTypeConverter
    {
        object FieldToObject(object field, Type type, DataTableParserSettings settings, DbNullConverter dbNullConverter);
    }
}
