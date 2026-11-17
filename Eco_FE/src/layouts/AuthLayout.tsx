import { Navigate, Outlet } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';

// Khung cho trang login / register. Đã đăng nhập thì về trang chủ.
export function Component() {
  const accessToken = useAppSelector((s) => s.oauth.accessToken);

  if (accessToken) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="auth-layout">
      <section className="auth-intro">
        <p className="eyebrow">ECO / TÀI KHOẢN</p>
        <h1>Chào mừng đến Eco</h1>
        <p className="intro-copy">Đăng nhập hoặc đăng ký để bắt đầu sử dụng hệ thống.</p>
      </section>
      <section className="auth-panel">
        <Outlet />
      </section>
    </div>
  );
}
