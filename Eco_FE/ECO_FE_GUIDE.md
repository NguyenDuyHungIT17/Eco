# ECO_FE - Huong dan su dung va phat trien frontend

Tai lieu nay danh cho nguoi da biet Backend nhung chua quen voi Frontend.
Muc tieu la sau khi doc xong ban co the:

- Chay duoc `Eco_FE` tren may.
- Hieu mot ung dung React/Vite hoat dong nhu the nao.
- Hieu du lieu di tu giao dien den API Backend va quay lai.
- Hieu dung vai tro cua component, module, service, store, router va CSS.
- Tao mot module moi theo dung cau truc dang dung trong project.
- Goi API moi ma khong dat sai file hoac sai tang xu ly.
- Tu tim loi khi giao dien khong hien thi hoac API khong tra ket qua.

> Luu y: `Eco_FE` hien la frontend scaffold nho de ket noi cac API auth cua `Eco_BE`.
> No dang mo phong cach to chuc cua project `Frontends`, nhung chua phai ban sao day du cua toan bo `Frontends`.

---

## 1. Frontend la gi neu ban da biet Backend?

Neu Backend la noi xu ly nghiep vu, database va API, thi Frontend la phan chay tren trinh duyet cua nguoi dung.

Co the hieu don gian nhu sau:

```text
Nguoi dung thao tac
        |
        v
React component (form, button, bang du lieu)
        |
        v
Service (quy dinh goi endpoint nao, gui body gi)
        |
        v
HTTP request qua trinh duyet
        |
        v
Vite proxy trong moi truong dev
        |
        v
Eco_BE API
        |
        v
JSON response
        |
        v
Service -> component state -> React render lai giao dien
```

Vi du voi dang nhap:

1. Nguoi dung nhap username va password.
2. Component `modules/auth/Login.tsx` giu gia tri input bang `useState`.
3. Khi submit form, component goi `authService.login(...)`.
4. `AuthService` goi ham HTTP chung `post(...)`.
5. `post(...)` gui `POST /api/auth/login`.
6. Vite proxy chuyen request den `https://localhost:7038` trong luc development.
7. Backend tra JSON.
8. Service luu token vao `localStorage`.
9. Component nhan ket qua va hien thi trang phien dang nhap.

Trong Frontend, khong nen viet SQL, truy cap database, hoac viet business logic Backend.
Frontend chi nen:

- Hien thi du lieu.
- Thu thap input.
- Goi API.
- Xu ly loading, success, error.
- Quan ly trang thai dang dung cua giao dien.

---

## 2. Yeu cau moi truong

Can co:

- Node.js va npm.
- Backend `Eco_BE` dang chay neu muon test API that.
- Trinh duyet hien dai.
- VS Code.

Kiem tra Node va npm:

```powershell
node --version
npm --version
```

Neu chua co `node_modules`, cai dependency tai thu muc `Eco_FE`:

```powershell
cd D:\Project_Personal\Eco\Eco_FE
npm install
```

Khong chay lenh trong thu muc `Eco` neu muon build rieng frontend. Hay dam bao terminal dang o:

```text
D:\Project_Personal\Eco\Eco_FE
```

---

## 3. Cach chay project

### 3.1. Chay development

```powershell
cd D:\Project_Personal\Eco\Eco_FE
npm run dev
```

Vite se hien mot URL, thuong la:

```text
http://localhost:5173
```

Mo URL do tren trinh duyet.

Development server co cac dac diem:

- Tu dong build lai khi ban sua file.
- Bao loi ngay tren terminal hoac trinh duyet.
- Su dung proxy trong `vite.config.ts` de goi Backend HTTPS local.

### 3.2. Build production

```powershell
npm run build
```

Lenh nay da chay thanh cong trong project hien tai.
Ket qua duoc tao trong thu muc `dist/`.

Build khac voi dev server:

- Dev server phuc vu code de phat trien.
- Build tao file toi uu de deploy.
- Build khong tu dong cho ban xem giao dien.

### 3.3. Xem ban build

```powershell
npm run preview
```

Lenh nay phuc vu thu muc `dist` bang mot server local.

