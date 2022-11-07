namespace ReserveTheBook.Domain.Tests
{
    public class ReservationTests
    {
        Book book;
        Author author;
        public ReservationTests() 
        {
            author = new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27));
            book = new Book("CLR via C#");
            book.AddAuthor(author);
        }

        [Fact(DisplayName = "Reservation book with comment should return active reservation with reserved status for a book")]
        public void Test1()
        {
            var reservation = new Reservation(book, "great book!");

            reservation.Id.Should().NotBeEmpty();
            reservation.Book.Id.Should().Be(book.Id);
            reservation.Comment.Should().Be("great book!");
            reservation.IsActive.Should().BeTrue();     
            reservation.ReservedAt.Should().BeBefore(DateTimeOffset.UtcNow);
            book.IsReserved.Should().BeTrue();
        }

        [Fact(DisplayName = "Delete reservation should return disabled reservation and book reserved status shoud be false")]
        public void Test2()
        {
            var reservation = new Reservation(book, "great book!");
            reservation.ReturnBook();

            reservation.IsActive.Should().BeFalse();
            reservation.ReturnedAt.Should().HaveValue();
            book.IsReserved.Should().BeFalse();
        }

        [Fact(DisplayName = "Add reservation for same book should return exception")]
        public void Test3()
        {
            var reservation = new Reservation(book, "great book!");
            Action action = () => new Reservation(book, "I want this book too!");

            action.Should().Throw<DomainException>().WithMessage("This book is alrady reserved");
        }

        [Fact(DisplayName = "Add reservation with empty comment should return exception")]
        public void Test4()
        {

            Action action = () => new Reservation(book, "");

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "Delete ended reservation should put isDeteted to true and add timestamp")]
        public void Test5()
        {
            var authors = new List<Author>
            {
                new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27))
            };

            var book = new Book("CLR via C#", authors);
            var reservation = new Reservation(book, "great book!");
            reservation.ReturnBook();
            reservation.Delete();

            reservation.IsDeleted.Should().BeTrue();
            reservation.DeletedAt.Should().HaveValue();
        }

        [Fact(DisplayName = "Delete active reservation should return exception")]
        public void Test6()
        {
            var authors = new List<Author>
            {
                new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27))
            };

            var book = new Book("CLR via C#", authors);
            var reservation = new Reservation(book, "great book!");
            Action action = () => reservation.Delete();

            action.Should().Throw<DomainException>().WithMessage("Cant't delete active reservation!");
        }

        [Fact(DisplayName = "Update ended reservation should return exception")]
        public void Test7()
        {
            var authors = new List<Author>
            {
                new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27))
            };

            var book = new Book("CLR via C#", authors);
            var reservation = new Reservation(book, "great book!");
            reservation.ReturnBook();

            Action action = () => reservation.Update("I disagree!");

            action.Should().Throw<DomainException>().WithMessage("Can't change comment for inactive reservation");
        }
    }
}
