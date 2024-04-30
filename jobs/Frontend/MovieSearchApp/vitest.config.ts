/// <reference types="vitest" />

import react from "@vitejs/plugin-react"
import { defineConfig } from "vite"

export default defineConfig({
  plugins: [react()],
  css: { postcss: { plugins: [] } },
  resolve: {
    alias: [{ find: /^~\/(.+)/, replacement: "/app/$1" }],
  },
  test: {
    include: ["./app/**/*.test.{ts,tsx}"],
    restoreMocks: true,
    coverage: {
      include: ["app/**/*.{ts,tsx}"],
      all: true,
    },
  },
})
