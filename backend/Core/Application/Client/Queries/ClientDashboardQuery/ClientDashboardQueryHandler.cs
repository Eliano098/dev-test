using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Queries.ClientDashboardQuery
{
    public class ClientDashboardQueryHandler : IRequestHandler<ClientDashboardQueryRequest, ClientDashboardQueryResponse>
    {
        private readonly IClientControlContext _context;

        public ClientDashboardQueryHandler(IClientControlContext context)
        {
            _context = context;
        }

        public async Task<ClientDashboardQueryResponse> Handle(ClientDashboardQueryRequest request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;
            var clients = await _context.Clients
                .AsNoTracking()
                .Select(client => new { client.BirthDate, client.CreatedAt })
                .ToListAsync(cancellationToken);

            var ages = clients.Select(client => CalculateAge(client.BirthDate, today)).ToList();
            var ageRangeCounts = ages
                .GroupBy(GetAgeRange)
                .ToDictionary(group => group.Key, group => group.Count());
            var ageRanges = AgeRangeNames
                .Select(name => new ClientAgeRangeResponse
                {
                    Name = name,
                    Count = ageRangeCounts.GetValueOrDefault(name)
                })
                .ToList();

            return new ClientDashboardQueryResponse
            {
                TotalClients = clients.Count,
                NewClientsLast30Days = clients.Count(client => client.CreatedAt >= today.AddDays(-30)),
                AverageAge = ages.Count == 0 ? 0 : (int)Math.Round(ages.Average()),
                AgeRanges = ageRanges
            };
        }

        private static readonly IReadOnlyCollection<string> AgeRangeNames = new[]
        {
            "0 a 17 anos",
            "18 a 29 anos",
            "30 a 44 anos",
            "45 anos ou mais"
        };

        private static int CalculateAge(DateTime birthDate, DateTime today)
        {
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private static string GetAgeRange(int age)
        {
            if (age < 18) return "0 a 17 anos";
            if (age < 30) return "18 a 29 anos";
            if (age < 45) return "30 a 44 anos";

            return "45 anos ou mais";
        }
    }
}
