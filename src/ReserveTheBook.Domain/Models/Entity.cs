namespace ReserveTheBook.Domain.Models
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        public virtual void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}
