namespace Application.Validators
{
    using Application.Commands;
    using Application.Dtos.Request;
    using FluentValidation;

    public class RecoverPasswordValidator : AbstractValidator<RecoverPasswordRequestDto>
    {
        public RecoverPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O e-mail deve ter no máximo 100 caracteres.")
                .EmailAddress()
                .WithMessage("E-mail Inválido.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("A nova senha é obrigatória.")
                .MinimumLength(6)
                .WithMessage("A nova senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100)
                .WithMessage("A nova senha deve ter no máximo 100 caracteres.");
        }
    }
}
