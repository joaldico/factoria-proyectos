import federation from '@originjs/vite-plugin-federation'
import react from '@vitejs/plugin-react'
import { defineConfig, loadEnv } from 'vite'

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  return {
    plugins: [
      react(),
      federation({
        name: 'factoria_host',
        remotes: {
          taximetroApp: env.VITE_TAXIMETRO_URL || 'http://localhost:3002/assets/remoteEntry.js',
        },
        shared: ['react', 'react-dom', 'react-router-dom', 'zustand', '@tanstack/react-query']
      })
    ],
    build: {
      sourcemap: false,
      chunkSizeWarningLimit: 1600,
      target: 'esnext' 
    },
    base: '/', 
  }
})