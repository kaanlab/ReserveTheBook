using Microsoft.AspNetCore.Mvc;
using ReserveTheBook.WebApi.Models.Book;
using ReserveTheBook.WebApi.Services;

namespace ReserveTheBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) 
        {
            _bookService = bookService;
        }

        /// <summary>Get all books</summary>
        /// <response code="200">List of books</response>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
            Ok(await _bookService.GetAll());

        /// <summary>Get books available for reservation</summary>
        /// <response code="200">List of books</response>
        [HttpGet]
        [Route("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailable() =>
            Ok(await _bookService.GetAvailable());


        /// <summary>Get book by Id</summary>
        /// <param name="bookId">book Id (Guid)</param>
        /// <response code="200">Return object</response>
        /// <response code="404">Object not found</response>
        [HttpGet]
        [Route("{bookId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid bookId)
        {
            var book = await this._bookService.GetById(bookId);
            return book is not null ? this.Ok(book) : this.NotFound();
        }

        /// <summary>Create new book</summary>
        /// <param name="request"></param>
        /// <response code="200">Return new object</response>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateBookRequest request)
            => this.Ok(await this._bookService.Create(request));

        /// <summary>Update existed book</summary>
        /// <param name="bookId">book Id (Guid)</param>
        /// <param name="request"></param>
        /// <response code="200">Return updated object</response>
        /// <response code="404">Object not found</response>
        [HttpPut]
        [Route("update/{bookId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid bookId, UpdateBookRequest request)
        {
            var book = await this._bookService.GetById(bookId);
            return book is not null
                ? this.Ok(await this._bookService.Update(bookId, request))
                : this.NotFound();
        }

        /// <summary>Add authors to book</summary>
        /// <param name="bookId">book Id (Guid)</param>
        /// <param name="request"></param>
        /// <response code="200">Return updated object</response>
        /// <response code="404">Object not found</response>
        [HttpPut]
        [Route("{bookId:Guid}/add-authors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAuthors(Guid bookId, AuthorsRequest request)
        {
            var book = await this._bookService.GetById(bookId);
            return book is not null
                ? this.Ok(await this._bookService.AddAuthors(bookId, request))
                : this.NotFound();
        }

        /// <summary>Remove authors from book</summary>
        /// <param name="bookId">book Id (Guid)</param>
        /// <param name="request"></param>
        /// <response code="200">Return updated object</response>
        /// <response code="404">Object not found</response>
        [HttpPut]
        [Route("{bookId:Guid}/remove-authors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveAuthors(Guid bookId, AuthorsRequest request)
        {
            var book = await this._bookService.GetById(bookId);
            return book is not null
                ? this.Ok(await this._bookService.RemoveAuthors(bookId, request))
                : this.NotFound();
        }

        /// <summary>Delete book</summary>
        /// <param name="bookId">book Id (Guid)</param>
        /// <response code="204">Delete succeeded</response>
        /// <response code="404">Object not found</response>
        [HttpDelete]
        [Route("delete/{bookId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid bookId)
        {
            var book = await this._bookService.GetById(bookId);
            if (book is null)
            {
                return this.NotFound();
            }

            await this._bookService.Delete(bookId);

            return this.NoContent();
        }
    }
}
