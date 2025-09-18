using BlogApi.Domain.Models;

namespace BlogApi.Domain.Common;

public class BaseEntity
{
    public string CreatedBy{ get; set; } = string.Empty;
    public User CreatedByUser { get; set; } = null!;

    public string? UpdatedBy { get; set; }
    public User? UpdatedByUser { get; set; } 
}