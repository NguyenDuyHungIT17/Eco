import { createBrowserRouter } from 'react-router-dom';
import { NotFoundPage } from 'shared/components';

// Mỗi route chỉ khai báo 1 lần. Module được tải khi mở trang (lazy) và phải export `Component`.
export const router = createBrowserRouter([
  // Trang công khai
  {
    lazy: () => import('layouts/AuthLayout'),
    children: [
      { path: '/login', lazy: () => import('modules/auth/Login') },
      { path: '/register', lazy: () => import('modules/auth/Register') },
    ],
  },
  { path: '/logout', lazy: () => import('modules/logout') },

  // Trang cần đăng nhập
  {
    path: '/',
    lazy: () => import('layouts/MainLayout'),
    children: [
      { index: true, lazy: () => import('modules/home') },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
]);
