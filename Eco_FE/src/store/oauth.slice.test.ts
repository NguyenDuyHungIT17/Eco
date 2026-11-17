import { describe, expect, it } from 'vitest';
import { oauthActions, oauthReducer } from './oauth.slice';

// Tạo JWT giả (chỉ cần phần payload, jwt-decode không kiểm tra chữ ký)
function fakeJwt(payload: object) {
  const encode = (obj: object) =>
    btoa(JSON.stringify(obj)).replace(/=+$/, '').replace(/\+/g, '-').replace(/\//g, '_');
  return `${encode({ alg: 'HS256', typ: 'JWT' })}.${encode(payload)}.signature`;
}

describe('oauth.slice', () => {
  it('setToken giải mã profile, roles và permissions từ JWT', () => {
    const accessToken = fakeJwt({
      sub: 'user-1',
      unique_name: 'hungnd',
      email: 'hung@example.com',
      role: 'Admin',
      permission: ['admin.nguoi-dung.xem', 'admin.nguoi-dung.xoa'],
    });

    const state = oauthReducer(
      undefined,
      oauthActions.setToken({ accessToken, refreshToken: 'rt', expiresInSeconds: 3600 }),
    );

    expect(state.profile).toEqual({ id: 'user-1', username: 'hungnd', email: 'hung@example.com' });
    expect(state.roles).toEqual(['Admin']);
    expect(state.permissions).toEqual(['admin.nguoi-dung.xem', 'admin.nguoi-dung.xoa']);
    expect(state.refreshToken).toBe('rt');
  });

  it('removeToken trả về trạng thái chưa đăng nhập', () => {
    const loggedIn = oauthReducer(
      undefined,
      oauthActions.setToken({
        accessToken: fakeJwt({ sub: '1', unique_name: 'a', email: 'a@a.vn' }),
        refreshToken: 'rt',
        expiresInSeconds: 3600,
      }),
    );

    const state = oauthReducer(loggedIn, oauthActions.removeToken());

    expect(state.accessToken).toBeNull();
    expect(state.permissions).toEqual([]);
  });
});
