namespace apbd_kolokwium1.Models.DTOs;

public class BooksDto
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public class GenresDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class AuthorsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class BooksGenresDto
{
    public BooksDto Book { get; set; }
    public GenresDto Genre { get; set; }
}

public class BooksAuthorsDto
{
    public BooksDto Book { get; set; }
    public List<AuthorsDto> Authors { get; set; }
}