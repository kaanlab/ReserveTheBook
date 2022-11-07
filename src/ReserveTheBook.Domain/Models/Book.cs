using System.Collections.ObjectModel;

namespace ReserveTheBook.Domain.Models
{
    public sealed class Book : Entity
    {
        public string Title { get; private set; }
        public bool IsReserved { get; internal set; }
        public List<Author> Authors { get; private set; } = new();

        private Book() { }

        public Book(string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException("title");

            Id = Guid.NewGuid();
            Title = title;
            IsReserved = false;
        }

        public Book(string title, List<Author> authors) : this(title)
            => authors.ForEach(x => Authors.Add(x));

        public void AddAuthor(Author author)
        {
            if (Authors.Any(x => x.Equals(author)))
            {
                throw new DomainException($"Can't add author! {author.FirstName} {author.LastName} is already in the authors of this book") ;
            }

            Authors.Add(author);
        }

        public void AddAuthors(List<Author> authors)
            => authors.ForEach(x => AddAuthor(x));

        public void RemoveAuthor(Author author) 
        {
            if (Authors.Any(x => x.Equals(author)))
            {
                Authors.Remove(author);
            }
            else
            {
                throw new DomainException($"Can't remove author! {author.FirstName} {author.LastName} not in the authors of this book");
            }
        }

        public void RemoveAuthors(List<Author> authors)
            => authors.ForEach(x => RemoveAuthor(x));

        public void Update(string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException("title");

            Title = title;
        }

        public override void Delete()
        {
            if(this.IsReserved) 
            {
                throw new DomainException("Can't delete book! It's reserved");
            }

            base.Delete();

        }
    }
}