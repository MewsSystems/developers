import "@testing-library/jest-dom";
import { afterEach } from "vitest";
import { cleanup } from "@testing-library/react";

// Auto-cleanup DOM between tests
afterEach(() => {
    cleanup();
});
