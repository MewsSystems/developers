import "@testing-library/react"
import "@testing-library/jest-dom/vitest"
import { afterAll, afterEach, beforeAll } from "vitest"

import { server } from "test/server"

beforeAll(() => server.listen({ onUnhandledRequest: "warn" }))
afterAll(() => server.close())
afterEach(() => server.resetHandlers())
