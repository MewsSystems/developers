/// <reference types="vitest" />
import { defineConfig, loadEnv } from "vite";
import { resolve } from "path";
import react from "@vitejs/plugin-react-swc";

// https://vitejs.dev/config/
export default defineConfig(({ mode, command }) => {
  const env = loadEnv(mode, process.cwd(), "");

  return {
    plugins: [react()],
    test: {
      globals: true,
      environment: "jsdom",
      setupFiles: ["./src/tests/setup.ts"],
    },
    resolve: {
      alias: {
        "@": resolve(__dirname, "src"),
      },
    },
    define: {
      "process.env.TMDB_ACCESS_TOKEN": JSON.stringify(env.TMDB_ACCESS_TOKEN),
    },
  };
});
