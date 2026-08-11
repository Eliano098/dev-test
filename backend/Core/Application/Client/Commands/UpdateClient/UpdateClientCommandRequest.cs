using Application.Client.Models;
using MediatR;
using System;

namespace Application.Client.Commands.UpdateClient
{
    public class UpdateClientCommandRequest : ClientModel, IRequest
    {
        public Guid Id { get; set; }
    }
}
