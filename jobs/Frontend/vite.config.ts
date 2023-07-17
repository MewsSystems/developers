/// <reference types="vitest" />

import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

const API_URL = "https://api.themoviedb.org/3";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/api": {
        target: API_URL,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ""),
      },
    },
    port: 3000,
  },
  test: {
    reporters: "verbose",
    globals: true,
    environment: "jsdom",
    testTimeout: 30000,
    setupFiles: ["./vitest.setup.ts"],
  },
});
