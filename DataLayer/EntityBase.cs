using System.ComponentModel.DataAnnotations;


namespace Domain
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
