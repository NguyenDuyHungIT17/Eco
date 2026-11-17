import { useState } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { Alert, Button, Form, Input, Typography } from 'antd';
import { authService, type LoginRequest } from 'shared/services';
import { getAuthErrorMessage } from './Const';

export function Component() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  async function onFinish(values: LoginRequest) {
    setLoading(true);
    setError('');
    try {
      await authService.login(values);
      navigate(searchParams.get('returnUrl') || '/', { replace: true });
    } catch (err) {
      setError(getAuthErrorMessage(err));
      setLoading(false);
    }
  }

  return (
    <>
      <Typography.Title level={3}>Đăng nhập</Typography.Title>
      {error && <Alert className="auth-alert" type="error" message={error} showIcon />}
      <Form layout="vertical" onFinish={onFinish} requiredMark={false} disabled={loading}>
        <Form.Item
          label="Tên đăng nhập hoặc email"
          name="usernameOrEmail"
          rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập hoặc email.' }]}
        >
          <Input autoFocus autoComplete="username" />
        </Form.Item>
        <Form.Item
          label="Mật khẩu"
          name="password"
          rules={[{ required: true, message: 'Vui lòng nhập mật khẩu.' }]}
        >
          <Input.Password autoComplete="current-password" />
        </Form.Item>
        <Button type="primary" htmlType="submit" loading={loading} block>
          Đăng nhập
        </Button>
      </Form>
      <p className="auth-switch">
        Chưa có tài khoản? <Link to="/register">Đăng ký</Link>
      </p>
    </>
  );
}
