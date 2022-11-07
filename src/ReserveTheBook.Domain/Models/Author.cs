namespace ReserveTheBook.Domain.Models
{
    public sealed class Author : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateOnly? Birthday { get; private set; }

        private Author() { }

        public Author(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException("firstName");
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException("lastName");

            FirstName = firstName;
            LastName = lastName;
        }

        public Author(string firstName, string lastName, DateOnly birthday) : this(firstName, lastName)
        {
            if (birthday.Equals(new DateOnly())) throw new DomainException("Author birthday is not valid");

            Birthday = birthday;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            if (Birthday.HasValue) yield return Birthday;

            yield return FirstName;
            yield return LastName;
        }
    }
}
