import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Menu, MenuProps } from "antd";
import { MenuItem } from "@models/utils-model";
import { MAIN_MENU_ITEMS } from "@utils/constants/menu-constants";
import { useAppSelector } from "@/store";

interface MainMenuProps {
  activeMenu: string;
  onMenuClick: (key: string) => void;
}

const findParentKey = (
  key: string,
  menuItems: MenuItem[],
  parentKey: string = ""
): string | null => {
  for (const item of menuItems) {
    if (item.key === key) {
      return parentKey;
    }

    if (item.children) {
      const foundParentKey = findParentKey(key, item.children, item.key);
      if (foundParentKey) {
        return foundParentKey;
      }
    }
  }

  return null;
};

const findSelectedKey = (activeMenu: string) => {
  return "/" + activeMenu.split("/")[1];
};

const MainMenu: React.FC<MainMenuProps> = ({ activeMenu, onMenuClick }) => {
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const [openKeys, setOpenKeys] = useState<string[]>([]);

  const userRole = useAppSelector((state) => state.auth?.role || "customer");

  // Filter menu items based on the user role
  const filteredMenuItems = MAIN_MENU_ITEMS.filter((item) => {
    if (userRole === "Admin") {
      return item.key !== "/assignedTask"; // Exclude assignedTask for admin
    }
    if (userRole === "customer") {
      return item.key === "/" || item.key === "/assignedTask"; // Show only dashboard and assignedTask
    }
    return false; // Default to no menu items if role is unknown
  });

  useEffect(() => {
    const key = findSelectedKey(pathname);
    const newOpenKeys: string[] = [];
    const parentKey = findParentKey(key, MAIN_MENU_ITEMS);

    if (parentKey && !newOpenKeys.includes(parentKey)) {
      newOpenKeys.push(parentKey);
    }

    setOpenKeys(newOpenKeys);
  }, [pathname]);

  const handleClick: MenuProps["onClick"] = (e) => {
    if (pathname === e.key) {
      return;
    }

    onMenuClick(e.key);
    navigate(e.key);
  };

  const handleOpenChange = (keys: string[]) => {
    setOpenKeys(keys);
  };

  const selectedKey = findSelectedKey(activeMenu || location.pathname);

  return (
    <Menu
      theme="dark"
      mode="inline"
      selectedKeys={[selectedKey]}
      openKeys={openKeys}
      onOpenChange={handleOpenChange}
      items={filteredMenuItems}
      onClick={handleClick}
    />
  );
};

export default MainMenu;
