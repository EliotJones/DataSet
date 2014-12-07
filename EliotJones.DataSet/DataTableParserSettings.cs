namespace EliotJones.DataSet
{
    using EliotJones.DataSet.Enums;

    /// <summary>
    /// Settings for de-serializing a DataTable.
    /// </summary>
    public class DataTableParserSettings
    {
        private const MissingMappingHandling DefaultMissingMappingHandling = MissingMappingHandling.Ignore;
        private const MappingMatchOrder DefaultMappingMatchOrder = MappingMatchOrder.PropertyNameFirst;
        private const NullInputHandling DefaulNullInputHandling = NullInputHandling.ReturnNull;
        private const EmptyInputHandling DefaultEmptyInputHandling = EmptyInputHandling.ReturnEmptyEnumerable;

        private MissingMappingHandling? missingMappingHandling;
        private MappingMatchOrder? mappingMatchOrder;
        private NullInputHandling? nullInputHandling;
        private EmptyInputHandling? emptyInputHandling;

        /// <summary>
        /// Gets or sets the handling of an incomplete DataTable to Class mapping.
        /// </summary>
        public MissingMappingHandling MissingMappingHandling 
        {
            get { return missingMappingHandling ?? DefaultMissingMappingHandling; }
            set { missingMappingHandling = value; } 
        }

        /// <summary>
        /// Gets or sets the order of importance with which property and attribute names are treated.
        /// </summary>
        public MappingMatchOrder MappingMatchOrder 
        {
            get { return mappingMatchOrder ?? DefaultMappingMatchOrder; }
            set { mappingMatchOrder = value; } 
        }

        /// <summary>
        /// Gets or sets the handling of null DataTables.
        /// </summary>
        public NullInputHandling NullInputHandling
        {
            get { return nullInputHandling ?? DefaulNullInputHandling; }
            set { nullInputHandling = value; }
        }

        /// <summary>
        /// Gets or sets the handling of empty DataTables.
        /// </summary>
        public EmptyInputHandling EmptyInputHandling
        {
            get { return emptyInputHandling ?? DefaultEmptyInputHandling; }
            set { emptyInputHandling = value; }
        }
    }
}
