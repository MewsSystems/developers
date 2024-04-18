/// <reference types="vitest" />
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
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

// /// <reference types="vitest" />
// /// <reference types="vite/client" />
// import { defineConfig } from "vitest/config";
// import react from "@vitejs/plugin-react-swc";
// import "@testing-library/jest-dom/vitest";

// // https://vitejs.dev/config/
// export default defineConfig({
//   plugins: [react()],
// });
