import axios, { AxiosError, type AxiosRequestConfig } from 'axios';
import { API_BASE_URL } from 'app-setting';
import { store } from 'store/store';
import { oauthActions, type AuthTokens } from 'store/oauth.slice';
import { ApiError, type ApiResponse } from 'shared/types/api';

type RetryConfig = AxiosRequestConfig & { _retry?: boolean };

const REFRESH_URL = '/api/auth/refresh-token';

const instance = axios.create({ baseURL: API_BASE_URL });

// 1. Tự gắn access token vào mọi request
instance.interceptors.request.use((config) => {
  const token = store.getState().oauth.accessToken;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// 2. Gặp 401 → refresh đúng 1 lần, các request khác chờ chung Promise này
let refreshing: Promise<string | null> | null = null;

async function refreshAccessToken(): Promise<string | null> {
  const refreshToken = store.getState().oauth.refreshToken;
  if (!refreshToken) {
    return null;
  }

  try {
    const res = await axios.post<ApiResponse<AuthTokens>>(`${API_BASE_URL}${REFRESH_URL}`, {
      refreshToken,
    });
    const tokens = res.data.data!;
    store.dispatch(oauthActions.setToken(tokens));
    return tokens.accessToken;
  } catch {
    store.dispatch(oauthActions.removeToken());
    return null;
  }
}

instance.interceptors.response.use(
  (res) => res,
  async (error: AxiosError<ApiResponse<unknown>>) => {
    const original = error.config as RetryConfig | undefined;
    // Không refresh cho các API auth: login sai mật khẩu cũng trả 401
    const isAuthApi = original?.url?.startsWith('/api/auth/');

    if (error.response?.status === 401 && original && !original._retry && !isAuthApi) {
      original._retry = true;
      refreshing ??= refreshAccessToken().finally(() => {
        refreshing = null;
      });
      const newToken = await refreshing;
      if (newToken) {
        return instance(original);
      }
    }

    const body = error.response?.data;
    throw new ApiError(
      error.response?.status ?? 0,
      body?.message ?? 'Không kết nối được máy chủ.',
      body?.errorCode,
      body?.errors ?? [],
    );
  },
);

// 3. Bóc ApiResponse → trả thẳng data
async function request<T>(config: AxiosRequestConfig): Promise<T> {
  const res = await instance.request<ApiResponse<T>>(config);
  return res.data.data as T;
}

export const http = {
  get: <T>(url: string, params?: object, config?: AxiosRequestConfig) =>
    request<T>({ ...config, method: 'get', url, params }),
  post: <T>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    request<T>({ ...config, method: 'post', url, data }),
  put: <T>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    request<T>({ ...config, method: 'put', url, data }),
  delete: <T>(url: string, config?: AxiosRequestConfig) =>
    request<T>({ ...config, method: 'delete', url }),
};
