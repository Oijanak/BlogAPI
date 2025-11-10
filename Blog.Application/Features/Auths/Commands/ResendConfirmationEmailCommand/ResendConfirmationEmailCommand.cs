using BlogApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Auths.Commands.ResendConfirmationEmailCommand
{
    public record ResendConfirmationEmailCommand(string Email) : IRequest<Result<string>>;
}
