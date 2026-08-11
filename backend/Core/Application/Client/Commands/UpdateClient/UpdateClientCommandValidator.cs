using FluentValidation;

namespace Application.Client.Commands.UpdateClient
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommandRequest>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("FirstName obrigatório");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("LastName obrigatório");
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15).WithMessage("PhoneNumber obrigatório");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(255).WithMessage("Email obrigatório");
            RuleFor(x => x.DocumentNumber).NotEmpty().MaximumLength(20).WithMessage("DocumentNumber obrigatório");
            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Address obrigatório")
                .ChildRules(child =>
                {
                    child.RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(10).WithMessage("Address.PostalCode obrigatório");
                    child.RuleFor(x => x.AddressLine).NotEmpty().MaximumLength(200).WithMessage("Address.AddressLine obrigatório");
                    child.RuleFor(x => x.Number).NotEmpty().MaximumLength(10).WithMessage("Address.Number obrigatório");
                    child.RuleFor(x => x.Complement).MaximumLength(100);
                    child.RuleFor(x => x.Neighborhood).NotEmpty().MaximumLength(100).WithMessage("Address.Neighborhood obrigatório");
                    child.RuleFor(x => x.City).NotEmpty().MaximumLength(100).WithMessage("Address.City obrigatório");
                    child.RuleFor(x => x.State).NotEmpty().MaximumLength(2).WithMessage("Address.State obrigatório");
                });
        }
    }
}