### 3.4. Canh bao CJS cua Vite

Neu thay:

```text
The CJS build of Vite's Node API is deprecated
```

Day la canh bao tu Vite, khong phai loi compile cua source code.
Neu van thay dong `built successfully` hoac dau `checkmark`, build da thanh cong.

---

## 4. Cau truc thu muc hien tai

Cau truc nguon chinh:

```text
Eco_FE/
|-- package.json
|-- tsconfig.json
|-- vite.config.ts
|-- index.html
|-- ECO_FE_GUIDE.md
`-- src/
    |-- main.tsx
    |-- App.tsx
    |-- AppRoutes.tsx
    |-- app-setting.ts
    |-- styles.css
    |-- layouts/
    |   |-- MainLayout.tsx
    |   `-- index.ts
    |-- modules/
    |   |-- auth/
    |   |   |-- Login.tsx
    |   |   `-- index.ts
    |   `-- home/
    |       |-- Dashboard.tsx
    |       `-- index.ts
    |-- shared/
    |   |-- services/
    |   |   |-- AuthService.ts
    |   |   `-- index.ts
    |   `-- utils/
    |       |-- http.ts
    |       `-- index.ts
    `-- store/
        |-- oauth.slice.ts
        `-- index.ts
```

### 4.1. `main.tsx`: diem khoi dong

`src/main.tsx` la entry point cua ung dung.
No lam ba viec:

1. Import React.
2. Tim phan tu HTML co id `root`.
3. Render component `App` vao phan tu do.
4. Import CSS toan cuc.

```tsx
createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
);
```

`StrictMode` giup phat hien mot so van de trong luc development.
No khong phai la mot giao dien va khong can tu sua neu ban chua hieu ro.

### 4.2. `App.tsx`: root component

`App.tsx` la component goc cua giao dien:

```tsx
export default function App() {
  return <div className="app"><AppRoutes /></div>;
}
```

Khong nen dat logic goi API lon vao `App.tsx`.
App nen lam nhiem vu ghep cac tang lon cua ung dung.

### 4.3. `AppRoutes.tsx`: cua vao routing

Hien tai file nay tra ve `MainLayout`:

```tsx
export function AppRoutes() {
  return <MainLayout />;
}
```

Project mau `Frontends` dung React Router va lazy module.
`Eco_FE` hien chua cai React Router, nen `AppRoutes` dang la lop trung gian de sau nay them route ma khong phai sua `App.tsx`.

Khi can them route that, co the cai router va chuyen logic vao file nay.
Khong nen import truc tiep tat ca page vao `App.tsx`.

### 4.4. `layouts/`: khung dung chung

`layouts/MainLayout.tsx` chua bo cuc lon:

- Header.
- Khu vuc noi dung chinh.
- Module trang home.
- Module auth.

Layout khong nen chua chi tiet cua tung nghiep vu.
Vi du, layout co the chua menu va header, nhung form tao san pham nen dat trong module san pham.

### 4.5. `modules/`: chia theo nghiep vu

Day la thu muc quan trong nhat khi lam theo phong cach `Frontends`.

Moi module nen dai dien cho mot khu vuc nghiep vu:

