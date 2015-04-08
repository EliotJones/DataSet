namespace ConsoleApplication1
{
    using System;
    using System.Data;
    using UglyToad.DataTable;
    using UglyToad.DataTable.Types;

    class Program
    {
        static void Main(string[] args)
        {
            var dataTable = new Program().GetDataTableForClassWithSomeAttributes();

            var results = DataTableConverter.Convert<ClassWithSomeAttributes>(dataTable);

            Console.WriteLine(results.Count + " results, result 100 owner = " + results[99].Owner + " foundation date = " + results[99].Foundation.ToShortDateString());

            Console.ReadKey();
        }

        private DataTable GetDataTableForClassWithSomeAttributes()
        {
            var dataTable = new DataTable();

            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Beehive", typeof (string)),
                new DataColumn("Owner", typeof (string)),
                new DataColumn("Foundation", typeof (DateTime)),
                new DataColumn("Viable", typeof (int))
            });

            for (int i = 0; i < 500; i++)
            {
                var dataRow = dataTable.NewRow();

                dataRow["Beehive"] = i.ToString();
                dataRow["Owner"] = "Lord Beekeeper The " + i;
                dataRow["Foundation"] = new DateTime(2001, 1, 1);
                dataRow["Viable"] = (i % 2 == 0) ? 1 : 0;

                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }

    internal class ClassWithSomeAttributes
    {
        [ColumnMapping(Name = "Beehive")]
        public int Count { get; set; }

        public string Owner { get; set; }

        public DateTime Foundation { get; set; }

        [ColumnMapping("Viable")]
        public bool HasQueen { get; set; }
    }
}
