namespace apbd_kolokwium1.Models.DTOs;

public class AddNewBook
{
    public string Title { get; set; }
    private List<AuthorsDto> AuthorsDtos { get; set; }
}