import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
  test: {
    include: ['test/**/*.{test,spec}.{js,mjs,cjs,ts,mts,cts,jsx,tsx}'],
    coverage: {
      reporter: ['text', 'lcov'],
    },
    environment: 'jsdom',
    globals: true,
  },
  resolve: {
    alias: {
      '@': resolve(__dirname, './src'),
      '@core': resolve(__dirname, './src/core'),
      '@services': resolve(__dirname, './src/services'),
      '@app': resolve(__dirname, './src/app'),
      '@test': resolve(__dirname, './test')
    },
  },
});
