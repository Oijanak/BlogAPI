using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<ApiResponse<IEnumerable<UserDto>>>;