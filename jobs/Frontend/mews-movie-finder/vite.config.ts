/// <reference types="vitest" />
import { defineConfig } from "vite";

import react from "@vitejs/plugin-react";
import { configDefaults } from "vitest/dist/config.js";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  test: {
    globals: true,
    setupFiles: ["./src/tests/testSetup.ts"],
    mockReset: false,
    environment: "happy-dom",
    coverage: {
      enabled: true,
      provider: "v8",
      all: true,
      exclude: [
        ...configDefaults.exclude,
        "**/App.tsx",
        "**/main.tsx",
        "**/*.d.ts",
        "**/.eslintrc.cjs",
      ],
    },
  },
});
