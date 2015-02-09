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
            try
            {
                if (type == typeof(int))
                {
                    if (field is int)
                    {
                        return field;
                    }
                    else if (field is string)
                    {
                        int returnValue;

                        bool canParse = int.TryParse(field.ToString(), out returnValue);

                        if (!canParse)
                        {
                            throw new NotImplementedException(string.Format("Cannot convert string: {0} to int", field.ToString()));
                        }

                        return returnValue;
                    }
                    else
                    {
                        return Convert.ToInt32(field);
                    }
                }

                throw new NotImplementedException(string.Format("No conversion for field with value: {0} to type: {1}", field.ToString(), type.Name));
            }
            catch (InvalidCastException ex)
            {
                throw new NotImplementedException(string.Format("No conversion for field with value: {0} to type: {1}", field.ToString(), type.Name), ex);
            }
        }
    }
}
