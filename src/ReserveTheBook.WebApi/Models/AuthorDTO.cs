using System.Text.Json.Serialization;

namespace ReserveTheBook.WebApi.Models
{
    public sealed class AuthorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