```text
modules/
|-- auth/
|-- home/
|-- nguoi-dung/
|-- san-pham/
`-- don-hang/
```

Trong project hien tai:

- `modules/auth/Login.tsx`: dang nhap, dang ky, refresh token, dang xuat.
- `modules/home/Dashboard.tsx`: noi dung gioi thieu trang chu.

Khong nen dat file `UserService.ts` vao `modules/auth` neu service do duoc dung boi nhieu man hinh.
Service dung chung nen dat trong `shared/services`.

### 4.6. `shared/services/`: noi goi API

Service la lop trung gian giua component va HTTP.

`shared/services/AuthService.ts` chua:

- Kieu du lieu request/response cua auth.
- Ham `login`.
- Ham `register`.
- Ham `refreshToken`.
- Ham `revokeToken`.

Component khong nen tu ghep URL API o nhieu noi.
Neu endpoint thay doi, chi sua service.

### 4.7. `shared/utils/`: ham dung chung

`shared/utils/http.ts` chua ham `post<T>` dung chung.
No lo cac viec lap lai:

- Tao request POST.
- Gan `Content-Type: application/json`.
- Serialize body bang `JSON.stringify`.
- Parse JSON response.
- Kiem tra HTTP status.
- Kiem tra truong `success` cua response.
- Gom message va errors thanh Error.

Component khong can lap lai nhung viec nay.

### 4.8. `store/`: trang thai dung chung

`store/oauth.slice.ts` dang quan ly token auth o muc don gian:

- `oauthActions.setToken(token)`: luu token vao bien trong bo nho va `localStorage`.
- `oauthActions.removeToken()`: xoa token.
- `getToken()`: doc token da luu.

Day la store tu viet nho, chua phai Redux store day du nhu project `Frontends`.
Neu sau nay co nhieu trang thai dung chung, co the chuyen sang Redux Toolkit theo mau `Frontends`.

### 4.9. `styles.css`: CSS toan ung dung

CSS dang duoc import mot lan trong `main.tsx`, nen cac class co the dung tren cac component.

Quy uoc hien tai:

- Bien mau dat trong `:root`.
- Class dat theo vai tro giao dien.
- Media query dat o cuoi file.
- Khong dat CSS truc tiep trong service.
- Khong dung ten class qua chung chung neu de trung voi module khac.

---

## 5. Alias import va vi sao khong dung dau `../`

Trong project nay co the viet:

```tsx
import MainLayout from 'layouts';
import Login from 'modules/auth';
import { authService } from 'shared/services';
import { oauthActions } from 'store';
```

Thay vi:

```tsx
import MainLayout from '../layouts/MainLayout';
import Login from '../../modules/auth/Login';
```

Cac alias duoc khai bao trong hai noi:

1. `tsconfig.json`: de TypeScript hieu duong dan.
2. `vite.config.ts`: de Vite/Rollup resolve duong dan khi build.

Alias hien co:

```text
app-setting -> src/app-setting.ts
layouts     -> src/layouts
modules     -> src/modules
shared      -> src/shared
store       -> src/store
```

Khi tao alias moi, phai cap nhat ca hai cau hinh.
Neu chi sua mot noi, editor co the khong bao loi nhung build se fail, hoac nguoc lai.

Quy tac de nho:

- Import noi bo theo alias neu import tu module khac.
- Import file cung module co the dung duong dan tuong doi.
- Khong tao alias cho tung file nho neu khong can.

---

## 6. Hieu React component

React component la ham tra ve giao dien.

Vi du:

```tsx
export default function Dashboard() {
  return (
    <section className="auth-intro">
      <h1>Chao mung den Eco</h1>
    </section>
  );
}
```

JSX trong React trong gan giong HTML, nhung co mot so quy tac:

- Dung `className`, khong dung `class`.
- Thuoc tinh JavaScript viet theo camelCase: `onClick`, `autoComplete`.
- Moi component phai tra ve mot root element, hoac Fragment `<>...</>`.
- JavaScript chen vao JSX bang `{}`.
- Event handler nhan mot ham, khong goi ham ngay khi render.

Dung:

```tsx
<button onClick={logout}>Dang xuat</button>
```

Khong dung:

```tsx
<button onClick={logout()}>Dang xuat</button>
```

Vi dong sai se goi `logout` ngay khi component render.

---

## 7. State: du lieu ma giao dien dang nho

Trong `Login.tsx`, moi input co state:

```tsx
const [username, setUsername] = useState('');
```

Co the doc nhu sau:

- `username`: gia tri hien tai.
- `setUsername`: ham cap nhat gia tri.
- `useState('')`: gia tri ban dau la chuoi rong.

Input controlled:

```tsx
<input
  value={username}
  onChange={(event) => setUsername(event.target.value)}
/>
```

Dong du lieu:

```text
Nguoi dung go phim
    -> onChange
    -> setUsername
    -> React cap nhat state
    -> component render lai
    -> input hien gia tri moi
