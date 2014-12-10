namespace EliotJones.DataSet.Tests.Factories
{
    using System.Data;
    using System.Linq;

    internal static class DataTableFactory
    {
        public static DataTable GenerateEmptyDataTableWithStringColumns(params string[] columnNames)
        {
            if (columnNames == null) columnNames = new string[] { null };

            DataTable dt = new DataTable("Test Table");

            dt.Columns.AddRange(columnNames.Select(name => new DataColumn(name, typeof(string))).ToArray());

            return dt;
        }
    }
}
