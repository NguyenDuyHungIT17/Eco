import { Link } from 'react-router-dom';
import { Avatar, Dropdown, Layout, Space } from 'antd';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
import { useAppSelector } from 'store/hooks';

export function Header() {
  const profile = useAppSelector((s) => s.oauth.profile);

  return (
    <Layout.Header className="app-header">
      <Link className="brand" to="/">
        Eco.
      </Link>
      <Dropdown
        menu={{
          items: [
            {
              key: 'logout',
              icon: <LogoutOutlined />,
              label: <Link to="/logout">Đăng xuất</Link>,
            },
          ],
        }}
      >
        <Space className="app-header-user">
          <Avatar size="small" icon={<UserOutlined />} />
          {profile?.username}
        </Space>
      </Dropdown>
    </Layout.Header>
  );
}
