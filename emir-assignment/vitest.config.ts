/// <reference types="vitest" />

import { defineConfig } from "vitest/config";

export default defineConfig({
    test: {
        include: ["tests/unit/**/*.{test,spec}.{ts,tsx}"], // only unit/integration
        exclude: [
            "node_modules/**",
            "dist/**",
            "tests/e2e/**", // << prevent Vitest from loading Playwright specs
        ],

        environment: "jsdom", // emulate browser
        setupFiles: "./tests/setup.ts", // setup after env ready
        css: true, // allow CSS imports in tests
        globals: true, // makes describe/it/expect global
        coverage: {
            reporter: ["text", "html"],
            exclude: ["node_modules/", "src/test/**", "src/**/*.d.ts"],
        },
    },
});
