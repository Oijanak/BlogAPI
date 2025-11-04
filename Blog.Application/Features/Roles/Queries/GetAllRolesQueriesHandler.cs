using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Roles.Queries
{
    public class GetAllRolesQueriesHandler : IRequestHandler<GetAllRolesQueries, Result<List<string>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public GetAllRolesQueriesHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<Result<List<string>>> Handle(GetAllRolesQueries request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(cancellationToken);
            return Result<List<string>>.Success(roles);
        }
    }
}
