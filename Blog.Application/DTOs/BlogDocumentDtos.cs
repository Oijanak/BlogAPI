namespace BlogApi.Application.DTOs;

public class BlogDocumentDto
{
    public Guid BlogDocumentId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentType { get; set; }
}