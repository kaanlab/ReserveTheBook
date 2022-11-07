namespace ReserveTheBook.Domain.Models
{
    public sealed class Reservation : Entity
    {
        public Book Book { get; private set; }
        public string Comment { get; private set; }
        public DateTimeOffset ReservedAt { get; private set; }
        public DateTimeOffset? ReturnedAt { get; private set; }
        public bool IsActive { get; private set; }

        private Reservation()
        {

        }

        public Reservation(Book book, string comment)
        {
            if (book.IsReserved) throw new DomainException("This book is alrady reserved");
            if (string.IsNullOrEmpty(comment)) throw new ArgumentNullException("comment");

            book.IsReserved = true;

            Id = Guid.NewGuid();
            ReservedAt = DateTimeOffset.UtcNow;
            Comment = comment;
            Book = book;
            IsActive = true;
        }

        public void Update(string comment)
        {
            if (string.IsNullOrEmpty(comment)) throw new ArgumentNullException("comment");
            if (!IsActive) throw new DomainException("Can't change comment for inactive reservation");

            Comment = comment;
        }

        public void ReturnBook()
        {
            Book.IsReserved = false;
            ReturnedAt = DateTimeOffset.UtcNow;
            IsActive = false;
        }

        public override void Delete()
        {
            if(IsActive) throw new DomainException("Cant't delete active reservation!");

            base.Delete();
        }

    }
}
