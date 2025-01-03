import {
  DashboardOutlined,
  FileAddOutlined,
  FolderOpenOutlined,
  LogoutOutlined,
  SettingOutlined
} from '@ant-design/icons';

export const PROFILE_MENU_ITEMS = [
  {
    key: '/settings',
    icon: <SettingOutlined />,
    label: 'Settings',
  },
  {
    key: 'logout',
    icon: <LogoutOutlined />,
    label: 'Logout',
  }
];

export const MAIN_MENU_ITEMS = [
  {
    key: '/',
    label: 'Dashboard',
    icon: <DashboardOutlined />
  },
  {
    key: '/users',
    label: 'Users',
    icon: <FolderOpenOutlined />
  },
  {
    key: '/tasks',
    label: 'Projects',
    icon: <FileAddOutlined />
  },
  {
    key: '/taskAssignment',
    label: 'Task Assignment',
    icon: <FileAddOutlined />
  },
  {
    key: '/assignedTask', 
    label: 'Assigned Task',
    icon: <FileAddOutlined />
  }

];