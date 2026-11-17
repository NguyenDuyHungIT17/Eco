import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Alert, App, Button, Form, Input, Typography } from 'antd';
import { authService, type RegisterRequest } from 'shared/services';
import { getAuthErrorMessage } from './Const';

export function Component() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { message } = App.useApp();

  async function onFinish(values: RegisterRequest) {
    setLoading(true);
    setError('');
    try {
      const result = await authService.register(values);
      message.success(result.message || 'Đăng ký thành công.');
      navigate('/login');
    } catch (err) {
      setError(getAuthErrorMessage(err));
      setLoading(false);
    }
  }

  // Rule giống RegisterRequestValidator bên BE
  return (
    <>
      <Typography.Title level={3}>Đăng ký</Typography.Title>
      {error && <Alert className="auth-alert" type="error" message={error} showIcon />}
      <Form layout="vertical" onFinish={onFinish} requiredMark={false} disabled={loading}>
        <Form.Item
          label="Họ và tên"
          name="fullName"
          rules={[{ required: true, message: 'Vui lòng nhập họ và tên.' }]}
        >
          <Input autoFocus autoComplete="name" />
        </Form.Item>
        <Form.Item
          label="Email"
          name="email"
          rules={[
            { required: true, message: 'Vui lòng nhập email.' },
            { type: 'email', message: 'Email không hợp lệ.' },
          ]}
        >
          <Input autoComplete="email" />
        </Form.Item>
        <Form.Item
          label="Số điện thoại"
          name="phoneNumber"
          rules={[{ required: true, message: 'Vui lòng nhập số điện thoại.' }]}
        >
          <Input autoComplete="tel" />
        </Form.Item>
        <Form.Item
          label="Tên đăng nhập"
          name="username"
          rules={[
            { required: true, message: 'Vui lòng nhập tên đăng nhập.' },
            { min: 3, message: 'Tên đăng nhập tối thiểu 3 ký tự.' },
          ]}
        >
          <Input autoComplete="username" />
        </Form.Item>
        <Form.Item
          label="Mật khẩu"
          name="password"
          rules={[
            { required: true, message: 'Vui lòng nhập mật khẩu.' },
            { min: 6, message: 'Mật khẩu tối thiểu 6 ký tự.' },
          ]}
        >
          <Input.Password autoComplete="new-password" />
        </Form.Item>
        <Button type="primary" htmlType="submit" loading={loading} block>
          Tạo tài khoản
        </Button>
      </Form>
      <p className="auth-switch">
        Đã có tài khoản? <Link to="/login">Đăng nhập</Link>
      </p>
    </>
  );
}
