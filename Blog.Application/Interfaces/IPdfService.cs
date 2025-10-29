using BlogApi.Application.DTOs;

namespace BlogApi.Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateBlogReportPdfAsync(List<BlogReportDto> data);
}