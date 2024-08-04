namespace BankingApp.AuthServer.Validators
{
    using BankingApp.AuthServer.ViewModels;
    using FluentValidation;

    public class RegisterAdminValidator : AbstractValidator<AdminRegistrationViewModel>
    {
        public RegisterAdminValidator()
        {
            Include(new RegisterUserValidator());

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.");
        }
    }
}
