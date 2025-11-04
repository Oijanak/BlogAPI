using BlogApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Users.Commands.AddUserAsMakerCommand
{
    public class AddUserAsMakerCommand:IRequest<Result<string>>
    {
        public string UserId {  get; set; }

    }
}
