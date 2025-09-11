using BlogApi.Domain.Models;

namespace BlogApi.Application.DTOs;

public class AuthorRequest
{
    public string AuthorName { get; set; }
    public string AuthorEmail { get; set; }
    public int Age { get; set; }
}
public class AuthorDto
{
    public Guid AuthorId { get; set; }
    public string AuthorEmail { get; set; }
    public string AuthorName { get; set; }
    public int Age { get; set; }
    public AuthorDto(){}
    public AuthorDto(Author author)
    {
        AuthorId = author.AuthorId;
        AuthorEmail = author.AuthorEmail;
        AuthorName = author.AuthorName;
        Age = author.Age;
        
    }

    public AuthorDto(Guid authorId, string authorEmail, string authorName, int age)
    {
        AuthorId = authorId;
        AuthorEmail = authorEmail;
        AuthorName = authorName;
        Age = age;
    }
}

