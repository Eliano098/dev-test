import { SidebarItemsType } from "@/types/sidebar";
import { NAVIGATION_PATH } from "@/constants";
import { PROFILE_PERMISSIONS } from "@/constants/Permissions";
import { FaRegAddressBook } from "react-icons/fa";
import { FaChartLine, FaUsers } from "react-icons/fa6";

// PAGES
const DASHBOARD_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.DASHBOARD.ROOT, title: "Visão geral", icon: FaChartLine, permission: [...PROFILE_PERMISSIONS.clients] }
const CLIENTS_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.CLIENTS.LISTING.ABSOLUTE, title: "Clientes", icon: FaRegAddressBook, permission: [...PROFILE_PERMISSIONS.clients] }
const USERS_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.USERS.LISTING.ABSOLUTE, title: "Usuários", icon: FaUsers, permission: [...PROFILE_PERMISSIONS.users] }

export const SIDEBAR = [{
    title: "Gestão",
    pages: [DASHBOARD_PAGE, CLIENTS_PAGE, USERS_PAGE]
}];
