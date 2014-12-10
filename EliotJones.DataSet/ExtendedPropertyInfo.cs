namespace EliotJones.DataSet
{
    using System.Reflection;

    public class ExtendedPropertyInfo
    {
        public string FieldName { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public int ColumnIndex { get; private set; }

        public ExtendedPropertyInfo(string fieldName, PropertyInfo propertyInfo, int columnIndex)
        {
            this.FieldName = fieldName;
            this.PropertyInfo = propertyInfo;
            this.ColumnIndex = columnIndex;
        }
    }
}
