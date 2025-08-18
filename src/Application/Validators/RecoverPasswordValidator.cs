namespace Application.Validators
{
    using Application.Commands;
    using FluentValidation;

    public class RecoverPasswordValidator : AbstractValidator<RecoverPasswordCommand>
    {
        public RecoverPasswordValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é Obrigatório!")
                .GreaterThan(0)
                .WithMessage("Id deve ser maior que zero!");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
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
