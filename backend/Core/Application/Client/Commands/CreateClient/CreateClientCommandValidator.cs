using FluentValidation;

namespace Application.Client.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommandRequest>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage((obj, propertyValue) => $"FirstName obrigatório");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage((obj, propertyValue) => $"LastName obrigatório");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(15)
                .WithMessage((obj, propertyValue) => $"PhoneNumber obrigatório");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage((obj, propertyValue) => $"Email obrigatório");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage((obj, propertyValue) => $"DocumentNumber obrigatório");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage((obj, propertyValue) => $"Address obrigatório")
                .ChildRules(child =>
                {
                    child
                        .RuleFor(x => x.PostalCode)
                        .NotEmpty()
                        .MaximumLength(10)
                        .WithMessage((obj, propertyValue) => $"Address.PostalCode obrigatório");

                    child
                        .RuleFor(x => x.AddressLine)
                        .NotEmpty()
                        .MaximumLength(200)
                        .WithMessage((obj, propertyValue) => $"Address.AddressLine obrigatório");

                    child
                        .RuleFor(x => x.Number)
                        .NotEmpty()
                        .MaximumLength(10)
                        .WithMessage((obj, propertyValue) => $"Address.Number obrigatório");

                    child
                        .RuleFor(x => x.Complement)
                        .MaximumLength(100);

                    child
                        .RuleFor(x => x.Neighborhood)
                        .NotEmpty()
                        .MaximumLength(100)
                        .WithMessage((obj, propertyValue) => $"Address.Neighborhood obrigatório");

                    child
                        .RuleFor(x => x.City)
                        .NotEmpty()
                        .MaximumLength(100)
                        .WithMessage((obj, propertyValue) => $"Address.City obrigatório");

                    child
                        .RuleFor(x => x.State)
                        .NotEmpty()
                        .MaximumLength(2)
                        .WithMessage((obj, propertyValue) => $"Address.State obrigatório");
                });
        }
    }
}
