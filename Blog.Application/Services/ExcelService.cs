using BlogApi.Application.DTOs;
using ClosedXML.Excel;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlogApi.Application.Interfaces;

namespace BlogApi.Application.Services
{
    public class ExcelService:IExcelService
    {
        public async Task<byte[]> GenerateBlogReportExcelAsync(List<BlogReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Blog Report");

            worksheet.Cell(1, 1).Value = "Blog Title";
            worksheet.Cell(1, 2).Value = "Author";
            worksheet.Cell(1, 3).Value = "Favorites";
            worksheet.Cell(1, 4).Value = "Comments";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.BlogTitle;
                worksheet.Cell(row, 2).Value = item.AuthorName;
                worksheet.Cell(row, 3).Value = item.FavoritesCount;
                worksheet.Cell(row, 4).Value = item.CommentsCount;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream); 
            stream.Position = 0;
            
            return await Task.FromResult(stream.ToArray());
        }
    }
}