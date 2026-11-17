# Checklist tổng — Eco (BE + FE)

> Cập nhật: 2026-11-17 · Trạng thái được kiểm tra trực tiếp trong code, không phải ước lượng.
> Chi tiết từng việc (API, lý do, cách làm) → [identity_roadmap.md](identity_roadmap.md) · FE → [FE_REFACTOR_PLAN.md](../Eco_FE/FE_REFACTOR_PLAN.md)

**Ký hiệu:** ✅ xong · 🟡 làm dở · ⬜ chưa làm · 👤 việc bạn phải tự làm

---

## 📍 Tổng quan

| Pha | Nội dung | BE | FE |
|:---:|---|:---:|:---:|
| 1 | Đăng ký, đăng nhập, refresh / revoke token | ✅ | ✅ |
| **2** | **Sửa lỗi BE + khung FE** | ⬜ **0 / 23** | ✅ **9 / 10** |
| 3 | Xác thực email, quên mật khẩu, OTP | ⬜ | ⬜ |
| 4 | Phân quyền Role / Permission | ⬜ | 🟡 có sẵn khung |
| 5 | Phiên đăng nhập, Audit log | ⬜ | ⬜ |
| 6 | Đăng nhập Google, gói thành viên | ⬜ | ⬜ |
| — | Bản mobile (APK Android) | — | ⬜ |

**👉 Đang ở:** FE xong Pha 2 → **BE Pha 2 là việc tiếp theo** (FE đang chờ 3 việc của BE).

---

## Pha 1 — Đăng nhập cơ bản ✅

| BE | FE |
|---|---|
| ✅ `POST /api/auth/register` | ✅ Trang `/register` |
| ✅ `POST /api/auth/login` | ✅ Trang `/login`, báo lỗi tiếng Việt |
| ✅ `POST /api/auth/refresh-token` (token cũ bị vô hiệu) | ✅ `httpClient` tự refresh khi 401 |
| ✅ `POST /api/auth/revoke-token` | ✅ `/logout` gọi revoke rồi xóa token |
| ✅ Mật khẩu BCrypt, JWT, FluentValidation | ✅ Lưu đăng nhập qua F5 (redux-persist) |
| ✅ `ApiResponse<T>` + middleware bắt lỗi | ✅ `ApiError` + bóc `data` tự động |
| ✅ Ghi `LoginHistory` | — |

---

## Pha 2 — Sửa lỗi BE + khung FE 👉

### BE

**🔒 Bảo mật & cấu hình**
- [ ] 👤 Đổi mật khẩu `sa` của SQL Server (mật khẩu cũ đã lộ trong lịch sử git)
- [ ] `EcoDbContextFactory`: đọc connection string từ config, xóa dòng comment chứa mật khẩu
- [ ] 👤 JWT `SecretKey` chuyển khỏi `appsettings.Development.json` sang user-secrets
- [ ] Class `JwtSettings` + Options pattern *(hiện đọc thẳng `Configuration`)*
- [ ] CORS cho `http://localhost:5173` *(hiện chưa có)*

**🔑 Luồng auth**
- [ ] Lấy IP + User-Agent từ `HttpContext`, bỏ khỏi `LoginRequestDto` → *FE đang chờ*
- [ ] `revoke-token`: `[Authorize]` + body `{ refreshToken }` → *FE đang chờ*
- [ ] Chặn user đã xóa (`IsDeleted`) khi login / refresh
- [ ] 🟡 Khóa tài khoản: *hiện sai 5 lần là khóa **vĩnh viễn*** → đổi thành khóa 30 phút (cột `LockoutEnd`)
- [ ] `ExpiresInSeconds` đọc từ config *(đang hardcode `3600` ở `AuthMapping.cs`)*
- [ ] Gộp đoạn lấy roles / permissions *(đang lặp y hệt 2 lần trong `AuthenticationService`)*
- [ ] Refresh token: cập nhật `UserSession`, phát hiện token cũ bị dùng lại → thu hồi cả chuỗi
- [ ] Lưu refresh token dạng hash *(đang lưu nguyên văn)*

**🗄️ Database & dọn code**
- [ ] Đổi sang nullable: `RefreshToken.RevokedAt`, `PasswordReset.UsedAt`, `User.LastLoginAt`, …
- [ ] Query filter `IsDeleted = false` *(chưa có `HasQueryFilter` nào)*
- [ ] Migration mới cho các thay đổi trên
- [ ] Xóa 5 file `Class1.cs` (Application, Domain, Identity, Infrastructure, Shared)
- [ ] Đổi tên `EmailVerificationConfiguration.cs.cs` → `.cs`

**🧩 Cho FE dùng**
- [ ] `PagedResult<T> { data, meta: { page, pageSize, total } }` → *FE đang chờ*
- [ ] Một API cần đăng nhập, ví dụ `GET /api/me` → để thử được tự refresh *(hiện BE chưa có API `[Authorize]` nào)*

**🧪 Test & CI**
- [ ] Unit test `AuthenticationService` *(project có, mới chỉ có `UnitTest1.cs` trống)*
- [ ] Integration test 4 API auth *(như trên)*
- [ ] GitHub Actions: build + test BE, build + lint FE *(chưa có `.github/workflows`)*

### FE

