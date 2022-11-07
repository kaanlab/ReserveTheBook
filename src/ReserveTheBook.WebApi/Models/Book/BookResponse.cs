namespace ReserveTheBook.WebApi.Models.Book
{
    public sealed class BookResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<AuthorDTO> Authors { get; set; }
    }
}
