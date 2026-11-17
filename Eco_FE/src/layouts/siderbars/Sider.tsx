import { Link, useLocation } from 'react-router-dom';
import { Layout, Menu } from 'antd';
import { useAbility } from '@casl/react';
import { AbilityContext } from 'shared/services/CaslService';
import { menuItems } from '../menu';

export function Sider() {
  const ability = useAbility(AbilityContext);
  const location = useLocation();

  const items = menuItems
    .filter((item) => !item.permission || ability.can(item.permission, 'all'))
    .map((item) => ({
      key: item.path,
      icon: item.icon,
      label: <Link to={item.path}>{item.title}</Link>,
    }));

  // Chọn mục menu có path dài nhất khớp với URL hiện tại
  const selected = menuItems
    .map((item) => item.path)
    .filter((path) => location.pathname === path || location.pathname.startsWith(`${path}/`))
    .sort((a, b) => b.length - a.length)[0];

  return (
    <Layout.Sider width={220} className="app-sider" breakpoint="lg" collapsedWidth={0}>
      <Menu mode="inline" selectedKeys={selected ? [selected] : []} items={items} />
    </Layout.Sider>
  );
}
