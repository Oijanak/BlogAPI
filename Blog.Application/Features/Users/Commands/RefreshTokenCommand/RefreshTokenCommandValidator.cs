using FluentValidation;

namespace BlogApi.Application.Features.Users.Commands.RefreshTokenCommand;

public class RefreshTokenCommandValidator:AbstractValidator<RefreshTokenCommand>
{
 
   public RefreshTokenCommandValidator()
   {
      RuleFor(x => x.AccessToken)
         .NotEmpty().WithMessage("Access token is required.")
         .Must(CheckJwtFormat).WithMessage("Invalid access token format.");

      RuleFor(x => x.RefreshToken)
         .NotEmpty().WithMessage("Refresh token is required.")
         .MinimumLength(88).WithMessage("Refresh token is too short."); 
   }
   private bool CheckJwtFormat(string token)
   {
      if (string.IsNullOrWhiteSpace(token)) return false;
      var parts = token.Split('.');
      return parts.Length == 3; 
   }

   
}