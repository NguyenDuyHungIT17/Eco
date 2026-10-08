import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'node:path'

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      'app-setting': path.resolve(__dirname, 'src/app-setting.ts'),
      layouts: path.resolve(__dirname, 'src/layouts'),
      modules: path.resolve(__dirname, 'src/modules'),
      shared: path.resolve(__dirname, 'src/shared'),
      store: path.resolve(__dirname, 'src/store'),
    },
  },
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7038',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
