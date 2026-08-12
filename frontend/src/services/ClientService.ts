import { BaseService } from "./BaseService";
import { Client } from "@/types/api/Client";
import { ClientDashboard } from "@/types/api/ClientDashboard";

class ClientService extends BaseService {
  constructor() {
    super("client");
  }

  async getAll(document?: string): Promise<Client[]> {
    return await this.get<Client[]>("", { document });
  }

  async getDashboard(): Promise<ClientDashboard> {
    return await this.get<ClientDashboard>("dashboard");
  }

  async create(client: Client): Promise<string> {
    return await this.post<Client, string>("", client);
  }

  async getById(id: string): Promise<Client> {
    return await this.get<Client>(id);
  }

  async update(id: string, client: Client): Promise<void> {
    return await this.put<Client, void>(id, client);
  }

  async importCsv(file: File): Promise<void> {
    const data = new FormData();
    data.append("file", file);

    await this.post<FormData, void>("import", data);
  }
}

export default new ClientService();
