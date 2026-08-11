using FluentValidation;

namespace Application.Client.Commands.CreateClientImport
{
    public class CreateClientImportCommandValidator : AbstractValidator<CreateClientImportCommandRequest>
    {
        public CreateClientImportCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .Must(name => name.EndsWith(".csv", System.StringComparison.OrdinalIgnoreCase))
                .WithMessage("Arquivo CSV obrigatório");

            RuleFor(x => x.FileContent).NotNull().WithMessage("Arquivo CSV obrigatório");
        }
    }
}
