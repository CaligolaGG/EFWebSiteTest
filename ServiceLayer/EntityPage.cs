using System.Collections.Generic;

namespace ServiceLayer
{
    /// <summary>
    /// class for the generic paging of a list of entities/objects
    /// </summary>
    /// <typeparam name="T">specific entity</typeparam>
    public class EntityPage <T>
    {
        public int CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalEntitiesNumber{ get; set; }
        public int TotalPagesNumber { get; set; }
        
        /// <summary>
        /// list of the entities in that page
        /// </summary>
        public IEnumerable<T> ListEntities { get; set; } = new List<T>();
    }
}
