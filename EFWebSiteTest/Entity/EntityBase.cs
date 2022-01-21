using System.ComponentModel.DataAnnotations;


namespace EFWebSiteTest
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
