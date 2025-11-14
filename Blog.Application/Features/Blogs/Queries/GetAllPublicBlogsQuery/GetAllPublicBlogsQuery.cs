using BlogApi.Application.DTOs;
using BlogApi.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllPulicBlogsQuery
{
    public record GetAllPublicBlogsQuery(

     int Page = 1,

     int Limit = 10,

     string? Search = null,

     string? SortBy = "CreatedAt",
     string? SortOrder = "desc",
     string? Author=null,
     List<string>? CategoryNames=null

 ) : IRequest<ApiResponse<IEnumerable<BlogPublicDto>>>;
}