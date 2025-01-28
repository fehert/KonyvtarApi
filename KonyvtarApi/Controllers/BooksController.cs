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

        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks(int oldal = 1, int oldaldb = 10)
        {
            var books = await _libraryContext.Books.Skip((oldal - 1) * oldaldb).Take(oldaldb).ToListAsync();
            return Ok(books);
        }

        [HttpGet("byid")]
        public async Task<ActionResult<Book>> GetBooksById(int id)
        {
            return Ok(await _libraryContext.Books.FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpGet("Title")]
        public async Task<ActionResult<Book>> GetBooksByTitle(string cim)
        {
            return Ok(await _libraryContext.Books.FirstOrDefaultAsync(x => x.Title == cim));
        }
        [HttpGet("Author")]
        public async Task<ActionResult<Book>> GetBooksByAuthor(string szerzo)
        {
            return Ok(await _libraryContext.Books.FirstOrDefaultAsync(x => x.Author == szerzo));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            _libraryContext.Books.Add(book);
            await _libraryContext.SaveChangesAsync();
            if (0 < book.PublishedYear && book.PublishedYear < DateTime.Now.Year && book.Title != null && book.Author != null)
            {
                return CreatedAtAction(nameof(GetBooksById), new { id = book.Id }, book);
            }
            return BadRequest();
        }
        [HttpPut("id")]
        public async Task<ActionResult<Book>> UpdateBook(int id, Book book)
        {
            var existingBook = await _libraryContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBook != null)
            {
                if (0 < book.PublishedYear && book.PublishedYear < DateTime.Now.Year && book.Title != null && book.Author != null)
                {
                    existingBook.PublishedYear = book.PublishedYear;
                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.Genre = book.Genre;
                    existingBook.Price = book.Price;
                    _libraryContext.Books.Update(existingBook);
                    await _libraryContext.SaveChangesAsync();
                    return Ok(existingBook);
                }
                return BadRequest("Hibás adatok");
            }
            return NotFound();
        }
        [HttpDelete("id")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _libraryContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book != null)
            {
                _libraryContext.Books.Remove(book);
                await _libraryContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();

        }

    }
}

