namespace EliotJones.DataTable.Tests.Unit.POCOs
{
    using System;

    public class PrivateConstructorPublicProperty
    {
        public Guid Id { get; set; }

        private PrivateConstructorPublicProperty()
        {
        }

        public PrivateConstructorPublicProperty(Guid id)
        {
            this.Id = id;
        }
    }
}
