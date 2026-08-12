import { UserProfile } from "@/types/api/enums/UserProfile";

export const PROFILE_PERMISSIONS = {
    clients: [UserProfile.Administrator, UserProfile.Operator],
    users: [UserProfile.Administrator],
} as const satisfies Record<string, readonly UserProfile[]>;
