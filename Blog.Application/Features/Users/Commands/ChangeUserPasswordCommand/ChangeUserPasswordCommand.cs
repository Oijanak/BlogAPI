using BlogApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Users.Commands.ChangeUserPasswordCommand
{
    public class ChangeUserPasswordCommand:IRequest<Result<string>>
    {
      
        public string OldPassword {  get; set; }

        public string NewPassword { get; set; }
    }
}
