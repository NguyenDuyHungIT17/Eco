# Kế hoạch sửa Eco_FE

> Mục tiêu: đưa Eco_FE về **cấu trúc giống Frontends** (dự án mẫu ở công ty),
> **bỏ những chỗ mẫu làm chưa tốt**, và **theo cách làm phổ biến** của các dự án React hiện nay.
>
> Đọc kèm: [FE_LEARNING_ROADMAP.md](FE_LEARNING_ROADMAP.md) (lộ trình học).

---

## Mục lục

1. [Tóm tắt: giữ gì, sửa gì, bỏ gì](#1-tóm-tắt)
2. [So sánh 3 cách: Eco hiện tại · Mẫu · Phổ biến](#2-so-sánh)
3. [Cấu trúc thư mục đích](#3-cấu-trúc-thư-mục-đích)
4. [Các bước sửa (làm theo thứ tự)](#4-các-bước-sửa)
5. [Mẫu một module CRUD](#5-mẫu-một-module-crud)
6. [Quy ước code](#6-quy-ước-code)
7. [Checklist tổng](#7-checklist-tổng)

---

## 1. Tóm tắt

| ✅ Giữ của Eco | 📋 Học theo mẫu | ❌ Không học theo mẫu |
|---|---|---|
| Vite (mẫu dùng CRA đã bị khai tử) | Chia `modules / shared / layouts / store` | Khai báo route trùng lặp |
| TypeScript (mẫu dùng JS) | Router `lazy` theo module | `client_secret` nằm trong code FE |
| Alias import (`shared/...`) | `AuthorizedLayout` chặn trang | Mỗi danh mục 1 Redux slice (35+ file giống nhau) |
| | `HttpClient` tự gắn token + tự refresh | RxJS `subscribe` ở khắp nơi |
| | `BaseService` có sẵn CRUD | `console.log`, code comment bỏ dở |
| | Module 6 file: `Const / Service / List / Form / Module / index` | Gọi API trong `useEffect` + tự quản lý `loading` |
| | Phân quyền bằng CASL + nút có `perm` | Tên file sai chính tả (`htppUtils`, `selectedKey.sice`) |
| | antd làm thư viện giao diện | |

---

## 2. So sánh

| Chủ đề | Eco hiện tại | Mẫu Frontends | Phổ biến hiện nay | **Eco nên dùng** |
|---|---|---|---|---|
| Build | Vite | Create React App | Vite | **Vite** |
| Ngôn ngữ | TS | JS | TS | **TS** |
| UI | CSS thuần | antd 5 | antd (admin) / shadcn + Tailwind | **antd** (giống mẫu, hợp màn admin) |
| Router | Không có | React Router 6, `createBrowserRouter` + `lazy` | React Router 7 / TanStack Router | **React Router** (`createBrowserRouter` + `lazy`) |
| State đăng nhập | biến JS + localStorage | Redux Toolkit + redux-persist | Redux Toolkit hoặc Zustand | **Redux Toolkit + redux-persist** (giống mẫu) |
| Dữ liệu từ API (list, detail) | `useState` | Redux slice cho từng màn | **TanStack Query** | Giai đoạn đầu: `useState` như mẫu → sau: **TanStack Query** |
| HTTP | `fetch` | axios + RxJS | axios / ky + Promise | **axios + Promise** (`async/await`) |
| Form | `useState` từng ô | antd `Form` | React Hook Form + Zod / antd Form | **antd `Form`** |
| Phân quyền | Không có | CASL | CASL / tự viết | **CASL** |
| Thông báo | `useState` error | `BehaviorsService` (RxJS) | antd `App.useApp()` (message, modal) | **antd `App.useApp()`** |
| Cấu hình URL | `import.meta.env` | `public/app-config.js` | `.env` / runtime config | **`.env`** (thêm runtime config nếu cần deploy nhiều nơi) |
| Lint / format | Không có | ESLint (airbnb cũ) | ESLint flat config + Prettier | **ESLint + Prettier** |
| Test | Không có | Không có thật | Vitest + Testing Library + MSW | **Vitest + Testing Library** |

> 💡 **Vì sao không dùng RxJS như mẫu?**
> RxJS mạnh nhưng khó học và hiếm gặp trong React hiện nay. `async/await` làm được
> mọi việc mẫu đang làm, kể cả hàng đợi refresh token (xem bước 3).

---

## 3. Cấu trúc thư mục đích

```text
Eco_FE/
├─ .env.example
├─ eslint.config.js
├─ .prettierrc
└─ src/
   ├─ main.tsx                    ← điểm khởi động
   ├─ App.tsx                     ← Provider: Redux, antd, Router
   ├─ AppRoutes.tsx               ← khai báo toàn bộ route
   ├─ app-setting.ts              ← URL API, hằng số cấu hình
   │
   ├─ layouts/
   │  ├─ MainLayout.tsx           ← AuthorizedLayout + Header + Sider + <Outlet/>
   │  ├─ AuthLayout.tsx           ← khung cho trang login/register
   │  ├─ headers/Header.tsx
   │  └─ siderbars/Sider.tsx
   │
   ├─ modules/                    ← mỗi thư mục = 1 nghiệp vụ
   │  ├─ auth/                    Login.tsx · Register.tsx · index.ts
   │  ├─ logout/                  Logout.tsx · index.ts
   │  ├─ home/                    Dashboard.tsx · index.ts
   │  └─ admin.nguoi-dung/        Const.ts · Service.ts · List.tsx · Form.tsx · Module.tsx · index.ts
   │
   ├─ shared/
   │  ├─ components/              Button (có perm) · NotFoundPage · FallbackElement
   │  ├─ services/                BaseService.ts · AuthService.ts · CaslService.ts · index.ts
   │  ├─ types/                   api.ts (ApiResponse, PagedResult, ApiError)
   │  └─ utils/http/              httpClient.ts · index.ts
   │
   ├─ store/                      oauth.slice.ts · reducers.ts · store.ts · hooks.ts · index.ts
   └─ styles/                     global.css
```

**Quy tắc đặt file**

| Đặt ở | Khi nào |
|---|---|
| `modules/<tên>/` | Chỉ module đó dùng |
| `shared/` | Từ 2 module trở lên dùng |
| `store/` | State cần dùng ở nhiều nơi **và** cần giữ khi chuyển trang (token, user) |
| `useState` trong component | Mọi thứ còn lại (giá trị form, loading, modal đang mở…) |

---

## 4. Các bước sửa

Làm lần lượt. Mỗi bước xong thì chạy `npm run dev` thấy chạy được rồi mới sang bước sau.

### Bước 1 — Cài thư viện & công cụ

```powershell
cd D:\Hung\Eco\Eco_FE
npm i react-router-dom @reduxjs/toolkit react-redux redux-persist axios antd dayjs jwt-decode @casl/ability @casl/react
npm i -D eslint @eslint/js typescript-eslint eslint-plugin-react-hooks prettier vitest @testing-library/react @testing-library/jest-dom jsdom
```

Thêm vào `package.json`:

```json
"scripts": {
  "dev": "vite",
  "build": "tsc --noEmit && vite build",
  "preview": "vite preview",
  "lint": "eslint src",
  "format": "prettier --write src",
  "test": "vitest"
}
```

> `tsc --noEmit` trước khi build giúp **bắt lỗi kiểu** — hiện tại `vite build` không kiểm tra type.

Sửa `.env`: **xóa** `VITE_API_BASE` để request đi qua proxy của Vite (tránh lỗi CORS). Tạo `.env.example` ghi chú các biến.

---

### Bước 2 — `shared/types/api.ts`: kiểu dữ liệu khớp với BE

```ts
// Khớp với ApiResponse<T> bên Eco.Application
export type ApiResponse<T> = {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
  errorCode?: string;
};

export type PagedResult<T> = {
  data: T[];
  meta: { page: number; pageSize: number; total: number };
};

export type PagedRequest = {
  page: number;
  pageSize: number;
  search?: string;
  sort?: Record<string, 1 | -1>;
  filter?: Record<string, unknown>;
};

export class ApiError extends Error {
  constructor(
    public status: number,
    message: string,
    public errorCode?: string,
    public errors: string[] = [],
  ) {
    super(message);
  }
}
```

---

### Bước 3 — `store/`: Redux Toolkit (giống mẫu, có TypeScript)

**`store/oauth.slice.ts`**

```ts
import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import { jwtDecode } from 'jwt-decode';

export type AuthTokens = {
  accessToken: string;
  refreshToken: string;
  expiresInSeconds: number;
};

type JwtPayload = {
  sub: string;
  unique_name: string;
  email: string;
  role?: string | string[];
  permission?: string | string[];
};

export type OauthState = {
  accessToken: string | null;
  refreshToken: string | null;
  profile: { id: string; username: string; email: string } | null;
  roles: string[];
  permissions: string[];
};

const initialState: OauthState = {
  accessToken: null,
  refreshToken: null,
  profile: null,
  roles: [],
  permissions: [],
};

const toArray = (v?: string | string[]) => (v ? (Array.isArray(v) ? v : [v]) : []);

const oauthSlice = createSlice({
  name: 'oauth',
  initialState,
  reducers: {
    setToken: (_state, action: PayloadAction<AuthTokens>) => {
      const { accessToken, refreshToken } = action.payload;
      const jwt = jwtDecode<JwtPayload>(accessToken);
      return {
        accessToken,
        refreshToken,
        profile: { id: jwt.sub, username: jwt.unique_name, email: jwt.email },
        roles: toArray(jwt.role),
        permissions: toArray(jwt.permission),
      };
    },
    removeToken: () => initialState,
  },
});

export const { actions: oauthActions, reducer: oauthReducer } = oauthSlice;
```

> Khác mẫu: mẫu gọi thêm API để lấy quyền. Eco **đã có quyền trong JWT** (claim `permission`) nên chỉ cần giải mã.
>
> ⚠️ Claim role trong JWT của .NET có tên dài (`http://schemas.microsoft.com/ws/2008/06/identity/claims/role`).
> Khi làm, mở token trên [jwt.io](https://jwt.io) để xem đúng tên claim rồi sửa `JwtPayload`.

**`store/store.ts`**

```ts
import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { persistReducer, persistStore, FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import { oauthReducer } from './oauth.slice';

const rootReducer = combineReducers({ oauth: oauthReducer });

export const store = configureStore({
  reducer: persistReducer({ key: 'eco', storage, whitelist: ['oauth'] }, rootReducer),
  middleware: (getDefault) =>
    getDefault({ serializableCheck: { ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER] } }),
});

export const persistor = persistStore(store);
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
```

**`store/hooks.ts`** — dùng thay cho `useSelector` / `useDispatch` để có gợi ý kiểu:

```ts
import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from './store';

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();
```

> ❌ Khác mẫu: **không** tạo slice cho từng danh mục (`danToc.slice`, `tonGiao.slice`…).
> Dữ liệu danh sách để trong component (hoặc TanStack Query ở giai đoạn sau).

---

### Bước 4 — `shared/utils/http/`: axios + tự refresh token

Làm đúng việc `HttpClient` của mẫu làm, nhưng dùng Promise thay cho RxJS.

**`shared/utils/http/httpClient.ts`**

```ts
import axios, { AxiosError, type AxiosRequestConfig } from 'axios';
import { API_BASE_URL } from 'app-setting';
import { store } from 'store/store';
import { oauthActions, type AuthTokens } from 'store/oauth.slice';
import { ApiError, type ApiResponse } from 'shared/types/api';

const instance = axios.create({ baseURL: API_BASE_URL });

// 1. Tự gắn token vào mọi request
instance.interceptors.request.use((config) => {
  const token = store.getState().oauth.accessToken;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// 2. Gặp 401 → refresh 1 lần, các request khác chờ chung 1 Promise
let refreshing: Promise<string | null> | null = null;

async function refreshAccessToken(): Promise<string | null> {
  const refreshToken = store.getState().oauth.refreshToken;
  if (!refreshToken) return null;
  try {
    const res = await axios.post<ApiResponse<AuthTokens>>(
      `${API_BASE_URL}/api/auth/refresh-token`,
      { refreshToken },
    );
    store.dispatch(oauthActions.setToken(res.data.data!));
    return res.data.data!.accessToken;
  } catch {
    store.dispatch(oauthActions.removeToken());
    return null;
  }
}

instance.interceptors.response.use(
  (res) => res,
  async (error: AxiosError<ApiResponse<unknown>>) => {
    const original = error.config as AxiosRequestConfig & { _retry?: boolean };
    if (error.response?.status === 401 && original && !original._retry) {
      original._retry = true;
      refreshing ??= refreshAccessToken().finally(() => (refreshing = null));
      const newToken = await refreshing;
      if (newToken) return instance(original);
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
  get: <T>(url: string, params?: object) => request<T>({ method: 'get', url, params }),
  post: <T>(url: string, data?: unknown) => request<T>({ method: 'post', url, data }),
  put: <T>(url: string, data?: unknown) => request<T>({ method: 'put', url, data }),
  delete: <T>(url: string) => request<T>({ method: 'delete', url }),
};
```

So với mẫu:

| Mẫu | Eco |
|---|---|
| `BehaviorSubject` + `isRefreshingToken` để các request chờ nhau | Một biến `refreshing` dùng chung Promise |
| `store.subscribe` để lấy token | `store.getState()` ngay lúc gửi request |
| Trả về `Observable`, component phải `.subscribe(...)` | Trả về `Promise`, component dùng `await` |

---

### Bước 5 — `shared/services/`

**`BaseService.ts`** (giống mẫu, có kiểu)

```ts
import { http } from 'shared/utils/http';
import type { PagedRequest, PagedResult } from 'shared/types/api';

export class BaseService<T, TKey = string> {
  protected http = http;
  constructor(protected url: string) {}

  getMany(req: PagedRequest) {
    return http.get<PagedResult<T>>(this.url, {
      ...req,
      sort: JSON.stringify(req.sort ?? {}),
      filter: JSON.stringify(req.filter ?? {}),
    });
  }
  getById(id: TKey) { return http.get<T>(`${this.url}/${id}`); }
  create(data: Partial<T>) { return http.post<T>(this.url, data); }
  update(id: TKey, data: Partial<T>) { return http.put<T>(`${this.url}/${id}`, data); }
  del(id: TKey) { return http.delete<boolean>(`${this.url}/${id}`); }
}
```

**`AuthService.ts`**

```ts
import { http } from 'shared/utils/http';
import { store } from 'store/store';
import { oauthActions, type AuthTokens } from 'store/oauth.slice';

export type LoginRequest = { usernameOrEmail: string; password: string };
export type RegisterRequest = { username: string; email: string; password: string; fullName: string; phoneNumber: string };

export const authService = {
  async login(data: LoginRequest) {
    const tokens = await http.post<AuthTokens>('/api/auth/login', data);
    store.dispatch(oauthActions.setToken(tokens));
    return tokens;
  },
  register: (data: RegisterRequest) => http.post<{ message: string }>('/api/auth/register', data),
  async logout() {
    const refreshToken = store.getState().oauth.refreshToken;
    try {
      if (refreshToken) await http.post('/api/auth/revoke-token', { refreshToken });
    } finally {
      store.dispatch(oauthActions.removeToken());
    }
  },
};
```

> ⚠️ `revoke-token` gửi `{ refreshToken }` là theo **contract mới** (BE Pha 2). Nếu BE chưa sửa thì tạm gửi chuỗi trần.
> Bỏ `deviceInfo`, `ipAddress` khỏi `LoginRequest` sau khi BE tự lấy từ request.

**`CaslService.ts`** (giống mẫu)

```ts
import { createContext } from 'react';
import { createMongoAbility, AbilityBuilder, type MongoAbility } from '@casl/ability';
import { createContextualCan } from '@casl/react';
import { store } from 'store/store';

export const ability = createMongoAbility();

const update = () => {
  const { can, rules } = new AbilityBuilder<MongoAbility>(createMongoAbility);
  store.getState().oauth.permissions.forEach((p) => can(p, 'all'));
  ability.update(rules);
};
update();
store.subscribe(update);

export const AbilityContext = createContext(ability);
export const Can = createContextualCan(AbilityContext.Consumer);
```

Dùng: `<Can I="admin.nguoi-dung.them_moi" a="all"><Button>Thêm</Button></Can>` hoặc `ability.can('admin.nguoi-dung.xoa', 'all')`.

---

### Bước 6 — Router + Layout

**`AppRoutes.tsx`** — mỗi route chỉ khai báo **một lần**:

```tsx
import { createBrowserRouter } from 'react-router-dom';
import { NotFoundPage } from 'shared/components';

export const router = createBrowserRouter([
  // Trang công khai
  {
    lazy: () => import('layouts/AuthLayout'),
    children: [
      { path: '/login', lazy: () => import('modules/auth/Login') },
      { path: '/register', lazy: () => import('modules/auth/Register') },
    ],
  },
  { path: '/logout', lazy: () => import('modules/logout') },

  // Trang cần đăng nhập
  {
    path: '/',
    lazy: () => import('layouts/MainLayout'),
    children: [
      { index: true, lazy: () => import('modules/home') },
      { path: 'nguoi-dung/*', lazy: () => import('modules/admin.nguoi-dung') },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
]);
```

> 💡 Muốn gọn hơn: tạo mảng `menuRoutes = [{ path, module, title, permission }]` rồi `map` ra route **và** menu Sider — 1 chỗ khai báo, không bao giờ trùng.

**`layouts/MainLayout.tsx`**

```tsx
import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { Layout } from 'antd';
import { useAppSelector } from 'store/hooks';
import { Header } from './headers/Header';
import { Sider } from './siderbars/Sider';

export function Component() {
  const accessToken = useAppSelector((s) => s.oauth.accessToken);
  const location = useLocation();

  // Giống AuthorizedLayout của mẫu, nhưng về /login của Eco thay vì SSO
  if (!accessToken) {
    return <Navigate to={`/login?returnUrl=${encodeURIComponent(location.pathname)}`} replace />;
  }

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Header />
      <Layout>
        <Sider />
        <Layout.Content style={{ padding: 16 }}>
          <Outlet />
        </Layout.Content>
      </Layout>
    </Layout>
  );
}
```

**`App.tsx`**

```tsx
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { RouterProvider } from 'react-router-dom';
import { App as AntApp, ConfigProvider } from 'antd';
import viVN from 'antd/locale/vi_VN';
import { store, persistor } from 'store/store';
import { AbilityContext, ability } from 'shared/services';
import { router } from './AppRoutes';

export default function App() {
  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <ConfigProvider locale={viVN} theme={{ token: { colorPrimary: '#1D50E7' } }}>
          <AntApp>
            <AbilityContext.Provider value={ability}>
              <RouterProvider router={router} />
            </AbilityContext.Provider>
          </AntApp>
        </ConfigProvider>
      </PersistGate>
    </Provider>
  );
}
```

> `<AntApp>` cho phép dùng `const { message, modal } = App.useApp()` ở mọi nơi — thay cho `BehaviorsService` của mẫu.

---

### Bước 7 — Tách `modules/auth`

- `Login.tsx`: antd `Form` (username/email + mật khẩu) → `authService.login` → `navigate(returnUrl ?? '/')`.
- `Register.tsx`: antd `Form` 5 ô, rule giống BE (username ≥ 3, mật khẩu ≥ 6, email hợp lệ) → thành công thì chuyển `/login`.
- Hiển thị lỗi theo `errorCode`:

```ts
export const authErrorText: Record<string, string> = {
  InvalidCredentials: 'Sai tên đăng nhập hoặc mật khẩu.',
  AccountLocked: 'Tài khoản đang bị khóa.',
  DuplicateAccount: 'Tên đăng nhập hoặc email đã tồn tại.',
  VALIDATION_ERROR: 'Dữ liệu chưa hợp lệ.',
};
```

- Mỗi file page export `Component` để router `lazy` dùng được:

```tsx
export function Component() { /* ... */ }
```

---

### Bước 8 — Lint, format, test

- `eslint.config.js`: `@eslint/js` + `typescript-eslint` + `eslint-plugin-react-hooks`.
- `.prettierrc`: `{ "singleQuote": true, "semi": true, "printWidth": 100 }`.
- Test đầu tiên: `oauth.slice.test.ts` — `setToken` giải mã đúng permissions; `removeToken` trả về rỗng.

---

## 5. Mẫu một module CRUD

Copy thư mục này mỗi khi tạo màn hình mới (giống mẫu, có TS).

```text
modules/admin.nguoi-dung/
├─ Const.ts      ← tên, quyền, endpoint, dữ liệu mặc định
├─ Service.ts    ← extends BaseService
├─ List.tsx      ← bảng + tìm kiếm + phân trang
├─ Form.tsx      ← xem / thêm / sửa theo /:id
├─ Module.tsx    ← route con: '/' → List, '/:id' → Form
└─ index.ts      ← export { Component }
```

**`Const.ts`**

```ts
export const ModuleName = 'Người dùng';
const Prefix = 'admin.nguoi-dung';
export const Permission = {
  VIEW: `${Prefix}.xem`,
  ADD: `${Prefix}.them_moi`,
  EDIT: `${Prefix}.chinh_sua`,
  DELETE: `${Prefix}.xoa`,
};
export const endPoint = '/api/admin/nguoi-dung';

export type NguoiDung = { id: string; username: string; email: string; isLocked: boolean };
export const defaultData: Partial<NguoiDung> = { username: '', email: '' };
```

**`Service.ts`**

```ts
import { BaseService } from 'shared/services';
import { endPoint, type NguoiDung } from './Const';

class NguoiDungService extends BaseService<NguoiDung> {
  lock(id: string) { return this.http.put(`${this.url}/${id}/lock`); }
}
export const service = new NguoiDungService(endPoint);
```

> Khác mẫu: không cần `createContext` cho service — import thẳng `service` là đủ.

**`Module.tsx`**

```tsx
import { Route, Routes } from 'react-router-dom';
import { NotFoundPage } from 'shared/components';
import { List } from './List';
import { Form } from './Form';

export function Component() {
  return (
    <Routes>
      <Route path="/" element={<List />}>
        <Route path=":id" element={<Form />} />
      </Route>
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}
```

**`List.tsx`** (rút gọn)

```tsx
import { useEffect, useState } from 'react';
import { Link, Outlet } from 'react-router-dom';
import { Table, Input, Button } from 'antd';
import { Can } from 'shared/services';
import { service } from './Service';
import { ModuleName, Permission, type NguoiDung } from './Const';

export function List() {
  const [data, setData] = useState<NguoiDung[]>([]);
  const [total, setTotal] = useState(0);
  const [query, setQuery] = useState({ page: 1, pageSize: 20, search: '' });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    service
      .getMany(query)
      .then((res) => { setData(res.data); setTotal(res.meta.total); })
      .finally(() => setLoading(false));
  }, [query]);

  return (
    <>
      <h2>{ModuleName}</h2>
      <Input.Search onSearch={(search) => setQuery({ ...query, page: 1, search })} />
      <Can I={Permission.ADD} a="all"><Link to="new"><Button type="primary">Thêm</Button></Link></Can>
      <Table
        rowKey="id"
        loading={loading}
        dataSource={data}
        columns={[
          { title: 'Tên đăng nhập', dataIndex: 'username', render: (v, r) => <Link to={r.id}>{v}</Link> },
          { title: 'Email', dataIndex: 'email' },
        ]}
        pagination={{
          current: query.page, pageSize: query.pageSize, total,
          onChange: (page, pageSize) => setQuery({ ...query, page, pageSize }),
        }}
      />
      <Outlet />
    </>
  );
}
```

> 🔜 Giai đoạn sau: thay `useEffect + useState` bằng **TanStack Query** (`useQuery`) — tự cache, tự loading, tự gọi lại khi lưu xong (thay cho `sendToView` Subject của mẫu).

---

## 6. Quy ước code

| Loại | Quy ước | Ví dụ |
|---|---|---|
| Thư mục module | `<nhóm>.<tên-kebab>` | `admin.nguoi-dung`, `dm.tinh-thanh` |
| Component / page | PascalCase `.tsx` | `List.tsx`, `Header.tsx` |
| Service, util | camelCase hoặc PascalCase `.ts` | `httpClient.ts`, `BaseService.ts` |
| Mã quyền | `<module>.<hành-động>` | `admin.nguoi-dung.xem` |
| Tên trường JSON | camelCase (theo BE Eco) | `accessToken`, **không** `access_token` |
| Import | thư viện → alias (`shared/…`) → file cùng module (`./…`) | |
| Export cho router | `export function Component()` | |

**Không làm**
- Không gọi `axios` / `fetch` trực tiếp trong component → luôn qua `service`.
- Không để `console.log` khi commit.
- Không để code bị comment bỏ dở → xóa đi, git đã lưu lịch sử.
- Không ghi secret (`client_secret`, mật khẩu, key) trong code FE — ai mở DevTools cũng đọc được.
- Không dùng `any` khi biết được kiểu.

---

## 7. Checklist tổng

**Nền tảng**
- [x] Bước 1 — Cài thư viện, script `lint / format / test`, sửa `.env`
- [x] Bước 2 — `shared/types/api.ts`
- [x] Bước 3 — `store/` (Redux Toolkit + persist)
- [x] Bước 4 — `shared/utils/http/` (axios + refresh)
- [x] Bước 5 — `BaseService`, `AuthService`, `CaslService`
- [x] Bước 6 — `AppRoutes`, `MainLayout`, `AuthLayout`, `App.tsx`
- [x] Bước 7 — Tách `Login` / `Register`, thêm `Logout`
- [x] Bước 8 — ESLint, Prettier, test đầu tiên

**Dọn dẹp**
- [x] Xóa `shared/utils/http.ts`, `fe.txt`, `styles.css` cũ không dùng
- [x] Cập nhật `ECO_FE_GUIDE.md` theo cấu trúc mới

**Kiểm tra cuối**
- [x] Đăng ký → đăng nhập → vào `/`
- [x] F5 vẫn còn đăng nhập
- [x] Vào trang cần đăng nhập khi chưa đăng nhập → bị đưa về `/login`, đăng nhập xong quay lại đúng trang
- [ ] Access token hết hạn → tự refresh, người dùng không thấy gì *(chờ BE có API `[Authorize]` để thử)*
- [x] Đăng xuất → token bị thu hồi trên BE
- [x] `npm run build` và `npm run lint` không lỗi

**Chờ BE Pha 2** (BE sửa xong thì FE sửa theo)
- [ ] BE lấy IP + User-Agent từ `HttpContext` → `AuthService.login` bỏ `deviceInfo`, `ipAddress`
- [ ] BE đổi body `revoke-token` thành `{ refreshToken }` → `AuthService.logout` gửi object thay vì chuỗi trần
- [ ] BE có `PagedResult<T>` → làm module `admin.nguoi-dung` theo mẫu mục 5
