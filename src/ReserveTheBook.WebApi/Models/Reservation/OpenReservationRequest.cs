namespace ReserveTheBook.WebApi.Models.Reservation
{
    public sealed class OpenReservationRequest
    {
        public Guid BookId { get; set; }
        public string Comment { get; set; }
    }
}