```

State local phu hop cho:

- Gia tri form.
- Loading cua mot nut.
- Loi cua mot form.
- Tab dang chon.
- Dialog dang mo hay dong.

Store dung chung phu hop cho:

- Token dang nhap.
- User hien tai.
- Theme.
- Danh sach can dung o nhieu module.

Khong dua moi thu vao store. State chi dung trong mot component thi de local se don gian hon.

---

## 8. Doc Login.tsx tu tren xuong

### 8.1. Import

```tsx
import { useState } from 'react';
import {
  authService,
  type AuthResponse,
  type RegisterRequest,
} from 'shared/services';
```

- `useState`: hook cua React.
- `authService`: doi tuong goi API.
- `AuthResponse`, `RegisterRequest`: type TypeScript, chi dung de kiem tra code.
- Tu khoa `type` giup phan biet import type voi import runtime.

### 8.2. State mode

```tsx
const [mode, setMode] = useState<'login' | 'register'>('login');
```

Type union buoc `mode` chi co the la `login` hoac `register`.
Day la loi ich lon cua TypeScript: tranh go nham chuoi tuy y.

### 8.3. Submit form

```tsx
async function submit(event: React.FormEvent) {
  event.preventDefault();
  setLoading(true);
  setError('');
  setNotice('');

  try {
    // goi API
  } catch (submitError) {
    // hien loi
  } finally {
    setLoading(false);
  }
}
```

Trinh tu dung:

1. Chan trinh duyet reload trang bang `preventDefault()`.
2. Bat loading de tranh bam nhieu lan.
3. Xoa loi va thong bao cu.
4. Goi API trong `try`.
5. Hien loi trong `catch`.
6. Tat loading trong `finally`, ke ca khi API loi.

Day la mau xu ly can dung cho hau het form goi API.

### 8.4. Dang ky

Payload phai trung contract BE:

```tsx
const data: RegisterRequest = {
  username,
  email,
  password,
  fullName,
  phoneNumber,
};

const result = await authService.register(data);
```

Khong tu y doi ten `fullName` thanh `name` neu Backend dang nhan `fullName`.
Frontend type khong tu dong sua contract Backend.

### 8.5. Dang nhap

```tsx
const result = await authService.login({
  usernameOrEmail: username,
  password,
  deviceInfo: navigator.userAgent,
  ipAddress: '',
});
```

`navigator.userAgent` la thong tin trinh duyet.
`ipAddress` dang gui chuoi rong de Backend tu xu ly hoac bo qua theo contract hien tai.

### 8.6. Loading va error

Trong luc request:

```tsx
<button type="submit" disabled={loading}>
  {loading ? 'Dang gui...' : 'Dang nhap'}
