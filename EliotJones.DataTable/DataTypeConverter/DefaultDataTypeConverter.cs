namespace EliotJones.DataTable.DataTypeConverter
{
    using System;
    using System.Collections.Generic;

    public class DefaultDataTypeConverter : IDataTypeConverter
    {
        private Type stringType = typeof(string);

        public virtual object FieldToObject(object field, 
            Type type, 
            DataTableParserSettings settings, 
            DbNullConverter dbNullConverter)
        {
            if (field == DBNull.Value || field == null)
            {
                return dbNullConverter.DbNullToObject(type);
            }

            if (type == stringType)
            {
                return field.ToString();
            }

            if (!type.IsValueType && type != stringType)
            {
                throw new NotImplementedException("No Conversion exists for class of type: " + type.Name);
            }

            return ValueTypeFieldToObject(field, type, settings);            
        }

        protected virtual object ValueTypeFieldToObject(object field, Type type, DataTableParserSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
