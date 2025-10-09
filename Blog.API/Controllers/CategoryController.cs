using BlogApi.Application.Features.Category.Commands;
using BlogApi.Application.Features.Category.Commands.UpdateCategoryCommand;
using BlogApi.Application.Features.Category.Queries.GetAllCategoryQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[ApiController]
[Route("api/categories")]
public class CategoryController: ControllerBase
{
    private readonly ISender  _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> createCategory(CreateCategoryCommand createCategoryCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createCategoryCommand));
    }

    [HttpPut("{CategoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> updateCategory(UpdateCategoryCommand updateCategoryCommand)
    {
        return Ok(await _sender.Send(updateCategoryCommand));
    }

    [HttpDelete("{CategoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> deleteCategory(DeleteCategoryCommand deleteCategoryCommand)
    {
        return Ok(await _sender.Send(deleteCategoryCommand));
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return Ok(await _sender.Send(new GetAllCategoryQuery()));
    }
    
}