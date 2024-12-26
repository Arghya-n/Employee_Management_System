import Dashboard from '@pages/dashboard';
import Settings from '@pages/settings';
import Users from '@pages/users';
import UserCreate from '@pages/users/create';
import UserEdit from '@pages/users/edit';
import {
  DashboardBreadcrumb,
  DynamicUserBreadcrumb,
} from '@/routes/route-utils';
import Tasks from '@/pages/tasks';
import TaskAssignment from '@/pages/new';
import TaskCreate from '@/pages/tasks/create';

const routes = [
  {
    path: '',
    breadcrumb: DashboardBreadcrumb,
    component: Dashboard,
    exact: true,
    children: []
  },
  {
    path: 'users',
    breadcrumb: 'Users',
    component: '',
    exact: true,
    children: [
      {
        path: '',
        breadcrumb: 'Users',
        component: Users,
        exact: true
      },
      {
        path: 'create',
        breadcrumb: 'Create User',
        component: UserCreate,
        exact: true
      },
      {
        path: ':id',
        breadcrumb: DynamicUserBreadcrumb,
        component: UserEdit,
        exact: true
      }
    ]
  },
  {
    path: 'settings',
    breadcrumb: 'Settings',
    component: Settings,
    exact: true,
    children: []
  },
  {
    path: 'tasks',
    breadcrumb: 'Projects',
    exact: true,
    children: [
      {
        path: '',
        breadcrumb: 'Projects',
        component: Tasks,
        exact: true
      },
      {
        path : 'create',
        breadcrumb: 'Create Project',
        component: TaskCreate,
        exact: true
      }
    ]
  },
  {
    path: 'taskAssignment',
    breadcrumb: 'Task Assignment',
    component: TaskAssignment,
    exact: true,
    children: []
  }
];

export default routes;