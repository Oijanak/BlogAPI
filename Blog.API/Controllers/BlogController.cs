using BlogApi.Application.Interfaces;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;
using BlogApi.Application.Dapper.Blogs.Commands.DeleteBlogWithDapperCommand;
using BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;
using BlogApi.Application.Dapper.Blogs.Queries;
using BlogApi.Application.Features.BlogFavorite.FavoriteBlogCommand;
using BlogApi.Application.Features.BlogFavorite.UnfavoriteBlogCommand;
using BlogApi.Application.Features.Blogs.Commands.ApproveStatusCommand;
using BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;
using BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;
using BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;
using BlogApi.Application.Features.Blogs.Queries.GetAllBlogCommentsQuery;
using BlogApi.Application.Features.Blogs.Queries.GetBlogDocumentQuery;
using BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;
using BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;
using BlogApi.Application.Features.Blogs.Queries.GetBlogReportPdfQuery;
using BlogApi.Application.Features.Blogs.Queries.GetBlogReportQuery;
using BlogApi.Application.Features.Rss;
using BlogApi.Application.SP.Blogs.Commands;
using BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;
using BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;
using MediatR;

namespace BlogApi.API.Controllers;

[Route("api/blogs")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly ISender  _sender;
    public BlogController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Roles="Maker")]
    
    public async Task<IActionResult> CreateBlog([FromForm]CreateBlogCommand blogBlogCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(blogBlogCommand));
    }

    [HttpPost("getAll")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBlogs(GetBlogListQuery getBlogListQuery)
    {
        return Ok(await _sender.Send(getBlogListQuery));
       
    }
    
    [HttpGet("rss")]
    public async Task<IActionResult> GetRssFeed()
    {
        var xml = await _sender.Send(new GetRssFeedQuery());
        return Content(xml, "application/rss+xml; charset=utf-8");
       
    }

    [HttpGet("{BlogId:guid}")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogById(GetBlogQuery blogQuery)
    {
        return Ok(await _sender.Send(blogQuery));
    }

    [HttpPut("{BlogId:guid}")]
    [Authorize(Policy = "BlogOwnerPolicy")]
    public async Task<IActionResult> UpdateBlog(UpdateBlogCommand updateBlogCommand)
    {
       return Ok(await _sender.Send(updateBlogCommand));
    }

    [HttpPatch("{BlogId:guid}/approve")]
    [Authorize(Roles = "Checker,Admin")]
    public async Task<IActionResult> ApproveBlog(ApproveStatusCommand approveStatusCommand)
    {
        return Ok(await _sender.Send(approveStatusCommand));
    }

    [HttpDelete("{BlogId:guid}")]
    [Authorize(Policy = "BlogOwnerPolicy")]
    public async Task<IActionResult> DeleteBlog(DeleteBlogCommand deleteBlogCommand)
    {
        return Ok(await _sender.Send(deleteBlogCommand));
    }

    [HttpGet("document/{BlogDocumentId:guid}")]
    public async Task<IActionResult> GetDocument(GetBlogDocumentQuery getBlogDocumentQuery)
    {
      return await _sender.Send(getBlogDocumentQuery);   
    }

    [HttpGet("{BlogId}/comments")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogComments(GetAllBlogCommentsQuery getAllBlogCommentsQuery)
    {
        return Ok(await _sender.Send(getAllBlogCommentsQuery));
    }


    [HttpPost("favorite/{BlogId:guid}")]
    [Authorize]
    public async Task<IActionResult> FavoriteBlog(FavoriteBlogCommand favoriteBlogCommand)
    {
        var response = await _sender.Send(favoriteBlogCommand);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("unfavorite/{BlogId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnFavoriteBlog(UnfavoriteBlogCommand unfavoriteBlogCommand)
    {
        var response = await _sender.Send(unfavoriteBlogCommand);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("export/excel/blog-report")]
    public async Task<IActionResult> ExportBlogReportExcel()
    {
        var result= await _sender.Send(new GetBlogReportExcelQuery());
        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BlogReport.xlsx");
    }
    
    [HttpGet("export/pdf/blog-report")]
    public async Task<IActionResult> ExportBlogReportPdf()
    {
        var result= await _sender.Send(new GetBlogReportPdfQuery());
        return File(result, "application/pdf", "BlogReport.pdf");
    }
    
    [Authorize]
    [HttpPost("sp")]
    public async Task<IActionResult> CreateBlogWithSp([FromForm]CreateBlogWithSpCommand createBlogWithSpCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createBlogWithSpCommand));
    }
    [Authorize]
    [HttpPut("sp/{BlogId:guid}")]
    public async Task<IActionResult> UpdateBlogWithSp(UpdateBlogWithSpCommand updateBlogWithSpCommand)
    {
        return Ok(await _sender.Send(updateBlogWithSpCommand));
       
    }

    [HttpDelete("sp/{BlogId:guid}")]
    public async Task<IActionResult> DeleteBlogWithSp(DeleteBlogWithSpCommand deleteBlogWithSpCommand)
    {
        return Ok(await _sender.Send(deleteBlogWithSpCommand));
    }

    [Authorize]
    [HttpPost("dapper")]
    public async Task<IActionResult> CreateBlogWithDapper([FromForm]CreateBlogWithDapperCommand createBlogWithDapperCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createBlogWithDapperCommand));
    }
    [HttpPost("dapper/query")]
    public async Task<IActionResult> GetBlogsWithDapper(GetAllBlogsQuery getAllBlogsQuery)
    {
        return Ok(await _sender.Send(getAllBlogsQuery));
    }
    
    [Authorize]
    [HttpPut("dapper/{BlogId:guid}")]
    public async Task<IActionResult> UpdateBlogWithDapper(UpdateBlogWithDappersCommand updateBlogWithDappersCommand)
    {
        return Ok(await _sender.Send(updateBlogWithDappersCommand));
    }

    [HttpDelete("dapper/{BlogId:guid}")]
    public async Task<IActionResult> DeleteBlogWithDapper(DeleteBlogWithDapperCommand deleteBlogWithDapperCommand)
    {
        return Ok(await _sender.Send(deleteBlogWithDapperCommand));
    }

}