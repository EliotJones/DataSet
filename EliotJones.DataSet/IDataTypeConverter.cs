namespace EliotJones.DataSet
{
    public interface IDataTypeConverter
    {
        /// <summary>
        /// Takes an object from the <see cref="DataTable"/> and converts it to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="field">Field to convert.</param>
        /// <returns>object of the correct type.</returns>
        object FieldToObject<T>(object field);
    }
}
