import { Navigate, Outlet, useLocation, useNavigation } from 'react-router-dom';
import { Layout } from 'antd';
import { useAppSelector } from 'store/hooks';
import { FallbackElement } from 'shared/components';
import { Header } from './headers/Header';
import { Sider } from './siderbars/Sider';

// Giống AuthorizedLayout của Frontends, nhưng chuyển về /login của Eco thay vì SSO
export function Component() {
  const accessToken = useAppSelector((s) => s.oauth.accessToken);
  const location = useLocation();
  const navigation = useNavigation();

  if (!accessToken) {
    const returnUrl = encodeURIComponent(location.pathname + location.search);
    return <Navigate to={`/login?returnUrl=${returnUrl}`} replace />;
  }

  return (
    <Layout className="app-layout">
      <Header />
      <Layout>
        <Sider />
        <Layout.Content className="app-content">
          {navigation.state === 'loading' ? <FallbackElement /> : <Outlet />}
        </Layout.Content>
      </Layout>
    </Layout>
  );
}
