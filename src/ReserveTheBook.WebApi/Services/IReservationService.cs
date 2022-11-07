using ReserveTheBook.WebApi.Models.Reservation;

namespace ReserveTheBook.WebApi.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationResponse>> GetAll();
        Task<IEnumerable<ReservationResponse>> GetActive();
        Task<ReservationResponse> GetById(Guid reservationId);
        Task<IEnumerable<ReservationResponse>> GetByBookId(Guid bookId);
        Task<ReservationResponse> Open(OpenReservationRequest request);
        Task<ReservationResponse> Close(CloseReservationRequest request);
        Task<ReservationResponse> Update(UpdateReservationRequest request);
        Task Delete(Guid reservationId);
    }
}
