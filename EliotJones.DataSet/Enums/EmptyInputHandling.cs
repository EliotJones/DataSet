namespace EliotJones.DataSet.Enums
{
    /// <summary>
    /// Handles how the <see cref="DataTableParser"/> will respond to being passed an empty <see cref="DataTable"/>.
    /// </summary>
    public enum EmptyInputHandling
    {
        ReturnEmptyEnumerable = 1,
        Error = 2,
        ReturnNull = 3
    }
}
