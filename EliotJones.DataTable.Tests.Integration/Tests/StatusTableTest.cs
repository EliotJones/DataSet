namespace EliotJones.DataTable.Tests.Integration.Tests
{
    using EliotJones.DataTable.Exceptions;
    using Entities;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Xunit;

    public class StatusTableTest
    {
        [Fact]
        public void GetAllStatuses_ReturnsResults()
        {
            DatabaseBootstrapper.CreateAndPopulate();

            DataTable dataTable = QueryRunner.ExecuteStoredProcedure("uspGetAllStatuses");

            DatabaseBootstrapper.Drop();

            Assert.True(dataTable.Rows.Count > 0);
        }

        [Fact]
        public void GetAllStatuses_ReturnsResults_CanBeParsed()
        {
            DatabaseBootstrapper.CreateAndPopulate();

            DataTable dataTable = QueryRunner.ExecuteStoredProcedure("uspGetAllStatuses");

            DatabaseBootstrapper.Drop();

            DataTableParser dtp = new DataTableParser();

            var results = dtp.ToObjects<StatusPropertyNamesMatch>(dataTable);

            Assert.Equal(dataTable.Rows.Count, results.Count());
        }

        [Fact]
        public void GetAllStatuses_ReturnsResults_CanBeParsedWithCorrectObjects()
        {
            DatabaseBootstrapper.CreateAndPopulate();

            DataTable dataTable = QueryRunner.ExecuteStoredProcedure("uspGetAllStatuses");

            DatabaseBootstrapper.Drop();

            DataTableParser dtp = new DataTableParser();

            var results = dtp.ToObjects<StatusPropertyNamesMatch>(dataTable);

            Assert.True(results.Where(r => r.Id == (int)dataTable.Rows[0]["Id"]
                && r.Description == dataTable.Rows[0]["Description"].ToString()
                && r.IsPublic == (bool)dataTable.Rows[0]["IsPublic"]).Count() == 1);
        }

        [Fact]
        public void GetAllStatuses_ReturnsResults_CanBeParsedWithMissingFields()
        {
            DatabaseBootstrapper.CreateAndPopulate();

            DataTable dataTable = QueryRunner.ExecuteStoredProcedure("uspGetAllStatuses");

            DatabaseBootstrapper.Drop();

            DataTableParser dtp = new DataTableParser();

            var results = dtp.ToObjects<StatusPropertyNameMissing>(dataTable);

            Assert.Equal(dataTable.Rows.Count, results.Count());
        }

        [Fact]
        public void GetAllStatuses_ReturnsResults_ThrowsOnExtraProperty()
        {
            DatabaseBootstrapper.CreateAndPopulate();

            DataTable dataTable = QueryRunner.ExecuteStoredProcedure("uspGetAllStatuses");

            DatabaseBootstrapper.Drop();

            DataTableParser dtp = new DataTableParser();

            dtp.DataTableParserSettings.MissingMappingHandling = Enums.MissingMappingHandling.Error;

            Assert.Throws<MissingMappingException<StatusExtraProperty>>(() => dtp.ToObjects<StatusExtraProperty>(dataTable));
        }

        [Fact]
        public void GetAllStatuses_ReturnsResults_IgnoresExtraProperty()
        {
            DatabaseBootstrapper.CreateAndPopulate();

            DataTable dataTable = QueryRunner.ExecuteStoredProcedure("uspGetAllStatuses");

            DatabaseBootstrapper.Drop();

            DataTableParser dtp = new DataTableParser();

            dtp.DataTableParserSettings.MissingMappingHandling = Enums.MissingMappingHandling.Ignore;

            var results = dtp.ToObjects<StatusExtraProperty>(dataTable);

            Assert.Equal(dataTable.Rows.Count, results.Count());
        }
    }
}
