using ReserveTheBook.Database.Mappings;
using ReserveTheBook.Domain.Interfaces;
using ReserveTheBook.Domain.Models;
using ReserveTheBook.WebApi.Exceptions;
using ReserveTheBook.WebApi.Models;
using ReserveTheBook.WebApi.Models.Book;
using ReserveTheBook.WebApi.Models.Reservation;

namespace ReserveTheBook.WebApi.Services
{
    public sealed class ReservationService : IReservationService
    {
        private readonly IRepository _repository;
        public ReservationService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReservationResponse>> GetAll()
        {
            var reservations = await _repository.LoadAllReservations();
            return MapToReservationsResponse(reservations);
        }

        public async Task<IEnumerable<ReservationResponse>> GetActive()
        {
            var reservations = await _repository.LoadActiveReservations();
            return MapToReservationsResponse(reservations);
        }

        public async Task<ReservationResponse> GetById(Guid id)
        {
            var reservation = await _repository.LoadReservationById(id);
            return MapToReservationResponse(reservation);
        }

        public async Task<IEnumerable<ReservationResponse>> GetByBookId(Guid id)
        {
            var reservations = await _repository.LoadReservationsByBookId(id);
            return MapToReservationsResponse(reservations);
        }

        public async Task<ReservationResponse> Open(OpenReservationRequest request)
        {
            var book = await _repository.LoadBookById(request.BookId);
            if (book is null) throw new WebApiException($"Can't find book with id {request.BookId}");

            var reservation = new Reservation(book, request.Comment);
            await _repository.SaveReservation(reservation);

            return MapToReservationResponse(reservation);
        }

        public async Task<ReservationResponse> Close(CloseReservationRequest request)
        {
            var reservation = await _repository.LoadReservationById(request.ReservationId);
            reservation.ReturnBook();
            await _repository.SaveReservation(reservation);

            return MapToReservationResponse(reservation);
        }

        public async Task<ReservationResponse> Update(UpdateReservationRequest request)
        {
            var reservation = await _repository.LoadReservationById(request.ReservationId);
            reservation.Update(request.Comment);
            await _repository.SaveReservation(reservation);

            return MapToReservationResponse(reservation);
        }

        public async Task Delete(Guid reservationId)
        {
            var reservation = await _repository.LoadReservationById(reservationId);
            reservation.Delete();

            await _repository.SaveReservation(reservation);
        }

        private ReservationResponse MapToReservationResponse(Reservation reservation)
        {
            if (reservation is null) return null;

            TimeOnly timeOnly = TimeOnly.Parse("00:00 PM");
            return new ReservationResponse
            {
                Id = reservation.Id,
                Book = new BookDTO
                {
                    Id = reservation.Book.Id,
                    Title = reservation.Book.Title,
                    Authors = reservation.Book.Authors.Select(x => new AuthorDTO { FirstName = x.FirstName, LastName = x.LastName, Birthday = x.Birthday.HasValue ? x.Birthday.Value.ToDateTime(timeOnly) : null })
                },
                Comment = reservation.Comment,
                ReservedAt = reservation.ReservedAt,
                ReturnedAt = reservation.ReturnedAt
            };
        }

        private IEnumerable<ReservationResponse> MapToReservationsResponse(IEnumerable<Reservation> reservations)
        {
            foreach (Reservation reservation in reservations)
            {
                yield return MapToReservationResponse(reservation);
            }
        }
    }
}
