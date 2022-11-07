using ReserveTheBook.WebApi.Models.Book;

namespace ReserveTheBook.WebApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponse>> GetAll();
        Task<IEnumerable<BookResponse>> GetAvailable();
        Task<BookResponse> GetById(Guid id);
        Task<BookResponse> Create(CreateBookRequest request);
        Task<BookResponse> Update(Guid bookId, UpdateBookRequest request);
        Task Delete(Guid bookId);
        Task<BookResponse> AddAuthors(Guid bookId, AuthorsRequest request);
        Task<BookResponse> RemoveAuthors(Guid bookId, AuthorsRequest request);
    }
}
