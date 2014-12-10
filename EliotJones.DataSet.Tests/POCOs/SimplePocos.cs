namespace EliotJones.DataSet.Tests.POCOs
{
    internal class SimpleNoIdNoMappings
    {
        public int PropertyOne { get; set; }

        public string PropertyTwo { get; set; }
    }

    internal class SimpleNoIdWithMappings
    {
        public int PropertyOne { get; set; }

        public string PropertyTwo { get; set; }
    }

    internal class SimpleNoProperties
    {
        public int PropertyOne = 0;
    }

    internal class SimpleOnePropertyNoIdNoMappings
    {
        public int PropertyOne { get; set; }
    }
}
