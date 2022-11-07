using ReserveTheBook.Domain.Models;

namespace ReserveTheBook.WebApi.Models.Book
{
    public sealed class AuthorsRequest
    {
        public IEnumerable<AuthorRequest> Authors { get; set; }
    }
}
