import { resolve } from "node:path"

import react from "@vitejs/plugin-react"
import { defineConfig } from "vitest/config"

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: [{ find: "@", replacement: resolve(__dirname, "./src") }],
  },
  test: {
    globals: true,
    environment: "jsdom",
    include: ["src/**/*.test.{ts,tsx}"],
    exclude: ["/node_modules/", "/.next/"],
    setupFiles: "./vitest.setup.mts",
    coverage: {
      provider: "v8",
      reporter: ["html"],
      include: ["**/*.tsx"],
    },
  },
})
