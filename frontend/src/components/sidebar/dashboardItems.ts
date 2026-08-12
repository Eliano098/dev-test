import { SidebarItemsType } from "@/types/sidebar";
import { NAVIGATION_PATH } from "@/constants";
import { PROFILE_PERMISSIONS } from "@/constants/Permissions";
import { FaRegAddressBook } from "react-icons/fa";
import { FaUsers } from "react-icons/fa6";

// PAGES
const CLIENTS_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.CLIENTS.LISTING.ABSOLUTE, title: "Clientes", icon: FaRegAddressBook, permission: [...PROFILE_PERMISSIONS.clients] }
const USERS_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.USERS.LISTING.ABSOLUTE, title: "Usuários", icon: FaUsers, permission: [...PROFILE_PERMISSIONS.users] }

export const SIDEBAR = [{
    title: "Gestão",
    pages: [CLIENTS_PAGE, USERS_PAGE]
}];
