namespace ReserveTheBook.WebApi.Models.Book
{
    public class AuthorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
