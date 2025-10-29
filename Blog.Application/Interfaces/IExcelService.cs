using BlogApi.Application.DTOs;

namespace BlogApi.Application.Interfaces;

public interface IExcelService
{
    Task<byte[]> GenerateBlogReportExcelAsync(List<BlogReportDto> data);
}