</button>
```

Nguyen tac:

- Request bat dau: `loading = true`.
- Request ket thuc: `loading = false`.
- Loi phai duoc hien thi cho nguoi dung.
- Khong de loi chi xuat hien trong console ma khong co thong bao tren giao dien.

---

## 9. Contract API auth hien tai

`AuthService.ts` dang goi cac endpoint:

| Nghiep vu | Method | URL | Body |
|---|---|---|---|
| Dang ky | POST | `/api/auth/register` | `username`, `email`, `password`, `fullName`, `phoneNumber` |
| Dang nhap | POST | `/api/auth/login` | `usernameOrEmail`, `password`, `deviceInfo`, `ipAddress` |
| Lam moi token | POST | `/api/auth/refresh-token` | `{ refreshToken }` |
| Thu hoi token | POST | `/api/auth/revoke-token` | Chuoi JSON la refresh token |

Diem can chu y: `revoke-token` dang gui refresh token truc tiep lam JSON string:

```tsx
post<boolean>('/api/auth/revoke-token', refreshToken);
```

Khong tu y doi thanh:

```tsx
post<boolean>('/api/auth/revoke-token', { refreshToken });
```

Tru khi contract Backend duoc sua.

### 9.1. Dang ky

```json
{
  "username": "nguyen.van.a",
  "email": "a@example.com",
  "password": "mat-khau",
  "fullName": "Nguyen Van A",
  "phoneNumber": "0900000000"
}
```

### 9.2. Dang nhap

```json
{
  "usernameOrEmail": "nguyen.van.a",
  "password": "mat-khau",
  "deviceInfo": "Mozilla/5.0 ...",
  "ipAddress": ""
}
```

### 9.3. Response wrapper

`http.ts` ky vong response co dang gan nhu:

```json
{
  "success": true,
  "message": "Dang nhap thanh cong",
  "data": {
    "accessToken": "...",
    "refreshToken": "...",
    "expiresInSeconds": 3600
  },
  "errors": []
}
```

Neu BE tra `success: false`, `http.ts` se nem `Error` de component xu ly.

Neu BE tra status khac 2xx, request cung bi coi la loi.

---

## 10. Vite proxy va loi CORS

Trong `vite.config.ts`:

```ts
server: {
  proxy: {
    '/api': {
      target: 'https://localhost:7038',
      changeOrigin: true,
      secure: false,
    },
  },
},
```

Khi frontend goi:

```text
/api/auth/login
```

Trinh duyet gui den Vite dev server, sau do Vite chuyen tiep den:

```text
https://localhost:7038/api/auth/login
```

`secure: false` cho phep Vite chap nhan certificate HTTPS local tu Backend.
Chi nen dung cai nay cho development, khong coi day la cau hinh bao mat production.

Neu chay frontend bang `npm run dev`, proxy co tac dung.
Neu deploy frontend len mot domain khac, can mot trong cac cach:

- Cau hinh reverse proxy tren server.
- Cau hinh CORS o Backend.
- Dat `VITE_API_BASE` thanh origin Backend phu hop.

Loi thuong gap:

| Loi | Nguyen nhan kha nang cao |
|---|---|
| `Failed to fetch` | BE chua chay, sai port, certificate, CORS |
| `404` | Sai path endpoint hoac proxy khong chuyen dung |
| `401` | Token thieu, sai, het han |
| `400` | Body khong dung contract/validator |
| `500` | Loi phia Backend |

---

## 11. Quy trinh them mot API moi

Gia su BE co API lay danh sach san pham:

```text
GET /api/products?page=1&pageSize=20
```

Lam theo cac buoc sau.

### Buoc 1: Xac nhan contract Backend

Can biet:

- HTTP method.
- URL.
- Query string.
- Request body.
- Header can gui.
- Response wrapper.
- Response data.
- Cac loi validator.

Khong bat dau viet UI khi chua biet contract.

### Buoc 2: Tao service dung module phu hop

Neu chi dung trong module products, co the tao:

```text
src/modules/products/ProductsService.ts
```

Neu duoc nhieu module dung, tao:

```text
src/shared/services/ProductService.ts
```

Khong dat request API vao file `Dashboard.tsx` neu API do con duoc dung o noi khac.

### Buoc 3: Mo rong HTTP helper neu can

Hien `shared/utils/http.ts` moi co `post`.
Voi GET, them ham co cung cach xu ly response:

```ts
import { API_BASE_URL } from 'app-setting';

type ApiResponse<T> = {
  success?: boolean;
  message?: string;
  data?: T;
  errors?: string[];
};

export async function get<T>(path: string): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`);
  const body = await response.json().catch(() => null) as ApiResponse<T> | null;

  if (!response.ok || !body?.success || body.data === undefined) {
    throw new Error(
      [body?.message, ...(body?.errors || [])]
        .filter(Boolean)
        .join(' ') || 'Yeu cau khong thanh cong.',
    );
  }

  return body.data;
}
```

Trong source that, hay giu format va helper da co thay vi copy logic vao tung service.

### Buoc 4: Tao type response

```ts
type Product = {
  id: string;
  name: string;
  price: number;
};

type ProductListResponse = {
  items: Product[];
  totalCount: number;
};
```

Type giup editor goi y ten field va bao loi khi dung sai.

### Buoc 5: Tao service

```ts
import { get } from 'shared/utils';

export function getProducts() {
  return get<ProductListResponse>('/api/products?page=1&pageSize=20');
}
```

### Buoc 6: Tao module

