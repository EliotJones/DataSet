namespace EliotJones.DataTable.Tests.Unit.Tests
{
    using System.Linq;
    using Factories;
    using POCOs;
    using Xunit;

    public class DataTableConverterTests
    {
        [Fact]
        public void ToObjects_CanConvertSimpleClass()
        {
            int id = 598897;
            string propertyOne = "Vitamin";

            var data = new[]
            {
                new SimpleIdNoAttributes
                {
                    Id = id,
                    PropertyOne = propertyOne
                }
            };

            var dataTable = DataTableFactory.GenerateDataTableFilledWithObjects(data);

            var objects = DataTableConverter.Create().ToObjects<SimpleIdNoAttributes>(dataTable);
            
            Assert.Equal(id, objects.Single().Id);
            Assert.Equal(propertyOne, objects.Single().PropertyOne);
        }
    }
}
