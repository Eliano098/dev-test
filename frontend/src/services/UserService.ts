import { BaseService } from "./BaseService";
import { User } from "@/types/api/User";
import { UserProfile } from "@/types/api/enums/UserProfile";

type UserResponse = Omit<User, "profile"> & {
    profile: keyof typeof UserProfile;
};

class UserService extends BaseService {
    constructor() {
        super("user");
    }

    async getAll(): Promise<User[]> {
        const users = await this.get<UserResponse[]>("");
        return users.map(this.toUser);
    }

    async create(user: User): Promise<{ id: string }> {
        return await this.post<User, { id: string }>("", user);
    }

    async getById(id: string): Promise<User> {
        const user = await this.get<UserResponse>(id);
        return this.toUser(user);
    }

    async update(id: string, user: User): Promise<void> {
        await this.put<User, void>(id, user);
    }

    private toUser(user: UserResponse): User {
        return {
            ...user,
            profile: UserProfile[user.profile],
        };
    }
}

export default new UserService();
