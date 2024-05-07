using System.Data;
using apbd_kolokwium1.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace apbd_kolokwium1.Repositories;

public class BooksRepository : IBooksAuthorsRepository
{
    private readonly IConfiguration _configuration;

    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<bool> DoesBookExist(int bookId)
    {
        var query = "select 1 from books where pk = @id";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", bookId);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task<bool> DoesBookHaveAuthors(int bookId)
    {
        var query = "select 1 from books_authors where fk_book = @id";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", bookId);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }


    public async Task<BooksAuthorsDto> GetBookAuthors(int bookId)
    {
        var query = "select title, first_name, last_name from authors join books_authors on fk_author = authors.pk join books on fk_book = books.pk where fk_book = @id";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", bookId);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        var titleOrdinal = reader.GetOrdinal("title");
        var firstNameOrdinal = reader.GetOrdinal("first_name");
        var lastNameOrdinal = reader.GetOrdinal("last_name");

        BooksAuthorsDto booksAuthorsDto = null;

        while (await reader.ReadAsync())
        {
            if (booksAuthorsDto is not null)
            {
                booksAuthorsDto.Authors.Add(new AuthorsDto()
                {
                    FirstName = reader.GetString(firstNameOrdinal),
                    LastName = reader.GetString(lastNameOrdinal)
                });
            }
            else
            {
                booksAuthorsDto = new BooksAuthorsDto()
                {
                    Book = new BooksDto() { Id = bookId, Title = reader.GetString(titleOrdinal) },
                    Authors = new List<AuthorsDto>()
                    {
                        new AuthorsDto()
                        {
                            FirstName = reader.GetString(firstNameOrdinal),
                            LastName = reader.GetString(lastNameOrdinal)
                        }
                    }
                };
            }
        }

        if (booksAuthorsDto is null) throw new Exception();
        return booksAuthorsDto;
    }

    public async Task<BooksDto> AddBook(BooksAuthorsDto booksAuthorsDto)
    {
        var query = "insert into books values, @title); select @@identity as IdProductWarehouse";
        
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@title", booksAuthorsDto);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return new BooksDto();
    }

    public Task<AuthorsDto> AddAuthors(List<AuthorsDto> list)
    {
        throw new NotImplementedException();
    }
}

public interface IBooksAuthorsRepository
{
    Task<bool> DoesBookExist(int bookId);
    Task<bool> DoesBookHaveAuthors(int bookId);
    Task<BooksAuthorsDto> GetBookAuthors(int bookId);
    Task<BooksDto> AddBook(BooksAuthorsDto booksAuthorsDto);
    Task<AuthorsDto> AddAuthors(List<AuthorsDto> list);

}