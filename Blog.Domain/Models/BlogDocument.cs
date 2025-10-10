namespace BlogApi.Domain.Models;

public class BlogDocument
{
    public Guid BlogDocumentId { get; set; } = Guid.NewGuid();
    public string DocumentName { get; set; }
    public string DocumentPath { get; set; }
    public string DocumentType { get; set; }
    public long DocumentSize { get; set; }
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}