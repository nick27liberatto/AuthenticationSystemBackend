namespace Application.Validators
{
    using Application.Commands;
    using Application.Constants.Validation;
    using FluentValidation;

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage(ValidationRules.DtoRequired);

            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage(ValidationRules.EmailRequired)
                .EmailAddress().WithMessage(ValidationRules.EmailValidRequired);

            RuleFor(x => x.Dto.Token)
                .NotEmpty().WithMessage(ValidationRules.TokenRequired);

            RuleFor(x => x.Dto.NewPassword)
                .NotEmpty().WithMessage(ValidationRules.PasswordRequired)
                .MinimumLength(8).WithMessage(ValidationRules.PasswordMinLengthRequired)
                .Matches("[A-Z]").WithMessage(ValidationRules.PasswordUppercaseRequired)
                .Matches("[a-z]").WithMessage(ValidationRules.PasswordLowercaseRequired)
                .Matches("[0-9]").WithMessage(ValidationRules.PasswordNumberRequired)
                .Matches("[^a-zA-Z0-9]").WithMessage(ValidationRules.PasswordSpecialCharacterRequired);
        }
    }
}
