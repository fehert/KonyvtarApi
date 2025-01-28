using KonyvtarApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _libraryContext;

        public BooksController(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        [HttpGet]

        public async Task<ActionResult<Book>> GetAllBooks()
        {
            return Ok(await _libraryContext.Books.ToListAsync());
        }

        [HttpGet("byid")]
        public async Task<ActionResult<Book>> GetBooksById(int id)
        {
            return Ok(await _libraryContext.Books.FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            _libraryContext.Books.Add(book);
            await _libraryContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooksById), new { id = book.Id }, book);
        }
    }

}

