import SidebarNavSection from "./SidebarNavSection";
import { SIDEBAR } from "@/components/sidebar/dashboardItems";

const SidebarNav = () => {
  return (
    <ul className="sidebar-nav">
      {SIDEBAR.map((item) => (
        <SidebarNavSection
          key={item.title}
          pages={item.pages}
          title={item.title}
        />
      ))}
    </ul>
  );
};

export default SidebarNav;
