# Lộ trình học Frontend (React) — dành cho người đã biết Backend .NET

> Học **song song với làm Eco**: mỗi giai đoạn đều có bài tập làm thẳng vào Eco_FE.
> Kế hoạch sửa code: [FE_REFACTOR_PLAN.md](FE_REFACTOR_PLAN.md)
>
> Học xong giai đoạn nào thì đổi `[ ]` → `[x]`.

---

## 🗺️ Tổng quan

| # | Giai đoạn | Thời gian* | Kết quả trong Eco |
|:-:|---|:-:|---|
| 1 | JavaScript hiện đại | 1–2 tuần | Đọc hiểu mọi file `.ts` trong Eco_FE |
| 2 | TypeScript | 1 tuần | Viết `shared/types/api.ts` |
| 3 | React cơ bản | 2 tuần | Viết lại `Login` / `Register` |
| 4 | Router + Layout | 1 tuần | `AppRoutes`, `MainLayout`, chặn trang |
| 5 | Gọi API + Auth | 1–2 tuần | `httpClient` tự refresh token, `AuthService` |
| 6 | State: Redux Toolkit | 1 tuần | `store/oauth.slice` + persist |
| 7 | antd: Table, Form | 2 tuần | Module CRUD `admin.nguoi-dung` |
| 8 | Phân quyền (CASL) | 3–5 ngày | Ẩn/hiện nút, menu theo quyền |
| 9 | TanStack Query | 1 tuần | Bỏ `useEffect` gọi API trong List |
| 10 | Chất lượng: Lint, Test | 1 tuần | ESLint, Vitest, CI |
| 11 | Nâng cao | liên tục | Tối ưu, deploy, Next.js… |

\* Học khoảng 1–2 giờ mỗi ngày. Tổng khoảng **3 tháng** để làm FE tự tin.

```text
JS ─► TS ─► React ─► Router ─► API/Auth ─► Redux ─► antd ─► CASL ─► TanStack Query ─► Test
                       └──────────── làm Eco Pha 2 ────────────┘   └── Eco Pha 4 ──┘
```

---

## 🔁 Bảng "dịch" từ .NET sang React

Dùng bảng này để hiểu nhanh — mỗi khái niệm FE đều có thứ tương tự bên BE.

| .NET (đã biết) | React / FE | Ghi chú |
|---|---|---|
| `Program.cs` | `main.tsx` + `App.tsx` | Nơi khởi động, đăng ký "provider" |
| DI container (`AddScoped`) | Context Provider / import module | FE không có DI chuẩn, thường import thẳng |
| Controller + Route | `AppRoutes.tsx` (React Router) | URL → component |
| Middleware (`[Authorize]`) | `AuthorizedLayout` / route guard | Chặn trang khi chưa đăng nhập |
| Razor View / `.cshtml` | Component `.tsx` (JSX) | Giao diện |
| Model / DTO | `type` / `interface` TypeScript | Phải khớp DTO bên BE |
| Service + `HttpClient` | `shared/services` + axios | Gọi API |
| `DelegatingHandler` | axios interceptor | Gắn token, xử lý 401 |
| `async Task<T>` | `async function (): Promise<T>` | Giống gần như 100% |
| LINQ `Select / Where` | `array.map / filter` | |
| FluentValidation | antd Form `rules` / Zod | Validate phía client |
| Session / Cache | Redux store / TanStack Query cache | |
| xUnit | Vitest | |
| NuGet | npm | `package.json` ≈ `.csproj` |

---

## Giai đoạn 1 — JavaScript hiện đại (ES6+)

**Học gì**
- [ ] `let / const`, arrow function `() => {}`
- [ ] Destructuring: `const { a, b } = obj`, `const [x, setX] = arr`
- [ ] Spread / rest: `{ ...obj, a: 1 }`, `[...arr, item]`
- [ ] Template string: `` `Xin chào ${name}` ``
- [ ] Mảng: `map`, `filter`, `find`, `some`, `reduce`, `includes`
- [ ] Optional chaining `a?.b` và `??`
- [ ] `import / export` (ES Modules)
- [ ] Promise, `async / await`, `try / catch / finally`
- [ ] `fetch` và JSON
- [ ] **Tham chiếu và bất biến (immutability)**: vì sao React không cho sửa trực tiếp object trong state

