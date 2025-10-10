using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Interfaces;

public interface IFileService
{
    Task<List<BlogDocument>> UploadFilesAsync(IEnumerable<IFormFile> files);
    Task<FileStreamResult?> GetFileByIdAsync(Guid fileId);
    
    Task<List<BlogDocument>> UpdateFilesAsync(Guid blogId, List<IFormFile> files);
    
    Task DeleteFilesAsync(Guid blogId);
}