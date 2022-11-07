namespace ReserveTheBook.WebApi.Models.Reservation
{
    public sealed class ReservationResponse
    {
        public Guid Id { get; set; }
        public BookDTO Book { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset ReservedAt { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }

    }
}
