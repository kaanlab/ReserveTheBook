namespace ReserveTheBook.Domain.Tests
{
    public class AuthorTests
    {
        [Fact(DisplayName = "Author without birthday should return author with null birthday value")]
        public void Test1()
        {
            var author = new Author("Jeffrey", "Richter");

            author.FirstName.Should().Be("Jeffrey");
            author.LastName.Should().Be("Richter");
            author.Birthday.Should().BeNull();
        }

        [Fact(DisplayName = "Author with correct fields should return author")]
        public void Test2()
        {
            var author = new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27));

            author.FirstName.Should().Be("Jeffrey");
            author.LastName.Should().Be("Richter");
            author.Birthday.Should().BeEquivalentTo(new DateOnly(1964, 7, 27));
        }


        [Theory(DisplayName = "Author with empty or null fields should return exception")]
        [MemberData(nameof(Data))]
        public void Test3(string firstName, string lastName, DateOnly birthday)
        {
            Action action = () => new Author(firstName, lastName, birthday);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "Author with default date value should return exception")]
        public void Test4()
        {
            Action action = () => new Author("Jeffrey", "Richter", new DateOnly());

            action.Should().Throw<DomainException>().WithMessage("Author birthday is not valid");
        }

        [Fact(DisplayName = "Same authors should be equals")]
        public void Test5()
        {
            var author1 = new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27));
            var author2 = new Author("Jeffrey", "Richter", new DateOnly(1964, 7, 27));

            author1.Should().Be(author2);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { null, "Richter", new DateOnly(1964,7,27) },
            new object[] { "", "Richter", new DateOnly(1964,7,27) },
            new object[] { "Jeffrey", "", new DateOnly(1964, 7, 27) },
            new object[] { "Jeffrey", null, new DateOnly(1964, 7, 27) }
        };
    }
}
