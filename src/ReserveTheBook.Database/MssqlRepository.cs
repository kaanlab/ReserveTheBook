using Microsoft.EntityFrameworkCore;
using ReserveTheBook.Database.Mappings;
using ReserveTheBook.Database.Models;
using ReserveTheBook.Domain.Interfaces;
using ReserveTheBook.Domain.Models;

namespace ReserveTheBook.Database
{
    public sealed class MssqlRepository : IRepository
    {
        private readonly AppDbContext _context;

        public MssqlRepository(AppDbContext context)
        {
            _context = context;
        }

        // ---- Authors ----
        public async Task<IEnumerable<Author>> LoadAuthors()
        {
            var dbAuthors = await _context.Authors.ToArrayAsync();
            return App.Mapper.Map<IEnumerable<Author>>(dbAuthors);
        }

        public async Task SaveAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        // ---- Books ----
        public async Task<Book> LoadBookById(Guid id)
        {
            var dbBook = await _context.Books.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id.Equals(id) && x.IsDeleted == false);
            return App.Mapper.Map<Book>(dbBook);
        }

        public async Task<IEnumerable<Book>> LoadAvailableBooks()
        {
            var dBbooks = await _context.Books.Include(x => x.Authors).Where(x => x.IsDeleted == false && x.IsReserved == false).ToArrayAsync();
            return App.Mapper.Map<IEnumerable<Book>>(dBbooks);
        }

        public async Task<IEnumerable<Book>> LoadBooks()
        {
            var dbBooks = await _context.Books.Include(x => x.Authors).Where(x => x.IsDeleted == false).ToArrayAsync();
            return App.Mapper.Map<IEnumerable<Book>>(dbBooks);
        }

        public async Task SaveBook(Book book)
        {
            var dbBook = await _context.Books.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id.Equals(book.Id) && x.IsDeleted == false);

            if (dbBook is not null)
            {

                if (dbBook.Authors.Any() && book.Authors.Any())
                {
                    var authors = MapToAuthors(dbBook.Authors).ToArray();
                    
                    if (!authors.SequenceEqual(book.Authors))
                    {
                        dbBook.Authors.Clear();

                        var dbAuthors = MapToAuthorsDb(book.Authors).ToArray();
                        await _context.Authors.AddRangeAsync(dbAuthors);

                        dbBook.Authors.AddRange(dbAuthors);
                    }
                }

                dbBook.Title = book.Title;
                dbBook.IsReserved = book.IsReserved;
                dbBook.DeletedAt = book.DeletedAt;
                dbBook.IsDeleted = book.IsDeleted;

            }
            else
            {
                var newDbBook = App.Mapper.Map<BookDb>(book);
                if (book.Authors.Any())
                {
                    var dbAuthors = MapToAuthorsDb(book.Authors).ToArray();
                    await _context.Authors.AddRangeAsync(dbAuthors);
                    newDbBook.Authors = new();
                    newDbBook.Authors.AddRange(dbAuthors);
                    await _context.Books.AddAsync(newDbBook);
                }
            }

            await _context.SaveChangesAsync();
        }

        // ---- Reservation ----
        public async Task<IEnumerable<Reservation>> LoadActiveReservations()
        {
            var dbReservations = await _context.Reservations.Include(x => x.Book).ThenInclude(x => x.Authors).Where(x => x.IsDeleted == false && x.IsActive == true).ToArrayAsync();
            return App.Mapper.Map<IEnumerable<Reservation>>(dbReservations);
        }

        public async Task<Reservation> LoadReservationById(Guid id)
        {
            var dbReservations = await _context.Reservations.Include(x => x.Book).ThenInclude(x => x.Authors).FirstOrDefaultAsync(x => x.Id.Equals(id) && x.IsDeleted == false);
            return App.Mapper.Map<Reservation>(dbReservations);
        }

        public async Task<IEnumerable<Reservation>> LoadAllReservations()
        {
            var dbReservations = await _context.Reservations.Include(x => x.Book).ThenInclude(x => x.Authors).Where(x => x.IsDeleted == false).ToArrayAsync();
            return App.Mapper.Map<IEnumerable<Reservation>>(dbReservations);
        }

        public async Task SaveReservation(Reservation reservation)
        {
            var dbReservation = await _context.Reservations.Include(x => x.Book).FirstOrDefaultAsync(x => x.Id.Equals(reservation.Id) && x.IsDeleted == false);
            if (dbReservation is not null)
            {
                if(dbReservation.IsActive != reservation.IsActive)
                {
                    var dbBook = await _context.Books.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id.Equals(reservation.Book.Id) && x.IsDeleted == false);
                    dbBook.IsReserved = reservation.Book.IsReserved;
                }

                dbReservation.Comment = reservation.Comment;
                dbReservation.ReturnedAt = reservation.ReturnedAt;
                dbReservation.IsDeleted = reservation.IsDeleted;
                dbReservation.DeletedAt = reservation.DeletedAt;
                dbReservation.IsActive = reservation.IsActive;
            }
            else
            {
                var dbBook = await _context.Books.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id.Equals(reservation.Book.Id) && x.IsDeleted == false);
                dbBook.IsReserved = reservation.Book.IsReserved;
                var newDbReservation = App.Mapper.Map<ReservationDb>(reservation);
                newDbReservation.Book = dbBook;
                await _context.Reservations.AddAsync(newDbReservation);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> LoadReservationsByBookId(Guid bookId)
        {
            var dbReservations = await _context.Reservations.Include(x => x.Book).ThenInclude(x => x.Authors).Where(x => x.IsDeleted == false).ToArrayAsync();
            var dbBookReservation = dbReservations.Where(x => x.Book.Id.Equals(bookId)).ToArray();
            return App.Mapper.Map<IEnumerable<Reservation>>(dbBookReservation);
        }

        private IEnumerable<AuthorDb> MapToAuthorsDb(IEnumerable<Author> authors)
        {
            TimeOnly timeOnly = TimeOnly.Parse("00:00 PM");
            foreach (var author in authors)
            {
                yield return new AuthorDb
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Birthday = author.Birthday.HasValue ? author.Birthday.Value.ToDateTime(timeOnly) : null
                };
            }
        }

        private IEnumerable<Author> MapToAuthors(IEnumerable<AuthorDb> authors)
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
    }
}