- [x] Thư viện: router, Redux Toolkit, persist, axios, antd, dayjs, jwt-decode, CASL
- [x] `store/`: `oauth.slice` (token, profile, roles, permissions), persist
- [x] `httpClient`: gắn Bearer, 401 → refresh 1 lần, bóc `ApiResponse`
- [x] `BaseService`, `AuthService`, `CaslService`
- [x] Router `lazy`, `MainLayout` (chặn chưa đăng nhập), `AuthLayout`, Header, Sider, menu theo quyền
- [x] Module `auth` (Login, Register), `logout`, `home`
- [x] ESLint, Prettier, Vitest (2 test), `.env.example`
- [x] `ECO_FE_GUIDE.md` viết lại theo cấu trúc mới
- [x] Thử trên Chrome: chặn trang · đăng ký · đăng nhập · F5 · đăng xuất · returnUrl (12/12 qua)
- [ ] Thử tự refresh khi token hết hạn ← *chờ BE có API `[Authorize]`*

**FE sửa theo khi BE xong:**
- [ ] `AuthService.login` bỏ `deviceInfo`, `ipAddress`
- [ ] `AuthService.logout` gửi `{ refreshToken }` thay vì chuỗi trần

**✔ Pha 2 xong khi:** không còn secret trong code · F5 không mất đăng nhập · token hết hạn tự refresh · CI xanh

---

## Pha 3 — Bảo mật tài khoản

| BE | FE |
|---|---|
| ⬜ 🟡 `EmailVerificationService` *(có code, **chưa đăng ký DI**, chưa có endpoint)* | ⬜ Trang `VerifyEmail` |
| ⬜ `GET verify-email`, `POST resend-verification` | ⬜ Nút "Gửi lại mail" |
| ⬜ `IEmailSender` + SMTP *(`Eco.Infrastructure` đang trống)*, dev dùng Mailpit | — |
| ⬜ Gửi mail khi đăng ký | — |
| ⬜ `POST forgot-password`, `POST reset-password` | ⬜ Trang `ForgotPassword`, `ResetPassword` |
| ⬜ `POST change-password` | ⬜ `modules/account`: đổi mật khẩu |
| ⬜ OTP / 2FA: `POST 2fa/verify` | ⬜ Bước nhập OTP khi login, bật/tắt 2FA |
| ⬜ Rate limit login / forgot / OTP | ⬜ Hiện thông báo khi bị giới hạn (429) |
| ⬜ 👤 Chốt: chặn đăng nhập khi chưa xác thực email? *(hiện vẫn cho vào)* | ⬜ Thêm câu lỗi tương ứng |

---

## Pha 4 — Phân quyền

| BE | FE |
|---|---|
| ⬜ `[HasPermission("...")]` + `PermissionAuthorizationHandler` | ✅ `PermButton`, `<Can>`, menu lọc quyền *(có sẵn)* |
| ⬜ Seed role `Admin`, `User`, permission, tài khoản admin *(chưa có seed)* | — |
| ⬜ Gán role `User` khi đăng ký *(user mới hiện không có role / quyền nào)* | — |
| ⬜ `GET /api/me` | ⬜ Header hiện tên đầy đủ, avatar |
| ⬜ `ICurrentUserService` → tự điền `CreatedBy` / `UpdatedBy` | — |
| ⬜ `GET / PUT /api/admin/nguoi-dung` (phân trang) | ⬜ Module `admin/nguoi-dung` (6 file: Const · Service · List · Form · Module · index) |
| ⬜ CRUD `/api/admin/vai-tro` + `PUT .../permissions` | ⬜ Module `admin/vai-tro` + màn chọn quyền |

---

## Pha 5 — Phiên đăng nhập & Audit

| BE | FE |
|---|---|
| ⬜ `GET / DELETE /api/sessions` *(entity `UserSession` đã có)* | ⬜ Trang "Thiết bị đang đăng nhập" |
| ⬜ `GET /api/me/login-history` | ⬜ Trang lịch sử đăng nhập |
| ⬜ `IAuditService` + `GET /api/admin/audit-logs` *(entity `AuditLog` đã có)* | ⬜ Trang nhật ký thao tác |

---

## Pha 6 — Google Login & Gói thành viên

| BE | FE |
|---|---|
| ⬜ `POST /api/auth/external-login` *(entity `ExternalLogin` đã có)* | ⬜ Nút "Đăng nhập bằng Google" |
| ⬜ `GET /api/plans`, `GET /api/me/subscription` *(entity `Subscription` đã có)* | ⬜ Trang gói, hiển thị AI credits |

---

## Sau này — Bản mobile (Android APK)

- [ ] FE: layout co giãn cho màn hình nhỏ (Sider → Drawer)
- [ ] BE: deploy lên máy chủ có HTTPS + CORS cho app
- [ ] FE: thêm Capacitor, script build APK
- [ ] 👤 Cài Android Studio
- [ ] iOS: cần máy Mac + tài khoản Apple Developer

---

## 👤 Việc chỉ bạn làm được

| Việc | Vì sao |
|---|---|
| Đổi mật khẩu `sa` SQL Server | Mật khẩu cũ nằm trong lịch sử git, xóa file không hết |
| Đặt JWT secret bằng `dotnet user-secrets set` | Secret phải nằm trên máy bạn, không nằm trong repo |
| Chốt có chặn login khi chưa xác thực email | Quyết định nghiệp vụ |
| Cài Android Studio (khi làm mobile) | Cài trên máy bạn |
