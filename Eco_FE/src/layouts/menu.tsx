import type { ReactNode } from 'react';
import { HomeOutlined } from '@ant-design/icons';

export type MenuItem = {
  path: string;
  title: string;
  icon?: ReactNode;
  // Bỏ trống = ai đăng nhập cũng thấy
  permission?: string;
};

// Thêm module mới: thêm 1 dòng ở đây + 1 route trong AppRoutes.tsx
export const menuItems: MenuItem[] = [{ path: '/', title: 'Trang chủ', icon: <HomeOutlined /> }];
