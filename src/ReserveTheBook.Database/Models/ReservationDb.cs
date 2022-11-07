namespace ReserveTheBook.Database.Models
{
    public sealed class ReservationDb : EntityDb
    {
        public string Comment { get; set; }
        public DateTimeOffset ReservedAt { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }
        public bool IsActive { get; set; }

        public BookDb Book { get; set; }
    }
}
