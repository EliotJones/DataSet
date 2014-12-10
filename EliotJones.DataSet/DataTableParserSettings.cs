namespace EliotJones.DataSet
{
    using EliotJones.DataSet.Enums;

    /// <summary>
    /// Settings for de-serializing a DataTable.
    /// </summary>
    public class DataTableParserSettings
    {
        private MissingMappingHandling missingMappingHandling = MissingMappingHandling.Ignore;
        private MappingMatchOrder mappingMatchOrder = MappingMatchOrder.PropertyNameFirst;
        private NullInputHandling nullInputHandling = NullInputHandling.ReturnNull;
        private EmptyInputHandling emptyInputHandling = EmptyInputHandling.ReturnEmptyEnumerable;

        /// <summary>
        /// Gets or sets the handling of an incomplete DataTable to Class mapping.
        /// </summary>
        public MissingMappingHandling MissingMappingHandling 
        {
            get { return missingMappingHandling; }
            set { missingMappingHandling = value; } 
        }

        /// <summary>
        /// Gets or sets the order of importance with which property and attribute names are treated.
        /// </summary>
        public MappingMatchOrder MappingMatchOrder 
        {
            get { return mappingMatchOrder; }
            set { mappingMatchOrder = value; } 
        }

        /// <summary>
        /// Gets or sets the handling of null DataTables.
        /// </summary>
        public NullInputHandling NullInputHandling
        {
            get { return nullInputHandling; }
            set { nullInputHandling = value; }
        }

        /// <summary>
        /// Gets or sets the handling of empty DataTables.
        /// </summary>
        public EmptyInputHandling EmptyInputHandling
        {
            get { return emptyInputHandling; }
            set { emptyInputHandling = value; }
        }

        public bool InheritMappings { get; set; }
    }
}
