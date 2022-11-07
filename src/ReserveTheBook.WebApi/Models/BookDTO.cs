namespace ReserveTheBook.WebApi.Models
{
    public sealed class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<AuthorDTO> Authors { get; set; }
    }
}
