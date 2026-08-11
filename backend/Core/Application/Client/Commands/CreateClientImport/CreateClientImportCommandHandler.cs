using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Commands.CreateClientImport
{
    public class CreateClientImportCommandHandler : IRequestHandler<CreateClientImportCommandRequest, Guid>
    {
        private readonly IClientControlContext _context;
        private readonly IClientImportFileStorage _fileStorage;

        public CreateClientImportCommandHandler(IClientControlContext context, IClientImportFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task<Guid> Handle(CreateClientImportCommandRequest request, CancellationToken cancellationToken)
        {
            var filePath = await _fileStorage.SaveAsync(request.FileName, request.FileContent, cancellationToken);
            var clientImport = new Domain.ClientImport(request.FileName, filePath);

            try
            {
                await _context.ClientImports.AddAsync(clientImport, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _fileStorage.DeleteAsync(filePath, cancellationToken);
                throw;
            }

            return clientImport.Id;
        }
    }
}
