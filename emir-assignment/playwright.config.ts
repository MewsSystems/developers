import { defineConfig, devices } from "@playwright/test";

export default defineConfig({
    testDir: "tests/e2e",
    timeout: 30_000,
    use: {
        baseURL: "http://localhost:5173",
        trace: "on-first-retry",
    },
    projects: [{ name: "chromium", use: { ...devices["Desktop Chrome"] } }],
    webServer: {
        command: "vite",
        url: "http://localhost:5173",
        reuseExistingServer: true,
    },
});
