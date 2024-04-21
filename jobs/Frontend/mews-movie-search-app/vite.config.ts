/// <reference types="vitest" />
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import tsconfigPaths from "vite-tsconfig-paths";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), tsconfigPaths()],
  test: {
    // include: ["./{src}/**/*.test.{ts,tsx}"],
    globals: true,
    environment: "jsdom",
    setupFiles: "./tests/setup.ts",
    poolOptions: {
      threads: {
        singleThread: true,
      },
    },
  },
});
