import react from "@vitejs/plugin-react"
import { defineConfig } from "vitest/config"

export default defineConfig({
  plugins: [react()],
  test: {
    globals: true,
    environment: "jsdom",
    setupFiles: ["./src/test/setup.ts"],
    env: {
      VITE_ENABLE_MSW: "true",
      VITE_TMDB_BASE_URL: "https://api.themoviedb.org/3",
    },
  },
})
