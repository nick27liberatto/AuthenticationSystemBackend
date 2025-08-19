namespace Application.Validators
{
    using Application.Dtos.Request;
    using Application.Queries;
    using FluentValidation;

    public class LoginUserValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O e-mail deve ter no máximo 100 caracteres.")
                .EmailAddress()
                .WithMessage("E-mail Inválido.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(6)
                .WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100)
                .WithMessage("A senha deve ter no máximo 100 caracteres.");
        }
    }
}
