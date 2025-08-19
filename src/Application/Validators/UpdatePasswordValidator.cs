namespace Application.Validators
{
    using Application.Commands;
    using Application.Dtos.Request;
    using FluentValidation;

    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequestDto>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("A senha antiga é obrigatória.")
                .MinimumLength(6)
                .WithMessage("A senha antiga deve ter pelo menos 6 caracteres.")
                .MaximumLength(100)
                .WithMessage("A senha antiga deve ter no máximo 100 caracteres.");

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
