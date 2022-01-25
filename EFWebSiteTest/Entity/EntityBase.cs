using System.ComponentModel.DataAnnotations;


namespace EFWebSiteTest
{
    /// <summary>
    /// basic class for all the entities that have an int Id as primary key
    /// </summary>
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