**Tài liệu**
- 📘 [javascript.info](https://javascript.info) — phần 1 (bỏ qua phần trình duyệt cũ)
- 📘 [MDN – JavaScript Guide](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide)

**Bài tập trong Eco**
- Đọc hiểu từng dòng của `src/shared/utils/http.ts` và `src/store/oauth.slice.ts` hiện tại.
- Mở DevTools → Console, tự gọi `fetch('/api/auth/login', …)` bằng tay.

**✔ Tự kiểm tra:** giải thích được `const next = { ...state, items: [...state.items, x] }` khác gì `state.items.push(x)`.

---

## Giai đoạn 2 — TypeScript

**Học gì**
- [ ] Kiểu cơ bản, `type` và `interface`
- [ ] Union `'login' | 'register'`, optional `name?: string`
- [ ] Generic `ApiResponse<T>` (giống `ApiResponse<T>` bên C#)
- [ ] `Partial<T>`, `Record<K, V>`, `Pick`, `Omit`
- [ ] `unknown` thay cho `any`, thu hẹp kiểu (`instanceof`, `typeof`)
- [ ] `import type`

**Tài liệu**
- 📘 [TypeScript Handbook](https://www.typescriptlang.org/docs/handbook/intro.html)
- 📘 [React TypeScript Cheatsheet](https://react-typescript-cheatsheet.netlify.app)

**Bài tập trong Eco**
- Viết `src/shared/types/api.ts` (Bước 2 trong FE_REFACTOR_PLAN).
- Viết type cho `LoginRequest`, `RegisterRequest`, `AuthTokens` khớp với DTO bên `Eco.Application/DTOs/Auth`.

**✔ Tự kiểm tra:** chạy `npx tsc --noEmit` không có lỗi.

---

## Giai đoạn 3 — React cơ bản

**Học gì**
- [ ] Component, JSX, props
- [ ] `useState` — state là gì, vì sao gọi `setX` thì giao diện vẽ lại
- [ ] Render có điều kiện (`cond && <A/>`, `a ? <A/> : <B/>`)
- [ ] Render danh sách + `key`
- [ ] Sự kiện (`onClick`, `onSubmit`, `preventDefault`)
- [ ] Form controlled
- [ ] `useEffect` — **khi nào cần và khi nào KHÔNG cần**
- [ ] `useMemo`, `useCallback`, `useRef` (biết dùng khi nào)
- [ ] Tự viết custom hook (`useXxx`)
- [ ] Context (`createContext`, `useContext`)

**Tài liệu**
- ⭐ [react.dev/learn](https://react.dev/learn) — **tài liệu chính thức, đọc hết phần "Learn"**
- ⭐ [You Might Not Need an Effect](https://react.dev/learn/you-might-not-need-an-effect) — đọc kỹ, đây là lỗi phổ biến nhất

**Bài tập trong Eco**
- Tách `Login.tsx` hiện tại thành `Login.tsx` và `Register.tsx`.
- Viết hook `useAuthError(errorCode)` trả câu thông báo tiếng Việt.

**✔ Tự kiểm tra:** giải thích được vì sao `Login.tsx` cũ mất trạng thái đăng nhập khi F5.

---

## Giai đoạn 4 — Router & Layout

**Học gì**
- [ ] `createBrowserRouter`, `RouterProvider`
- [ ] Route lồng nhau + `<Outlet />`
- [ ] `lazy` — chia nhỏ code, mở trang nào tải trang đó
- [ ] `useNavigate`, `useParams`, `useSearchParams`, `<Link>`, `<Navigate>`
- [ ] Route guard (chặn trang khi chưa đăng nhập)
- [ ] Route `*` cho trang 404

**Tài liệu**
- 📘 [React Router – docs](https://reactrouter.com)
- 🔍 Đọc mẫu: `Frontends/src/AppRoutes.js`, `Frontends/src/layouts/MainLayout.js`, `modules/dm.dan_toc/Module.js`

**Bài tập trong Eco**
- Bước 6 trong FE_REFACTOR_PLAN: `AppRoutes`, `MainLayout`, `AuthLayout`.
- Vào `/nguoi-dung` khi chưa đăng nhập → về `/login?returnUrl=…` → đăng nhập xong quay lại đúng trang.

**✔ Tự kiểm tra:** giải thích được vì sao `Module.js` của mẫu dùng `path: 'dan-toc/*'` (có `/*`).

---

## Giai đoạn 5 — Gọi API & Xác thực

**Học gì**
- [ ] axios: instance, `baseURL`, `params`
- [ ] Interceptor request (gắn Bearer) và response (bắt 401)
- [ ] Luồng access token + refresh token, **hàng đợi refresh** (nhiều request cùng 401)
- [ ] CORS là gì, vì sao dùng proxy của Vite
- [ ] Lưu token ở đâu: localStorage, cookie HttpOnly — ưu nhược điểm
- [ ] Đọc tab **Network** trong DevTools thành thạo

**Tài liệu**
- 📘 [axios – Interceptors](https://axios-http.com/docs/interceptors)
- 📘 [MDN – CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS)
- 🔍 Đọc mẫu: `Frontends/src/shared/utils/http/httpClient.js` (hàm `handle401Error`)

**Bài tập trong Eco**
- Bước 4 + 5 trong FE_REFACTOR_PLAN: `httpClient.ts`, `AuthService.ts`.
- Thử nghiệm: đặt `ExpiryMinutes = 1` ở BE → chờ 1 phút → bấm thao tác → token tự refresh.

**✔ Tự kiểm tra:** vẽ lại được trên giấy luồng: request → 401 → refresh → gọi lại request.

---

## Giai đoạn 6 — State dùng chung: Redux Toolkit

**Học gì**
- [ ] Khi nào cần store chung, khi nào chỉ cần `useState`
- [ ] `createSlice`, `configureStore`, action, reducer
- [ ] `useSelector`, `useDispatch` (bản có kiểu: `useAppSelector`)
- [ ] `redux-persist`: giữ state khi F5
- [ ] Redux DevTools (extension trình duyệt)
- [ ] Biết thêm: **Zustand** — cách đơn giản hơn, rất phổ biến hiện nay

**Tài liệu**
- ⭐ [Redux Toolkit – Quick Start](https://redux-toolkit.js.org/tutorials/quick-start)
- 📘 [Redux Toolkit – TypeScript](https://redux-toolkit.js.org/tutorials/typescript)
- 🔍 Đọc mẫu: `Frontends/src/store/oauth.slice.js`, `store/store.js`

**Bài tập trong Eco**
- Bước 3 trong FE_REFACTOR_PLAN: `oauth.slice.ts`, `store.ts`, `hooks.ts`.
- Header hiển thị `username` lấy từ store.

**✔ Tự kiểm tra:** giải thích được vì sao **không nên** tạo slice cho từng danh mục như mẫu.

---

## Giai đoạn 7 — Thư viện giao diện antd

**Học gì**
- [ ] `ConfigProvider` (theme, tiếng Việt), `App.useApp()` (message, modal)
- [ ] Bố cục: `Layout`, `Menu`, `Row / Col`, `Space`, `Card`
- [ ] **`Form`**: `Form.Item`, `rules`, `form.setFieldsValue`, `onFinish`
- [ ] **`Table`**: `columns`, `render`, `pagination`, `loading`, `rowKey`
- [ ] `Modal`, `Drawer`, `Popconfirm`, `Select`, `DatePicker`
- [ ] Biết thêm: **shadcn/ui + Tailwind** — lựa chọn phổ biến cho web không phải admin

**Tài liệu**
- 📘 [Ant Design – Components](https://ant.design/components/overview)
- 🔍 Đọc mẫu: `Frontends/src/modules/dm.dan_toc/` — đọc **cả 6 file**

**Bài tập trong Eco**
- Viết lại `Login` / `Register` bằng antd `Form`.
- Module CRUD đầu tiên `admin.nguoi-dung` theo mục 5 của FE_REFACTOR_PLAN (cần BE Pha 4).
- Trong lúc chờ BE: làm module giả với dữ liệu tĩnh để luyện `Table` + `Form`.

**✔ Tự kiểm tra:** tạo một module CRUD mới trong < 1 giờ bằng cách copy 6 file.

---

## Giai đoạn 8 — Phân quyền với CASL

**Học gì**
- [ ] `ability.can(...)`, `<Can I="..." a="...">`
- [ ] Cập nhật quyền khi đăng nhập / đăng xuất
- [ ] Ẩn nút, ẩn menu, chặn route theo quyền
- [ ] Hiểu: **FE ẩn nút chỉ là trải nghiệm — BE mới là nơi chặn thật**

**Tài liệu**
- 📘 [CASL – React](https://casl.js.org/v6/en/package/casl-react)
- 🔍 Đọc mẫu: `Frontends/src/shared/services/CaslService.js`, các nút có `perm=` trong `Form.js`

**Bài tập trong Eco**
- `CaslService.ts` + component `PermButton` (nút tự ẩn khi không có quyền).
- Menu Sider lọc theo quyền.

---

## Giai đoạn 9 — TanStack Query (cách làm hiện đại)

> Đây là phần **mẫu không có** nhưng gần như mọi dự án React mới đều dùng.

**Học gì**
- [ ] Phân biệt **server state** (dữ liệu từ API) và **client state** (token, theme)
- [ ] `useQuery` — tự loading, tự cache, tự gọi lại
- [ ] `useMutation` + `invalidateQueries` — lưu xong tự làm mới danh sách
- [ ] Query key, `staleTime`

**Tài liệu**
- ⭐ [TanStack Query – docs](https://tanstack.com/query/latest/docs/framework/react/overview)

**Bài tập trong Eco**
- Viết lại `List.tsx` của `admin.nguoi-dung`: bỏ `useEffect + useState(loading)` → dùng `useQuery`.
- Lưu ở `Form.tsx` xong → `invalidateQueries` (thay cho `sendToView` Subject của mẫu).

**✔ Tự kiểm tra:** so sánh số dòng code List trước và sau khi dùng TanStack Query.

---

## Giai đoạn 10 — Chất lượng code

**Học gì**
- [ ] ESLint (flat config) + `eslint-plugin-react-hooks`
- [ ] Prettier — format tự động khi lưu file
- [ ] Vitest + Testing Library: test hàm, test component
- [ ] MSW: giả lập API khi test
- [ ] (Tuỳ chọn) Playwright: test end-to-end trên trình duyệt thật
- [ ] GitHub Actions: tự chạy lint + test + build

**Tài liệu**
- 📘 [Vitest](https://vitest.dev/guide/)
- 📘 [Testing Library – React](https://testing-library.com/docs/react-testing-library/intro)
- 📘 [MSW](https://mswjs.io/docs)

**Bài tập trong Eco**
- Test `oauth.slice` (giải mã token).
- Test `Login`: nhập sai → hiện "Sai tên đăng nhập hoặc mật khẩu."

---

## Giai đoạn 11 — Nâng cao (học dần)

- [ ] Tối ưu hiệu năng: React DevTools Profiler, `memo`, ảo hóa danh sách dài
- [ ] Error Boundary, trang lỗi đẹp
- [ ] React Hook Form + Zod (form phức tạp, ngoài antd)
- [ ] Đa ngôn ngữ (i18next)
- [ ] Build & deploy: `vite build`, Nginx / IIS, biến môi trường production
- [ ] Bảo mật FE: XSS, `dangerouslySetInnerHTML`, nơi lưu token
- [ ] Next.js (React chạy cả phía server) — khi cần SEO hoặc làm web công khai

---

## 📚 Nguồn học chính

| Loại | Nguồn | Dùng cho |
|---|---|---|
| ⭐ Chính thức | [react.dev](https://react.dev) | React — đọc đầu tiên |
| ⭐ Chính thức | [javascript.info](https://javascript.info) | JavaScript |
| ⭐ Chính thức | [TypeScript Handbook](https://www.typescriptlang.org/docs/handbook/intro.html) | TypeScript |
| Chính thức | [MDN Web Docs](https://developer.mozilla.org) | Tra cứu HTML / CSS / JS / HTTP |
| Chính thức | [Redux Toolkit](https://redux-toolkit.js.org), [React Router](https://reactrouter.com), [TanStack Query](https://tanstack.com/query), [Ant Design](https://ant.design) | Thư viện dùng trong Eco |
| Tham khảo cấu trúc | [bulletproof-react](https://github.com/alan2207/bulletproof-react) | Cách tổ chức dự án React lớn được nhiều người dùng |
| Tham khảo | [roadmap.sh/frontend](https://roadmap.sh/frontend), [roadmap.sh/react](https://roadmap.sh/react) | Bức tranh tổng thể |
| Dự án mẫu | `HCMA.CTCT/Frontends` | Cấu trúc thực tế ở công ty |

---

## 🧭 Cách học hiệu quả

1. **Đọc → làm ngay trong Eco → so với mẫu.** Đừng đọc lý thuyết quá 2 ngày mà không viết code.
2. **Mỗi ngày mở DevTools.** Tab Network và Console là "debugger" quan trọng nhất của FE.
3. **Đọc mẫu có chọn lọc.** Đối chiếu với cột ❌ trong FE_REFACTOR_PLAN để biết chỗ nào không nên chép.
4. **Gõ lại, đừng copy.** Copy code mẫu thì nhanh nhưng không nhớ.
5. **Ghi chú vào file này.** Thêm mục "Ghi chú" bên dưới mỗi giai đoạn: lỗi đã gặp, cách sửa.

---

## 📝 Nhật ký học

| Ngày | Đã học | Đã làm trong Eco | Vướng mắc |
|---|---|---|---|
| | | | |
