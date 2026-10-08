# Eco FE — Scaffold học tập

Khung front-end nhẹ sử dụng Vite + React + TypeScript để bạn tham khảo.

Quick start:

```bash
# 1) Cài dependencies
npm install

# 2) Chạy môi trường phát triển
npm run dev

# 3) Build production
npm run build
npm run preview
```

Files of interest:

- [package.json](package.json) — scripts và dependencies
- [vite.config.ts](vite.config.ts) — cấu hình Vite
- [src/main.tsx](src/main.tsx) — entry
- [src/App.tsx](src/App.tsx) — component chính

Gợi ý bảo mật: Đừng commit secrets. Với dev, giữ `package.json` và `vite.config.ts` công khai; nếu cần cấu hình API endpoint, sử dụng biến môi trường.

Environment:

- `VITE_API_BASE` — base URL tới backend API (ví dụ `http://localhost:5000`). Mặc định sẽ gọi relative path.
