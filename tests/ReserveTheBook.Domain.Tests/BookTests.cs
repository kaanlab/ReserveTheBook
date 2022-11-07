namespace ReserveTheBook.Domain.Tests
{
    public class BookTests
    {
        [Fact(DisplayName = "Book with title should return book")]
        public void Test1()
        {
            var book = new Book("CLR via C#");

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("CLR via C#");
            book.Authors.Should().BeEmpty();
            book.IsReserved.Should().BeFalse();
        }

        [Fact(DisplayName = "Book with empty title should return exception")]
        public void Test2()
        {
            Action action = () => new Book("");

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "Add author to book should return book with author")]
        public void Test3()
        {
            var author = new Author("Jeffrey", "Richter ", new DateOnly(1964, 7, 27));
            var book = new Book("CLR via C#");
            book.AddAuthor(author);

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("CLR via C#");
            book.Authors.Count.Should().Be(1);
            book.IsReserved.Should().BeFalse();
        }

        [Fact(DisplayName = "Add the same author to book should return exception")]
        public void Test4()
        {
            var author = new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27));
            var book = new Book("CLR via C#");
            book.AddAuthor(author);
            Action action = () => book.AddAuthor(author);

            action.Should().Throw<DomainException>().WithMessage("Can't add author! Jeffrey Richter is already in the authors of this book");
        }

        [Fact(DisplayName = "Add authors to book should return book with authors")]
        public void Test5()
        {
            var authors = new List<Author>
            {
                new Author("Erich", "Gamma"),
                new Author("Richard", "Helm"),
                new Author("Ralph", "Johnson"),
                new Author("John", "Vlissides")
            };

            var book = new Book("Gangs of Four Design Patterns", authors);

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("Gangs of Four Design Patterns");
            book.Authors.Count.Should().Be(4);
        }

        [Fact(DisplayName = "Remove author from book should return book without this author")]
        public void Test6()
        {
            var authors = new List<Author>
            {
                new Author("Erich", "Gamma"),
                new Author("Richard", "Helm"),
                new Author("Ralph", "Johnson"),
                new Author("John", "Vlissides")
            };

            var someAuthor = new Author("Some", "Author");
            authors.Add(someAuthor);

            var book = new Book("Gangs of Four Design Patterns", authors);

            book.RemoveAuthor(someAuthor);

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("Gangs of Four Design Patterns");
            book.Authors.Count.Should().Be(4);
            book.Authors.ForEach(author => author.Should().NotBe(someAuthor));
        }

        [Fact(DisplayName = "Remove author from book should return book without this author")]
        public void Test7()
        {
            var authors = new List<Author>
            {
                new Author("Erich", "Gamma"),
                new Author("Richard", "Helm"),
                new Author("Ralph", "Johnson"),
                new Author("John", "Vlissides")
            };

            var someAuthor = new Author("Some", "Author");

            var book = new Book("Gangs of Four Design Patterns", authors);
            Action action = () => book.RemoveAuthor(someAuthor);

            action.Should().Throw<DomainException>().WithMessage("Can't remove author! Some Author not in the authors of this book");
        }

        [Fact(DisplayName = "Remove authors from book should return book without this authors")]
        public void Test8()
        {
            var authors = new List<Author>
            {
                new Author("Erich", "Gamma"),
                new Author("Richard", "Helm"),
                new Author("Ralph", "Johnson"),
                new Author("John", "Vlissides")
            };

            var book = new Book("Gangs of Four Design Patterns", authors);
            book.RemoveAuthors(authors);

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("Gangs of Four Design Patterns");
            book.Authors.Count.Should().Be(0);

        }

        [Fact(DisplayName = "Add authors to book should return book with these authors")]
        public void Test9()
        {
            var authors = new List<Author>
            {
                new Author("Erich", "Gamma"),
                new Author("Richard", "Helm"),
                new Author("Ralph", "Johnson"),
                new Author("John", "Vlissides")
            };

            var book = new Book("Gangs of Four Design Patterns");
            book.AddAuthors(authors);

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("Gangs of Four Design Patterns");
            book.Authors.Count.Should().Be(4);

        }

        [Fact(DisplayName = "Update title should return same book with updated title")]
        public void Test10()
        {
            var authors = new List<Author>
            {
                new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27))
            };

            var book = new Book("CLR via Python", authors);
            book.Update("CLR via C#");

            book.Id.Should().NotBeEmpty();
            book.Title.Should().Be("CLR via C#");
            book.Authors.Count.Should().Be(1);
        }

        [Fact(DisplayName = "Delete not reserved book should put isDeteted to true adn and timestamp")]
        public void Test11()
        {
            var authors = new List<Author>
            {
                new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27))
            };

            var book = new Book("CLR via C#", authors);
            book.Delete();

            book.IsDeleted.Should().BeTrue();
            book.DeletedAt.Should().HaveValue();
        }

        [Fact(DisplayName = "Delete reserved book should return exception")]
        public void Test12()
        {
            var authors = new List<Author>
            {
                new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27))
            };

            var book = new Book("CLR via C#", authors);
            var reservation = new Reservation(book, "great book!");
            Action action = () => book.Delete();

            action.Should().Throw<DomainException>().WithMessage("Can't delete book! It's reserved");
        }
    }
}