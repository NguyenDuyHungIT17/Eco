# Roadmap Identity — Eco

> Cập nhật: 2026-11-02 · Làm xong việc nào thì đổi `[ ]` → `[x]`.
> Xem nhanh BE + FE đang ở đâu → [CHECKLIST.md](CHECKLIST.md).

---

## 📍 Đang ở đâu?

| Pha | Nội dung | Trạng thái |
|:---:|---|:---:|
| 1 | Đăng ký, đăng nhập, refresh / revoke token | ✅ Xong |
| **2** | **Sửa lỗi BE + dựng khung FE theo cấu trúc Frontends** | 👉 **Đang làm** |
| 3 | Xác thực email, quên mật khẩu, OTP | ⏳ |
| 4 | Phân quyền Role / Permission | ⏳ |
| 5 | Quản lý phiên đăng nhập, Audit log | ⏳ |
| 6 | Đăng nhập Google, gói thành viên | ⏳ |

Mỗi pha: **BE làm API → test bằng Swagger → FE ghép giao diện.**

---

## 👉 5 việc làm ngay

1. **BE** – Đưa mật khẩu DB và JWT secret ra khỏi code, thêm CORS.
2. **BE** – Lấy IP / thiết bị ở server, khóa tài khoản 30 phút, sửa `revoke-token`.
3. **FE** – Cài thư viện, dựng `store` (Redux) + `http` client (axios) giống Frontends.
4. **FE** – Router `createBrowserRouter` + `AuthorizedLayout` + trang `/login`, `/register`.
5. **BE + FE** – Chạy thử trọn luồng: đăng ký → đăng nhập → F5 → token tự refresh → đăng xuất.

---

## Pha 2 — Sửa lỗi BE + Khung FE

### BE

**Bảo mật cấu hình**
- [ ] Đổi mật khẩu `sa` của SQL Server (mật khẩu cũ đã lộ trong lịch sử git)
- [ ] `EcoDbContextFactory` đọc connection string từ config, xóa dòng comment chứa mật khẩu
- [ ] JWT secret chuyển sang user-secrets
- [ ] Tạo class `JwtSettings` dùng Options pattern
- [ ] Thêm CORS cho `http://localhost:5173`

**Logic đăng nhập**
- [ ] Lấy IP + User-Agent từ `HttpContext`, bỏ 2 field này khỏi `LoginRequestDto`
- [ ] Chặn user đã xóa (`IsDeleted`)
- [ ] Khóa 30 phút sau 5 lần sai (thêm cột `LockoutEnd`)
- [ ] `ExpiresInSeconds` đọc từ config, không hardcode 3600
- [ ] Gộp đoạn lấy roles / permissions bị lặp thành 1 hàm
- [ ] Refresh token: cập nhật `UserSession`, phát hiện token cũ bị dùng lại
- [ ] Lưu refresh token dạng hash
- [ ] `revoke-token` cần `[Authorize]`, body là `{ refreshToken }`

**Database & dọn code**
- [ ] Các trường không bắt buộc đổi sang nullable (`RevokedAt`, `UsedAt`, `LastLoginAt`, `Birthday`, …)
- [ ] Query filter `IsDeleted = false`
- [ ] Tạo migration mới
- [ ] Xóa `Class1.cs`, sửa tên `EmailVerificationConfiguration.cs.cs`, `UserPermission.cs`
- [ ] Thêm API phân trang dùng chung: `PagedResult<T> { data, meta: { page, pageSize, total } }` (để FE dùng `BaseService.getMany`)

**Test**
- [ ] Unit test `AuthenticationService`
- [ ] Integration test 4 API auth
- [ ] GitHub Actions: build + test BE, build FE

### FE (theo cấu trúc Frontends)

**Thư viện**
- [x] `react-router-dom` `@reduxjs/toolkit` `react-redux` `redux-persist` `axios` `antd` `dayjs` `jwt-decode` `@casl/ability` `@casl/react`

**`store/`**
- [x] `oauth.slice.ts`: `createSlice` gồm `accessToken`, `refreshToken`, `profile` (giải mã từ JWT), `permissions`
- [x] `reducers.ts` + `store.ts` (redux-persist chỉ lưu `oauth`)

**`shared/utils/http/`**
- [x] `httpClient.ts`: axios tự gắn `Bearer`, gặp 401 thì refresh **1 lần** rồi gọi lại, refresh lỗi thì logout
- [x] Tự bóc `ApiResponse` → trả về `data`, lỗi ném ra `{ status, errorCode, message, errors }`

**`shared/services/`**
- [x] `BaseService.ts`: `getMany / getById / create / update / del`
- [x] `AuthService.ts`: dùng `http` + `dispatch(actions.setToken)`
- [x] `CaslService.ts`: `ability` + `<Can>` đọc permissions từ store

**Router & layout**
- [x] `AppRoutes.tsx` dùng `createBrowserRouter` + `lazy`
- [x] `layouts/MainLayout.tsx` gồm `AuthorizedLayout` (chưa đăng nhập → `/login`), Header, Sider, `<Outlet/>`

**Modules**
- [x] `modules/auth`: tách `Login.tsx`, `Register.tsx`; `index.ts` export `Component`
- [x] `modules/logout`, `modules/home`
- [x] Xóa `VITE_API_BASE` trong `.env`, tạo `.env.example`
- [x] Cập nhật `ECO_FE_GUIDE.md` theo cấu trúc mới

