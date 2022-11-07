using ReserveTheBook.Domain.Models;

namespace ReserveTheBook.Domain.Interfaces
{
    public interface IRepository
    {
        Task SaveBook(Book book);
        Task<Book> LoadBookById(Guid bookId);
        Task<IEnumerable<Book>> LoadBooks();
        Task<IEnumerable<Book>> LoadAvailableBooks();

        Task SaveReservation(Reservation reservation);
        Task<Reservation> LoadReservationById(Guid reservationId);
        Task<IEnumerable<Reservation>> LoadAllReservations();
        Task<IEnumerable<Reservation>> LoadActiveReservations();
        Task<IEnumerable<Reservation>> LoadReservationsByBookId(Guid bookId);
    }
}
