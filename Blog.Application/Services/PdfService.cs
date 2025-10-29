using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BlogApi.Application.Services;

   public class PdfService : IPdfService
    {
        public async Task<byte[]> GenerateBlogReportPdfAsync(List<BlogReportDto> data)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    
                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80); 
                            });
                            
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("S.N.").Bold();
                                header.Cell().Element(CellStyle).Text("Blog Title").Bold();
                                header.Cell().Element(CellStyle).Text("Author").Bold();
                                header.Cell().Element(CellStyle).Text("Favorites").Bold();
                                header.Cell().Element(CellStyle).Text("Comments").Bold();
                            });

                            
                            foreach (var (item, index) in data.Select((item, index) => (item, index)))
                            {
                                table.Cell().Element(CellStyle).Text((index + 1).ToString());
                                table.Cell().Element(CellStyle).Text(item.BlogTitle);
                                table.Cell().Element(CellStyle).Text(item.AuthorName);
                                table.Cell().Element(CellStyle).Text(item.FavoritesCount.ToString());
                                table.Cell().Element(CellStyle).Text(item.CommentsCount.ToString());
                            }
                            static IContainer CellStyle(IContainer container)
                                => container.Border(1).Padding(10);
                        });
                });
            });
            
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return await Task.FromResult(stream.ToArray());
        }
    }