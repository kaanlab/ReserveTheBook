using ReserveTheBook.Domain.Interfaces;
using ReserveTheBook.Domain.Models;
using ReserveTheBook.WebApi.Exceptions;
using ReserveTheBook.WebApi.Models;
using ReserveTheBook.WebApi.Models.Book;

namespace ReserveTheBook.WebApi.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository _repository;
        public BookService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BookResponse>> GetAll()
        {
            var books = await _repository.LoadBooks();
            return MapToBooksResponse(books);
        }

        public async Task<IEnumerable<BookResponse>> GetAvailable()
        {
            var books = await _repository.LoadAvailableBooks();
            return MapToBooksResponse(books);
        }

        public async Task<BookResponse> GetById(Guid id)
        {
            var book = await _repository.LoadBookById(id);
            if (book is null) throw new WebApiException($"Can't find book with id {id}");

            return MapToBookResponse(book);
        }

        public async Task<BookResponse> Create(CreateBookRequest request)
        {
            Book book = null;

            if (request.Authors.Any())
            {
                var authors = MapToAuthors(request.Authors).ToList();

                book = new Book(request.Title, authors);
            }
            else
            {
                book = new Book(request.Title);
            }

            await _repository.SaveBook(book);

            return MapToBookResponse(book);
        }

        public async Task<BookResponse> Update(Guid bookId, UpdateBookRequest request)
        {
            var book = await _repository.LoadBookById(bookId);
            if (book is null) throw new WebApiException($"Can't find book with id {bookId}");

            book.Update(request.Title);

            await _repository.SaveBook(book);

            return MapToBookResponse(book);
        }

        public async Task Delete(Guid bookId)
        {
            var book = await _repository.LoadBookById(bookId);
            if (book is null) throw new WebApiException($"Can't find book with id {bookId}");

            book.Delete();
            await _repository.SaveBook(book);
        }

        public async Task<BookResponse> AddAuthors(Guid bookId, AuthorsRequest request)
        {
            var book = await _repository.LoadBookById(bookId);
            if (book is null) throw new WebApiException($"Can't find book with id {bookId}");

            var authors = MapToAuthors(request.Authors).ToList();

            book.AddAuthors(authors);

            await _repository.SaveBook(book);

            return MapToBookResponse(book);
        }

        public async Task<BookResponse> RemoveAuthors(Guid bookId, AuthorsRequest request)
        {
            var book = await _repository.LoadBookById(bookId);
            if (book is null) throw new WebApiException($"Can't find book with id {bookId}");

            var authors = MapToAuthors(request.Authors).ToList();

            book.RemoveAuthors(authors);

            await _repository.SaveBook(book);

            return MapToBookResponse(book);
        }

        private IEnumerable<Author> MapToAuthors(IEnumerable<AuthorRequest> authors) 
        {
            foreach (var author in authors)
            {
                if (author.Birthday.HasValue)
                {
                    yield return new Author(author.FirstName, author.LastName, DateOnly.FromDateTime(author.Birthday.Value));
                }
                else
                {
                    yield return new Author(author.FirstName, author.LastName);
                }
            }
        }

        private BookResponse MapToBookResponse(Book book)
        {
            if (book is null) return null;

            TimeOnly timeOnly = TimeOnly.Parse("00:00 PM");
            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Authors = book.Authors.Select(x => new AuthorDTO { FirstName = x.FirstName, LastName = x.LastName, Birthday = x.Birthday.HasValue ? x.Birthday.Value.ToDateTime(timeOnly) : null })
            };
        }

        private IEnumerable<BookResponse> MapToBooksResponse(IEnumerable<Book> books)
        {
            foreach (Book book in books)
            {
                yield return MapToBookResponse(book);
            }
        }
    }
}
