import { http } from 'shared/utils/http';
import { store } from 'store/store';
import { oauthActions, type AuthTokens } from 'store/oauth.slice';

export type LoginRequest = {
  usernameOrEmail: string;
  password: string;
};

export type RegisterRequest = {
  username: string;
  email: string;
  password: string;
  fullName: string;
  phoneNumber: string;
};

export const authService = {
  async login(data: LoginRequest) {
    // BE hiện vẫn đọc deviceInfo/ipAddress từ body (cột Browser bắt buộc).
    // Bỏ 2 trường này khi BE Pha 2 tự lấy từ HttpContext.
    const tokens = await http.post<AuthTokens>('/api/auth/login', {
      ...data,
      deviceInfo: navigator.userAgent,
      ipAddress: '',
    });
    store.dispatch(oauthActions.setToken(tokens));
    return tokens;
  },

  register(data: RegisterRequest) {
    return http.post<{ message: string }>('/api/auth/register', data);
  },

  async logout() {
    const refreshToken = store.getState().oauth.refreshToken;
    try {
      if (refreshToken) {
        // BE hiện nhận body là chuỗi JSON trần; đổi thành { refreshToken } khi BE Pha 2 sửa contract
        await http.post('/api/auth/revoke-token', JSON.stringify(refreshToken), {
          headers: { 'Content-Type': 'application/json' },
        });
      }
    } finally {
      store.dispatch(oauthActions.removeToken());
    }
  },
};