```text
src/modules/products/
|-- ProductsService.ts
|-- ProductsPage.tsx
`-- index.ts
```

`index.ts`:

```ts
export { default } from './ProductsPage';
```

### Buoc 7: Goi service tu component bang state

```tsx
import { useEffect, useState } from 'react';
import { getProducts, type Product } from './ProductsService';

export default function ProductsPage() {
  const [items, setItems] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function load() {
      try {
        setItems((await getProducts()).items);
      } catch (loadError) {
        setError(
          loadError instanceof Error
            ? loadError.message
            : 'Khong tai duoc du lieu.',
        );
      } finally {
        setLoading(false);
      }
    }

    void load();
  }, []);

  if (loading) return <p>Dang tai...</p>;
  if (error) return <p className="error">{error}</p>;

  return (
    <ul>
      {items.map((item) => (
        <li key={item.id}>{item.name}</li>
      ))}
    </ul>
  );
}
```

### Buoc 8: Them vao route/layout

Hien `AppRoutes` chua router that, nen co the tam import module vao `MainLayout`.
Khi dung React Router, them route trong `AppRoutes.tsx`, theo phong cach project `Frontends`:

```tsx
{
  path: 'products/*',
  lazy: () => import('modules/products'),
}
```

---

## 12. Quy trinh tao form ghi du lieu

Mau chuan:

1. Tao type request.
2. Tao state cho moi input.
3. Tao ham submit.
4. Chan reload bang `preventDefault()`.
5. Bat loading.
6. Tao payload dung ten field BE.
7. Goi service.
8. Hien thong bao thanh cong.
9. Bat loi va hien loi.
10. Tat loading trong `finally`.

Vi du ngan:

```tsx
const [name, setName] = useState('');
const [loading, setLoading] = useState(false);
const [error, setError] = useState('');

async function submit(event: React.FormEvent) {
  event.preventDefault();
  setLoading(true);
  setError('');

  try {
    await productService.create({ name });
  } catch (submitError) {
    setError(
      submitError instanceof Error
        ? submitError.message
        : 'Khong luu duoc.',
    );
  } finally {
    setLoading(false);
  }
}
```

Khong de `loading` chi bat ma khong tat.
Neu quen `finally`, khi API loi nut co the bi khoa mai.

---

## 13. Quy uoc format source code

Project hien co TypeScript/TSX va dang dung semicolon trong mot so file da duoc format.
Khi sua file, hay giu style cua file do.

### 13.1. Dat ten

- Component React: PascalCase, vi du `ProductsPage.tsx`.
- Service: PascalCase, vi du `ProductService.ts`.
- Ham: camelCase, vi du `getProducts`.
- Type: PascalCase, vi du `ProductResponse`.
- Bien: camelCase.
- Module folder: ten nghiep vu ngan gon, vi du `products`.
- File barrel: `index.ts` de export public API cua module.

### 13.2. Import

Nhom import theo thu tu:

1. React va thu vien ben ngoai.
2. Alias noi bo.
3. Relative import trong cung module.

Vi du:

```tsx
import { useEffect, useState } from 'react';

import { getProducts } from 'shared/services';
import { formatCurrency } from 'shared/utils';

import ProductRow from './ProductRow';
```

### 13.3. Component

- Moi component co mot nhiem vu ro rang.
- Page/module dieu phoi state va service.
- Component con nhan du lieu qua props.
- Khong goi API trong component nho neu page da co the quan ly request.

### 13.4. Comment

Chi viet comment khi can giai thich ly do hoac mot doan logic kho.

Nen:

```tsx
/*
 * Neu da dang nhap, hien thao tac quan ly phien.
 */
```

Khong nen:

```tsx
// Gan loading bang true
setLoading(true);
```

Vi code da tu giai thich.

### 13.5. JSX dai

Khi JSX dai, format moi prop tren mot dong.
Dieu nay de doc va de review hon:

```tsx
<input
  type="email"
  value={email}
  onChange={(event) => setEmail(event.target.value)}
  required