**✔ Xong khi:** không còn secret trong code · F5 không mất đăng nhập · token hết hạn tự refresh · CI xanh.

---

## Pha 3 — Bảo mật tài khoản

**API**

| Method | URL | Việc |
|---|---|---|
| GET | `/api/auth/verify-email?token=` | Kích hoạt tài khoản |
| POST | `/api/auth/resend-verification` | Gửi lại mail xác thực |
| POST | `/api/auth/forgot-password` | Gửi mail đặt lại mật khẩu |
| POST | `/api/auth/reset-password` | Đặt mật khẩu mới bằng token |
| POST | `/api/auth/change-password` | Đổi mật khẩu (đã đăng nhập) |
| POST | `/api/auth/2fa/verify` | Nhập OTP khi đăng nhập |

**BE**
- [ ] Đăng ký `EmailVerificationService` vào DI + thêm endpoint
- [ ] `IEmailSender` + `SmtpEmailSender` trong `Eco.Infrastructure` (dev dùng Mailpit)
- [ ] Gửi mail khi đăng ký
- [ ] `PasswordResetService`, `OtpService`
- [ ] Rate limit cho login / forgot / OTP
- [ ] Chốt: **chặn đăng nhập khi chưa xác thực email?** (khuyến nghị: có)

**FE**
- [ ] `modules/auth`: thêm `VerifyEmail.tsx`, `ForgotPassword.tsx`, `ResetPassword.tsx`, bước nhập OTP
- [ ] `modules/account`: đổi mật khẩu, bật / tắt 2FA

**✔ Xong khi:** đăng ký → nhận mail → bấm link → đăng nhập được.

---

## Pha 4 — Phân quyền

**Quy ước permission** (giống Frontends): `<module>.<hành động>`
→ ví dụ `admin.nguoi-dung.xem`, `admin.nguoi-dung.them_moi`, `admin.nguoi-dung.chinh_sua`, `admin.nguoi-dung.xoa`

**API**

| Method | URL | Quyền |
|---|---|---|
| GET | `/api/me` | đăng nhập |
| GET / PUT | `/api/admin/nguoi-dung` | `admin.nguoi-dung.*` |
| GET / POST / PUT / DELETE | `/api/admin/vai-tro` | `admin.vai-tro.*` |
| PUT | `/api/admin/vai-tro/{id}/permissions` | `admin.vai-tro.chinh_sua` |

**BE**
- [ ] `[HasPermission("...")]` + `PermissionAuthorizationHandler`
- [ ] Seed role `Admin`, `User`, toàn bộ permission, tài khoản admin
- [ ] `ICurrentUserService` → tự điền `CreatedBy` / `UpdatedBy`

**FE**
- [ ] Module `admin.nguoi-dung`, `admin.vai-tro` theo mẫu 6 file: `Const` · `Service` · `List` · `Form` · `Module` · `index`
- [ ] Nút bấm có `perm={Permission.EDIT}`, ẩn khi không có quyền (`<Can>`)
- [ ] Menu Sider lọc theo quyền

**✔ Xong khi:** user thường gọi API admin bị 403, menu admin bị ẩn.

---

## Pha 5 — Phiên đăng nhập & Audit

| Method | URL | Việc |
|---|---|---|
| GET | `/api/sessions` | Danh sách thiết bị đang đăng nhập |
| DELETE | `/api/sessions/{id}` | Đăng xuất 1 thiết bị |
| DELETE | `/api/sessions` | Đăng xuất tất cả thiết bị khác |
| GET | `/api/me/login-history` | Lịch sử đăng nhập |
| GET | `/api/admin/audit-logs` | Nhật ký thao tác |

- **BE:** claim `sid` trong JWT · đọc User-Agent ra Browser / OS · ghi `AuditLog` · job dọn token hết hạn
- **FE:** module `account.thiet-bi`, `account.lich-su-dang-nhap`, `admin.audit-log`

---

## Pha 6 — Google Login & Gói thành viên

| Method | URL | Việc |
|---|---|---|
| POST | `/api/auth/external-login` | Đăng nhập bằng Google `idToken` |
| GET | `/api/me/subscription` | Gói hiện tại + AI credits |
| GET | `/api/plans` | Danh sách gói |

- **BE:** xác thực Google idToken · tạo gói Free khi đăng ký · `[RequiresPlan]` · job hết hạn gói
- **FE:** nút "Đăng nhập Google" · trang `/pricing` · hiện gói trên Header

---

## Phụ lục A — Format response

```json
{ "success": true, "message": "...", "data": {}, "errors": null, "errorCode": null }
```

| Trường hợp | HTTP |
|---|:---:|
| Sai dữ liệu nhập | 400 |
| Sai mật khẩu / token hỏng | 401 |
| Bị khóa / không có quyền | 403 |
| Trùng dữ liệu | 409 |
| Lỗi server | 500 |

## Phụ lục B — Service cần có (Eco.Application)

| Interface | Pha | Trạng thái |
|---|:---:|:---:|
| `IPasswordHasher`, `ITokenService`, `IAuthenticationService` | 1 | ✅ |
| `IEmailVerificationService` | 3 | ⚠️ có code, chưa đăng ký DI |
| `IEmailSender`, `IPasswordResetService`, `IOtpService` | 3 | ⏳ |
| `ICurrentUserService` | 4 | ⏳ |
| `ISessionService`, `IAuditService` | 5 | ⏳ |
| `IExternalAuthService`, `ISubscriptionService` | 6 | ⏳ |
