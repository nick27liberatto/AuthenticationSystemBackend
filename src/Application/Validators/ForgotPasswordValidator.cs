namespace Application.Validators
{
    using Application.Commands;
    using Application.Constants.Validation;
    using FluentValidation;

    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage(ValidationRules.DtoRequired);

            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage(ValidationRules.EmailRequired)
                .EmailAddress().WithMessage(ValidationRules.EmailValidRequired);
        }
    }
}