/>
```

### 13.6. Kiem tra sau moi thay doi

Sau khi sua code:

```powershell
npm run build
```

Neu build fail, doc loi dau tien trong terminal, sua loi do truoc.
Khong sua ngau nhien nhieu file cung luc.

---

## 14. Cach debug tu Backend sang Frontend

### 14.1. Xac dinh request co duoc gui khong

Mo DevTools trong trinh duyet:

1. Bam `F12`.
2. Mo tab `Network`.
3. Thuc hien thao tac tren giao dien.
4. Tim request `/api/auth/login`.
5. Kiem tra `Request Payload`.
6. Kiem tra `Status Code`.
7. Kiem tra tab `Response`.

### 14.2. Neu khong thay request

Kha nang:

- Button khong nam trong form.
- `onSubmit` chua gan.
- Component chua duoc render.
- JavaScript loi truoc khi submit.
- Button dang bi `disabled`.

Xem tab `Console`.

### 14.3. Neu request co nhung bi 400

So sanh payload voi DTO/validator BE:

- Ten field co dung khong?
- Field co bi thieu khong?
- Kieu du lieu co dung khong?
- Email va phone co dung format khong?
- Password co du do dai khong?

### 14.4. Neu bi 401

- Access token co ton tai khong?
- Header `Authorization` da gan chua?
- Token co het han khong?
- Refresh token co con hop le khong?
- Backend co dung scheme `Bearer` khong?

Luu y: HTTP helper hien tai chua tu dong gan `Authorization` cho moi request.
Khi them API can auth, can mo rong helper/service de gui:

```ts
headers: {
  'Content-Type': 'application/json',
  Authorization: `Bearer ${accessToken}`,
}
```

### 14.5. Neu bi CORS hoac Failed to fetch

Kiem tra theo thu tu:

1. BE co dang chay khong?
2. Port trong `vite.config.ts` co dung khong?
3. Backend co redirect HTTP sang HTTPS khong?
4. Certificate local co hop le khong?
5. Request co di qua proxy khong?
6. Neu deploy that, Backend da cau hinh CORS chua?

---

## 15. Nhung dieu can biet ve ban hien tai

Day la cac diem hien tai ban nen biet truoc khi phat trien tiep.

### 15.1. Token duoc luu nhung giao dien chua khoi phuc token

`oauth.slice.ts` co `getToken()` de doc token tu `localStorage`.
Tuy nhien `Login.tsx` hien khoi tao state:

```tsx
const [token, setToken] = useState<AuthResponse | null>(null);
```

Vi vay sau khi reload trang, giao dien co the quay ve man hinh login du token van con trong localStorage.
Day la viec can bo sung neu muon giu phien dang nhap sau reload:

```tsx
const [token, setToken] = useState<AuthResponse | null>(() => getToken());
```

Khi ap dung, import `getToken` tu `store`.
Can them xu ly token het han trong ban production.

### 15.2. HTTP helper moi co POST

Hien tai `shared/utils/http.ts` moi co `post`.
Khi viet man hinh danh sach, can them `get`.
Khi upload file, can them helper cho `FormData` va khong gan `Content-Type` bang tay.

### 15.3. Chua co Authorization tu dong

Login luu token nhung cac request khac chua tu dong doc token va gan Bearer header.
Do do, truoc khi viet API can quyen, phai bo sung tang HTTP auth.

### 15.4. AppRoutes chua phai router day du

Noi dung hien tai chi render `MainLayout`.
Neu co nhieu trang, nen cai React Router va dua route vao `AppRoutes.tsx`, theo cach project `Frontends` dang lam.

### 15.5. CSS class can dong bo voi JSX

Khi dat class trong JSX, ten class phai ton tai trong `styles.css`.
Neu component dung class moi ma CSS chua co, giao dien van co the chay nhung khong duoc style.

Vi du, neu JSX dung `className="button-primary"` thi CSS phai co `.button-primary`.

---

## 16. Lo trinh hoc Frontend de lam viec voi BE

### Giai doan 1: HTML va CSS

Hoc:

- The semantic: `form`, `label`, `input`, `button`, `main`, `header`.
- Box model: margin, padding, border.
- Flexbox va Grid.
- Responsive media query.
- Class CSS va cascade.

Bai tap:

- Tao form login bang HTML.
- Can giua form tren desktop va mobile.
- Hien thi loi mau do.

### Giai doan 2: JavaScript tren trinh duyet

Hoc:

- Variable, function, object, array.
- Destructuring.
- Promise va `async/await`.
- `fetch`.
- Event va event handler.
- JSON.

Bai tap:

- Goi mot API bang `fetch`.
- In response ra console.
- Bat loi bang `try/catch`.

### Giai doan 3: React

Hoc:

- Component.
- Props.
- State.
- Conditional rendering.
- List va `key`.
- Form controlled.
- `useEffect` cho load data.

Bai tap:

- Tao list user.
- Tao form create user.
- Them loading va error.

### Giai doan 4: TypeScript

Hoc:

- Type primitive.
- Object type.
- Union type.
- Optional field.
- Generic.
- Type import.
- Type response API.

Bai tap:

- Khai bao DTO frontend trung DTO BE.
- Khong dung `any` neu co the biet type.

### Giai doan 5: Cau truc ung dung

Hoc:

- Service layer.
- Shared utility.
- Store.
- Routing.
- Authentication.
- Authorization.
- Error boundary.
- Code splitting/lazy loading.

Day la giai doan ma ban co the mo rong `Eco_FE` theo project `Frontends`.

---

## 17. Checklist khi tao module moi

Truoc khi code:

- [ ] Da doc API contract tu Backend.
- [ ] Biet method, URL, body va response.
- [ ] Biet API co can Bearer token khong.
- [ ] Xac dinh module nghiep vu.

Khi tao file:

- [ ] Tao folder trong `src/modules/<ten-module>`.
- [ ] Tao page/component chinh.
- [ ] Tao `index.ts` de export module.
- [ ] Tao service dung tang phu hop.
- [ ] Khai bao type request/response.
- [ ] Dung alias import dung quy uoc.
- [ ] Xu ly loading.
- [ ] Xu ly error.
- [ ] Xu ly empty state neu la danh sach.
- [ ] Gan `key` khi render array.
- [ ] Dat class CSS co y nghia.

Sau khi code:

- [ ] Chay `npm run build`.
- [ ] Mo DevTools Network.
- [ ] Test success.
- [ ] Test sai input.
- [ ] Test BE tat.
- [ ] Test reload trang neu co auth.
- [ ] Test man hinh mobile.

---

## 18. Mau luong lam viec hang ngay

```text
1. Pull code moi nhat
2. Chay npm install neu package thay doi
3. Chay npm run dev
4. Doc endpoint BE can lam
5. Tao type request/response
6. Tao service
7. Tao component/module
8. Noi module vao route
9. Test tren trinh duyet
10. Kiem tra Network va Console
11. Chay npm run build
12. Review diff truoc khi commit
```

Khi gap loi, hay phan loai truoc:

- Loi compile/type: xem terminal va TypeScript.
- Loi render: xem Console va component tree.
- Loi API: xem Network, status code, payload, response.
- Loi layout: xem Elements va CSS computed styles.
- Loi auth: xem token, header, expiry, Backend log.

---

## 19. Tom tat cac nguyen tac can nho

1. Component hien thi va dieu khien tuong tac.
2. Service goi API.
3. Utility xu ly logic dung chung.
4. Store giu trang thai dung chung.
5. Module gom code theo nghiep vu.
6. Layout gom bo cuc lon.
7. Route quyet dinh trang nao duoc hien thi.
8. Type phai khop contract Backend.
9. Loading va error la mot phan bat buoc cua flow API.
10. Moi thay doi phai build lai.
11. Khong copy logic HTTP vao tung component.
12. Khong dat business logic Backend vao Frontend.
13. Khong sua ten field request neu chua doi contract BE.
14. Alias import phai dong bo giua TypeScript va Vite.
15. Khi mo rong project, uu tien giu cau truc `modules`, `shared`, `layouts`, `store`.

Neu ban nam duoc 15 nguyen tac tren, ban da co nen tang du de tiep tuc noi cac API con lai cua `Eco_BE` vao `Eco_FE` mot cach co to chuc.
