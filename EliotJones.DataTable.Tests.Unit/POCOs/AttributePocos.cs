namespace EliotJones.DataTable.Tests.Unit.POCOs
{
    internal class SimpleClassWithAttributes
    {
        [ColumnMapping(Name = "Prop1")]
        public int PropertyOne { get; set; }

        [ColumnMapping(Name = "Prop2")]
        public string PropertyTwo { get; set; }
    }

    internal class SimpleClassWithAttributesAndId
    {
        [ColumnMapping(Name = "class_id")]
        public int Id { get; set; }

        [ColumnMapping(Name = "PropOne")]
        public string PropertyOne { get; set; }
    }
}
