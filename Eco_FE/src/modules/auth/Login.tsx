import { useState } from 'react';
import {
  authService,
  type AuthResponse,
  type RegisterRequest,
} from 'shared/services';

export default function Login() {
  const [mode, setMode] = useState<'login' | 'register'>('login');

  const [username, setUsername] = useState('');
  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');

  const [token, setToken] = useState<AuthResponse | null>(null);
  const [notice, setNotice] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  async function submit(event: React.FormEvent) {
    event.preventDefault();

    setLoading(true);
    setError('');
    setNotice('');

    try {
      if (mode === 'register') {
        const data: RegisterRequest = {
          username,
          email,
          password,
          fullName,
          phoneNumber,
        };

        const result = await authService.register(data);

        setNotice(
          result.message ||
            'Đăng ký thành công. Bạn có thể đăng nhập.',
        );

        setMode('login');
      } else {
        const result = await authService.login({
          usernameOrEmail: username,
          password,
          deviceInfo: navigator.userAgent,
          ipAddress: '',
        });

        setToken(result);
      }
    } catch (submitError) {
      setError(
        submitError instanceof Error
          ? submitError.message
          : 'Có lỗi xảy ra, vui lòng thử lại.',
      );
    } finally {
      setLoading(false);
    }
  }

  async function refresh() {
    if (!token) {
      return;
    }

    setLoading(true);
    setError('');

    try {
      const result = await authService.refreshToken(
        token.refreshToken,
      );

      setToken(result);
      setNotice('Đã làm mới token.');
    } catch (refreshError) {
      setError(
        refreshError instanceof Error
          ? refreshError.message
          : 'Không thể làm mới token.',
      );
    } finally {
      setLoading(false);
    }
  }

  async function logout() {
    if (!token) {
      return;
    }

    setLoading(true);
    setError('');

    try {
      await authService.revokeToken(token.refreshToken);

      setToken(null);
      setNotice('Đã đăng xuất.');
    } catch (logoutError) {
      setError(
        logoutError instanceof Error
          ? logoutError.message
          : 'Không thể đăng xuất.',
      );
    } finally {
      setLoading(false);
    }
  }

  /*
   * Nếu đã đăng nhập
   */
  if (token) {
    return (
      <section className="auth-panel">
        <h2>Phiên đang hoạt động</h2>

        <p>
          Token còn hiệu lực khoảng{' '}
          {Math.ceil(token.expiresInSeconds / 60)} phút.
        </p>

        <div className="session-actions">
          <button onClick={refresh} disabled={loading}>
            Làm mới token
          </button>

          <button onClick={logout} disabled={loading}>
            Đăng xuất
          </button>
        </div>

        {error && <p className="error">{error}</p>}

        {notice && <p className="success">{notice}</p>}
      </section>
    );
  }

  /*
   * Login / Register
   */
  return (
    <section className="auth-panel">
      <div className="auth-tabs">
        <button
          className={mode === 'login' ? 'active' : ''}
          onClick={() => setMode('login')}
        >
          Đăng nhập
        </button>

        <button
          className={mode === 'register' ? 'active' : ''}
          onClick={() => setMode('register')}
        >
          Đăng ký
        </button>
      </div>

      <form onSubmit={submit} className="form">
        {mode === 'register' && (
          <>
            <label>
              Họ và tên
              <input
                value={fullName}
                onChange={(event) =>
                  setFullName(event.target.value)
                }
                required
              />
            </label>

            <label>
              Email
              <input
                type="email"
                value={email}
                onChange={(event) =>
                  setEmail(event.target.value)
                }
                required
              />
            </label>

            <label>
              Số điện thoại
              <input
                value={phoneNumber}
                onChange={(event) =>
                  setPhoneNumber(event.target.value)
                }
                required
              />
            </label>
          </>
        )}

        <label>
          {mode === 'login'
            ? 'Tên đăng nhập hoặc email'
            : 'Tên đăng nhập'}

          <input
            value={username}
            onChange={(event) =>
              setUsername(event.target.value)
            }
            required
          />
        </label>

        <label>
          Mật khẩu
          <input
            type="password"
            value={password}
            onChange={(event) =>
              setPassword(event.target.value)
            }
            required
          />
        </label>

        <button type="submit" disabled={loading}>
          {loading
            ? 'Đang gửi...'
            : mode === 'login'
              ? 'Đăng nhập'
              : 'Tạo tài khoản'}
        </button>
      </form>

      {error && <p className="error">{error}</p>}

      {notice && <p className="success">{notice}</p>}
    </section>
  );
}