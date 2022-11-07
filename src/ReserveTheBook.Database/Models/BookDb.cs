namespace ReserveTheBook.Database.Models
{
    public sealed class BookDb : EntityDb
    {
        public string Title { get; set; }
        public bool IsReserved { get; set; }
        public List<AuthorDb> Authors { get; set; }
        public IEnumerable<ReservationDb>? Reservations { get; set; }
    }
}
