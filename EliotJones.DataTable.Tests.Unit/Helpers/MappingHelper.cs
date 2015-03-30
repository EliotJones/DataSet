namespace EliotJones.DataTable.Tests.Unit.Helpers
{
    using System.Collections.Generic;
    
    internal class MappingHelper
    {
        public static IEnumerable<ExtendedPropertyInfo> CreatePropertyMappingsDirectlyMatchingObject<T>()
        {
            List<ExtendedPropertyInfo> returnList = new List<ExtendedPropertyInfo>();

            foreach (var p in typeof(T).GetProperties())
            {
                returnList.Add(new ExtendedPropertyInfo(p.Name, p, -1));
            }

            return returnList;
        }
    }
}
