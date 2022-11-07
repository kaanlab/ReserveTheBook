using System.ComponentModel.DataAnnotations;

namespace ReserveTheBook.Database.Models
{
    public abstract class EntityDb
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
