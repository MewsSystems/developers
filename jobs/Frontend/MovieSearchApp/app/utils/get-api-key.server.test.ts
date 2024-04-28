import { describe, expect, test, vi } from "vitest"
import { getApiKey } from "./get-api-key.server"

describe("getApiKey", () => {
  test("returns the API key", () => {
    vi.stubEnv("TMDB_API_KEY", "123")
    expect(getApiKey()).toBe("123")
    vi.unstubAllEnvs()
  })

  test("throws Response", () => {
    vi.stubEnv("TMDB_API_KEY", "")
    expect(() => getApiKey()).toThrow(Response)
    vi.unstubAllEnvs()
  })
})
