using System.Collections.Generic;
namespace EFWebSiteTest
{
    public class EntityPage <T>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int NumberEntities{ get; set; }

        public IEnumerable<T> Entities { get; set; } = new List<T>();
    }
}
