using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlogApi.Infrastructure.Services;

public class FileService:IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IBlogDbContext _blogDbContext;
    private readonly string _uploadFolder;

    public FileService(IWebHostEnvironment environment,IBlogDbContext blogDbContext,IOptions<FileStorageOptions> options)
    {
        _environment = environment;
        _blogDbContext = blogDbContext;
        _uploadFolder = Path.Combine(_environment.ContentRootPath, options.Value.UploadFolder);
    }

    public async Task<List<BlogDocument>> UploadFilesAsync(IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
            return new List<BlogDocument>();
        
       

        if (!Directory.Exists(_uploadFolder))
            Directory.CreateDirectory(_uploadFolder);

        var uploadedDocuments = new List<BlogDocument>();

        foreach (var file in files)
        {
            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(_uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new BlogDocument
            {
                DocumentName = uniqueFileName,
                DocumentPath = filePath, 
                DocumentType = file.ContentType,
                DocumentSize = file.Length
            };

            uploadedDocuments.Add(document);
           
        }
       

        return uploadedDocuments;
    }
    
    public async Task<FileStreamResult?> GetFileByIdAsync(Guid fileId)
    {
        var document = await _blogDbContext.BlogDocument.FindAsync(fileId);
        if (document == null)
            return null;

        
        if (!System.IO.File.Exists(document.DocumentPath))
            return null;

        
        var stream = new FileStream(document.DocumentPath, FileMode.Open, FileAccess.Read);
        var contentType = document.DocumentType ?? "application/octet-stream";

        return new FileStreamResult(stream, contentType);
    }

    public async Task<List<BlogDocument>> UpdateFilesAsync(Guid blogId, List<IFormFile> files)
    {
        var documents = await _blogDbContext.BlogDocument
            .Where(d => d.BlogId == blogId)
            .ToListAsync();
        
        foreach (var doc in documents)
        {
            if (File.Exists(doc.DocumentPath))
                File.Delete(doc.DocumentPath); 

            _blogDbContext.BlogDocument.Remove(doc);
        }

        var newDocuments = await UploadFilesAsync(files);

        
        foreach (var doc in newDocuments)
        {
            _blogDbContext.BlogDocument.Add(doc);
        }
        
        return newDocuments;
    }

    public async Task DeleteFilesAsync(Guid blogId)
    {
        var documents = await _blogDbContext.BlogDocument
            .Where(d => d.BlogId == blogId)
            .ToListAsync();
        
        foreach (var doc in documents)
        {
            if (File.Exists(doc.DocumentPath))
                File.Delete(doc.DocumentPath); 
        }
    }
}