# Hướng dẫn Eco_FE

> Đọc file này khi cần **chạy project, tìm file, thêm trang mới hoặc debug**.
> Muốn học React từ đầu → [FE_LEARNING_ROADMAP.md](FE_LEARNING_ROADMAP.md).
> Muốn biết vì sao cấu trúc như thế này → [FE_REFACTOR_PLAN.md](FE_REFACTOR_PLAN.md).

## Mục lục

1. [Chạy project](#1-chạy-project)
2. [Thư viện đang dùng](#2-thư-viện-đang-dùng)
3. [Cấu trúc thư mục](#3-cấu-trúc-thư-mục)
4. [Một request đi như thế nào](#4-một-request-đi-như-thế-nào)
5. [Đăng nhập & phân quyền](#5-đăng-nhập--phân-quyền)
6. [Thêm một trang / API mới](#6-thêm-một-trang--api-mới)
7. [Quy ước code](#7-quy-ước-code)
8. [Debug](#8-debug)
9. [Còn tạm / chờ BE](#9-còn-tạm--chờ-be)

---

## 1. Chạy project

**Cần có:** Node.js 20+, BE `Eco.Api` chạy profile `https` (cổng `7038`).

```bash
cd Eco_FE
npm install          # lần đầu
npm run dev          # mở http://localhost:5173
```

| Lệnh | Làm gì |
|---|---|
| `npm run dev` | Chạy dev, sửa code là trình duyệt tự cập nhật |
| `npm run build` | Kiểm tra kiểu (`tsc`) rồi build ra `dist/` |
| `npm run preview` | Xem thử bản build |
| `npm run lint` | Bắt lỗi code (ESLint) |
| `npm run format` | Tự format code (Prettier) |
| `npm test` | Chạy test (Vitest) |

**Biến môi trường** (`.env`, copy từ `.env.example`):

| Biến | Giá trị | Ghi chú |
|---|---|---|
| `VITE_API_BASE` | để trống | Dev đi qua proxy của Vite. Chỉ điền khi build cho máy chủ thật / mobile |

> **Proxy:** [vite.config.ts](vite.config.ts) chuyển mọi request `/api/...` sang `https://localhost:7038`.
> Nhờ vậy FE và BE coi như "cùng địa chỉ" → không bị lỗi CORS khi dev.

---

## 2. Thư viện đang dùng

| Thư viện | Vai trò | Tương đương bên .NET |
|---|---|---|
| `react` | Vẽ giao diện bằng component | Razor View |
| `react-router-dom` | Đổi trang theo URL | `[Route]` / routing |
| `@reduxjs/toolkit` + `react-redux` | Dữ liệu dùng chung toàn app (token, profile) | Singleton service |
| `redux-persist` | Lưu store vào `localStorage` → F5 không mất | Cookie / session |
| `axios` | Gọi API | `HttpClient` |
| `antd` + `@ant-design/icons` | Bộ component có sẵn: Form, Table, Button, Modal… | Bộ control UI |
| `@casl/ability` + `@casl/react` | Kiểm tra quyền, ẩn/hiện nút | `[Authorize(Policy)]` (phía giao diện) |
| `jwt-decode` | Đọc nội dung access token | — |
| `dayjs` | Xử lý ngày giờ | `DateTime` |

---

## 3. Cấu trúc thư mục

```
src/
├── main.tsx            Điểm khởi động: render <App/>, nạp CSS
├── App.tsx             Bọc các "provider": Redux, persist, antd (tiếng Việt), quyền, router
├── AppRoutes.tsx       Danh sách URL → trang nào
├── app-setting.ts      Đọc biến môi trường (API_BASE_URL)
│
├── layouts/            Khung trang
│   ├── MainLayout.tsx    Header + Sider + nội dung. Chưa đăng nhập → đá về /login
│   ├── AuthLayout.tsx    Khung trang login/register. Đã đăng nhập → về /
│   ├── menu.tsx          Khai báo menu bên trái (1 chỗ duy nhất)
│   ├── headers/Header.tsx   Logo + tên người dùng + Đăng xuất
│   └── siderbars/Sider.tsx  Menu, tự lọc theo quyền
│
├── modules/            Mỗi nghiệp vụ một thư mục
│   ├── auth/             Login.tsx, Register.tsx, Const.ts (câu báo lỗi tiếng Việt)
│   ├── logout/           Gọi revoke-token rồi về /login
│   └── home/             Dashboard.tsx
│
├── shared/             Dùng chung cho mọi module
│   ├── types/api.ts      ApiResponse<T>, PagedResult<T>, ApiError — khớp với BE
│   ├── utils/http/       httpClient.ts: axios + gắn token + tự refresh
│   ├── services/         BaseService (CRUD chung), AuthService, CaslService (quyền)
│   └── components/       PermButton, NotFoundPage, FallbackElement
│
├── store/              Redux
│   ├── oauth.slice.ts    Token + profile + roles + permissions
│   ├── store.ts          Tạo store, cấu hình persist
│   ├── reducers.ts       Gộp các slice
│   └── hooks.ts          useAppSelector / useAppDispatch (đã có kiểu)
│
└── styles/global.css   CSS toàn app
```

**Import bằng alias**, không dùng `../../`:

```ts
import { http } from 'shared/utils/http';
import { useAppSelector } from 'store/hooks';
```

Alias khai báo ở **2 nơi** và phải giống nhau: [tsconfig.json](tsconfig.json) (`paths`) và [vite.config.ts](vite.config.ts) (`resolve.alias`).

---

## 4. Một request đi như thế nào

Ví dụ: bấm **Đăng nhập**.

```
Login.tsx            người dùng bấm nút → gọi authService.login(values)
   ↓
AuthService.ts       http.post('/api/auth/login', data) → nhận token → dispatch(setToken)
   ↓
httpClient.ts        gắn header Bearer, gửi request
   ↓  Vite proxy
BE  POST /api/auth/login  → trả ApiResponse { success, data, errorCode, ... }
   ↓
httpClient.ts        thành công: trả về phần `data`
                     thất bại:   ném ApiError(status, message, errorCode, errors)
   ↓
Login.tsx            thành công: chuyển trang · thất bại: getAuthErrorMessage(err) → hiện Alert
```

**3 việc `httpClient` làm sẵn — module không phải tự làm:**

1. Gắn `Authorization: Bearer <accessToken>` vào mọi request.
2. Gặp **401** → gọi `refresh-token` **đúng 1 lần** (nhiều request cùng lỗi thì chờ chung), rồi gửi lại request cũ. Refresh hỏng → đăng xuất.
   Các URL `/api/auth/*` được bỏ qua (vì đăng nhập sai cũng trả 401).
3. Bóc `ApiResponse` → component nhận thẳng dữ liệu.

**Khớp với BE ở 5 điểm:**

| # | Điểm nối | BE | FE |
|:-:|---|---|---|
| ① | Địa chỉ | `[Route]` + `[HttpPost("login")]` | URL trong Service |
| ② | Dữ liệu gửi | `LoginRequestDto` (PascalCase) | `interface LoginRequest` (camelCase) |
| ③ | Dữ liệu nhận | `ApiResponse<AuthResponseDto>` | `ApiResponse<AuthTokens>` trong `shared/types` |
| ④ | Lỗi | HTTP status + `errorCode` | `ApiError.errorCode` → câu tiếng Việt trong `Const.ts` |
| ⑤ | Quyền | Claim `role`, `permission` trong JWT + `[Authorize]` | `oauth.slice` giải mã → CASL ẩn/hiện |

> Thêm field vào DTO bên BE thì **phải tự thêm** vào interface bên FE — không có gì tự đồng bộ.

---

## 5. Đăng nhập & phân quyền

**Lưu ở đâu?** `store.oauth` (Redux), được `redux-persist` ghi vào `localStorage` key `persist:eco`.

| Trường | Lấy từ |
|---|---|
| `accessToken`, `refreshToken` | Response login / refresh |
| `profile` `{ id, username, email }` | Giải mã JWT: `sub`, `unique_name`, `email` |
| `roles` | Claim `role` |
| `permissions` | Claim `permission` |

**Đọc trong component:**

```tsx
const { profile, permissions } = useAppSelector((s) => s.oauth);
```

**Bảo vệ trang:** mọi route con của `MainLayout` đều cần đăng nhập. Chưa đăng nhập → `/login?returnUrl=<trang đang mở>`, đăng nhập xong quay lại đúng trang đó.

**Ẩn/hiện theo quyền** — mã quyền dạng `<module>.<hành-động>`, ví dụ `admin.nguoi-dung.xem`:

```tsx
// Nút tự ẩn khi không có quyền
<PermButton perm="admin.nguoi-dung.them" type="primary">Thêm</PermButton>

// Ẩn một khối bất kỳ
<Can I="admin.nguoi-dung.xoa" a="all">
  <Button danger>Xóa</Button>
</Can>

// Menu: khai báo permission là Sider tự lọc
{ path: '/admin/nguoi-dung', title: 'Người dùng', permission: 'admin.nguoi-dung.xem' }
```

> ⚠️ Ẩn ở FE chỉ để giao diện gọn, **không phải bảo mật**. BE luôn phải kiểm tra quyền lại.

---

## 6. Thêm một trang / API mới

Ví dụ: trang **Người dùng** gọi `GET /api/admin/nguoi-dung`.

**Bước 0 — BE trước.** Làm API, mở Swagger gọi thử, **chép lại JSON trả về**.

**Bước 1 — Tạo thư mục module** theo mẫu 6 file:

```
modules/admin/nguoi-dung/
├── Const.ts      Kiểu dữ liệu + mã quyền
├── Service.ts    Gọi API
├── List.tsx      Bảng danh sách
├── Form.tsx      Form thêm / sửa (Modal)
├── Module.tsx    Ghép List + Form
└── index.ts      export { Component }
```

**Bước 2 — `Const.ts`:** viết interface theo đúng JSON đã chép.

```ts
export interface User { id: string; username: string; email: string; isActive: boolean; }

export const Permission = {
  VIEW: 'admin.nguoi-dung.xem',
  CREATE: 'admin.nguoi-dung.them',
  EDIT: 'admin.nguoi-dung.sua',
  DELETE: 'admin.nguoi-dung.xoa',
};
```

**Bước 3 — `Service.ts`:** CRUD có sẵn nhờ `BaseService`, chỉ viết thêm API đặc biệt.

```ts
import { BaseService } from 'shared/services/BaseService';
import type { User } from './Const';

class UserService extends BaseService<User> {
  constructor() { super('/api/admin/nguoi-dung'); }

  lock(id: string) {
    return this.http.post<boolean>(`${this.url}/${id}/lock`);
  }
}
export const userService = new UserService();
```

**Bước 4 — Trang:** gọi service, đổ vào component antd. Mẫu đầy đủ List/Form xem [FE_REFACTOR_PLAN.md mục 5](FE_REFACTOR_PLAN.md#5-mẫu-một-module-crud).

**Bước 5 — Đăng ký route** trong [AppRoutes.tsx](src/AppRoutes.tsx), bên trong `children` của `MainLayout`:

```tsx
{ path: 'admin/nguoi-dung', lazy: () => import('modules/admin/nguoi-dung') },
```

**Bước 6 — Thêm menu** trong [layouts/menu.tsx](src/layouts/menu.tsx) (kèm `permission`).

**Bước 7 — Lỗi riêng:** nếu BE có `errorCode` mới, thêm câu tiếng Việt tương ứng.

**Bước 8 — Kiểm tra:** `npm run lint` · `npm run build` · mở trình duyệt thử.

---

## 7. Quy ước code

| Loại | Quy ước | Ví dụ |
|---|---|---|
| Component, file component | PascalCase | `Login.tsx`, `PermButton` |
| Biến, hàm, field JSON | camelCase | `accessToken`, `getMany` |
| Hằng số | UPPER_SNAKE | `API_BASE_URL` |
| Thư mục module | kebab-case, theo nghiệp vụ | `admin/nguoi-dung` |
| Mã quyền | `<module>.<hành-động>` | `admin.vai-tro.sua` |

- **Trang** export `function Component()` — router `lazy` cần đúng tên này.
- **Gọi API** chỉ qua Service, không gọi `axios` trực tiếp trong component.
- **Đọc store** bằng `useAppSelector`, không dùng `useSelector` trơn (mất kiểu).
- **Thông báo** dùng `App.useApp().message` / `modal` của antd, không dùng `alert()`.
- **Format:** để Prettier lo (nháy đơn, có `;`, tối đa 100 ký tự/dòng). Chạy `npm run format`.
- **Comment:** viết tiếng Việt, chỉ giải thích "vì sao", không mô tả lại code.
- **Trước khi commit:** `npm run lint && npm run build && npm test` phải qua hết.

---

## 8. Debug

Mở **F12 → tab Network**, bấm vào request lỗi, xem *Headers / Payload / Response*:

| Thấy gì | Nguyên nhân hay gặp | Sửa ở |
|---|---|---|
| Không có request nào | Nút không gọi hàm, form không submit, lỗi JS trước đó | Tab **Console**, component |
| `404` | Sai URL hoặc method | Service ↔ `[Route]` bên BE |
| `400` + `VALIDATION_ERROR` | Thiếu field / sai tên field (xem `errors`) | Payload ↔ DTO + Validator |
| `401` ở API thường | Thiếu token, refresh token hết hạn | Đăng nhập lại; xem tab Application → Local Storage → `persist:eco` |
| `401` ở login | Sai tài khoản/mật khẩu (bình thường) | — |
| `403` | Không có quyền / tài khoản bị khóa | Quyền của user bên BE |
| `500` / `ECONNREFUSED` trong terminal Vite | BE chưa chạy hoặc lỗi server | Chạy BE profile `https`; xem log BE |
| Lỗi CORS | Gọi thẳng BE thay vì qua proxy | Để trống `VITE_API_BASE` khi dev |
| Dữ liệu trả về đúng nhưng màn hình trống | Đọc sai tên field (`UserName` vs `userName`) | Interface trong `Const.ts` |

Công cụ nên cài trên trình duyệt: **React Developer Tools**, **Redux DevTools** (xem store thay đổi theo từng action).

---

## 9. Còn tạm / chờ BE

| Chỗ | Hiện tại | Khi BE Pha 2 xong |
|---|---|---|
| `AuthService.login` | Gửi thêm `deviceInfo`, `ipAddress` (BE đang bắt buộc) | Bỏ 2 field, BE tự lấy từ `HttpContext` |
| `AuthService.logout` | Gửi refresh token dạng chuỗi trần | Gửi `{ refreshToken }` |
| Tự refresh khi 401 | Code có, chưa thử thực tế được (BE chưa có API nào `[Authorize]`) | Thử khi có API cần đăng nhập |
| Module `admin.nguoi-dung` | Chưa có | Làm khi BE có `PagedResult<T>` + API người dùng |
| Đăng nhập khi chưa xác thực email | BE vẫn cho vào | Pha 3 |
