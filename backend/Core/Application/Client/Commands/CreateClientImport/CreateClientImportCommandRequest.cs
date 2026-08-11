using MediatR;
using System;
using System.IO;

namespace Application.Client.Commands.CreateClientImport
{
    public class CreateClientImportCommandRequest : IRequest<Guid>
    {
        public string FileName { get; set; }
        public Stream FileContent { get; set; }
    }
}
