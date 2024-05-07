using apbd_kolokwium1.Models.DTOs;
using apbd_kolokwium1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace apbd_kolokwium1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class booksController : ControllerBase
{
    private readonly BooksRepository _booksRepository;

    public booksController(BooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }

    [HttpGet("{id}/authors")]
    public async Task<IActionResult> GetBooksAuthors(int id)
    {
        if (!await _booksRepository.DoesBookExist(id))
            return NotFound($"Book with ID {id} not found.");

        if (!await _booksRepository.DoesBookHaveAuthors(id))
        {
            return NotFound($"No authors found for book with ID {id}");
        }

        BooksAuthorsDto result = await _booksRepository.GetBookAuthors(id);

        return Ok(result);
    }
    
    [HttpPost("{id}")]
    public async Task<IActionResult> AddBook(int id)
    {
        if (!await _booksRepository.DoesBookExist(id))
            return NotFound($"Book with ID {id} not found.");

        if (!await _booksRepository.DoesBookHaveAuthors(id))
        {
            return NotFound($"No authors found for book with ID {id}");
        }

        BooksAuthorsDto result = await _booksRepository.GetBookAuthors(id);

        return Ok(result);
    }
}