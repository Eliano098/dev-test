export type ClientDashboard = {
    totalClients: number;
    newClientsLast30Days: number;
    averageAge: number;
    ageRanges: ClientAgeRange[];
};

export type ClientAgeRange = {
    name: string;
    count: number;
};
