namespace ReserveTheBook.WebApi.Models.Reservation
{
    public class UpdateReservationRequest
    {
        public Guid ReservationId { get; set; }
        public string Comment { get; set; }
    }
}
