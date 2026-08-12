using System.Collections.Generic;

namespace Application.Client.Queries.ClientDashboardQuery
{
    public class ClientDashboardQueryResponse
    {
        public int TotalClients { get; set; }
        public int NewClientsLast30Days { get; set; }
        public int AverageAge { get; set; }
        public IEnumerable<ClientAgeRangeResponse> AgeRanges { get; set; }
    }

    public class ClientAgeRangeResponse
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
