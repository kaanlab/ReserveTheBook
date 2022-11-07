namespace ReserveTheBook.WebApi.Models.Book
{
    public sealed class CreateBookRequest
    {
        public string Title { get; set; }
        public IEnumerable<AuthorRequest> Authors { get; set; }
    }
}
