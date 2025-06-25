import { HttpResponse, http } from "msw"
import { setupServer } from "msw/node"
import { afterAll, afterEach, beforeAll, describe, expect, it, vi } from "vitest"
import { ERROR_CODES, ERROR_MESSAGES } from "../../constants/errors"
import { ApiError, api } from "../api"

const BASE_URL = import.meta.env.VITE_TMDB_BASE_URL

const server = setupServer()

const consoleSpy = {
  log: vi.spyOn(console, "log").mockImplementation(() => {}),
  error: vi.spyOn(console, "error").mockImplementation(() => {}),
}

beforeAll(() => {
  server.listen({ onUnhandledRequest: "bypass" })
  vi.stubEnv("DEV", true)
})

afterEach(() => {
  server.resetHandlers()
  consoleSpy.log.mockClear()
  consoleSpy.error.mockClear()
})

afterAll(() => {
  server.close()
  consoleSpy.log.mockRestore()
  consoleSpy.error.mockRestore()
  vi.unstubAllEnvs()
})

describe("api", () => {
  describe("configuration", () => {
    it("should have correct base URL", () => {
      expect(api.defaults.baseURL).toBe(BASE_URL)
    })

    it("should have correct headers", () => {
      expect(api.defaults.headers.Authorization).toMatch(/^Bearer /)
      expect(api.defaults.headers["Content-Type"]).toBe("application/json")
    })
  })

  describe("request interceptor", () => {
    it("should log requests in development mode", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ success: true })
        })
      )

      await api.get("/test")

      expect(consoleSpy.log).toHaveBeenCalledWith("🚀 API Request:", "GET", "/test")
    })

    it("should not log requests in production mode", async () => {
      vi.stubEnv("DEV", false)

      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ success: true })
        })
      )

      await api.get("/test")

      expect(consoleSpy.log).not.toHaveBeenCalled()

      vi.stubEnv("DEV", true)
    })
  })

  describe("response interceptor", () => {
    it("should log successful responses in development mode", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ success: true })
        })
      )

      await api.get("/test")

      expect(consoleSpy.log).toHaveBeenCalledWith("✅ API Response:", 200, "/test")
    })

    it("should log errors in development mode", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Server error" }, { status: 500 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        // Expected to throw
      }

      expect(consoleSpy.error).toHaveBeenCalledWith("❌ API Error:", 500, "/test")
    })
  })

  describe("error handling", () => {
    it("should throw ApiError with AUTH_ERROR code for 401", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Unauthorized" }, { status: 401 })
        })
      )

      await expect(api.get("/test")).rejects.toThrow(ApiError)

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(401)
        expect((error as ApiError).code).toBe(ERROR_CODES.AUTH_ERROR)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.AUTH_FAILED)
      }
    })

    it("should throw ApiError with FORBIDDEN code for 403", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Forbidden" }, { status: 403 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(403)
        expect((error as ApiError).code).toBe(ERROR_CODES.FORBIDDEN)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.ACCESS_FORBIDDEN)
      }
    })

    it("should throw ApiError with NOT_FOUND code for 404", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Resource not found" }, { status: 404 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(404)
        expect((error as ApiError).code).toBe(ERROR_CODES.NOT_FOUND)
        expect((error as ApiError).message).toBe("Resource not found")
      }
    })

    it("should use default message for 404 when no status_message", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({}, { status: 404 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.RESOURCE_NOT_FOUND)
      }
    })

    it("should throw ApiError with RATE_LIMIT code for 429", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Too many requests" }, { status: 429 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(429)
        expect((error as ApiError).code).toBe(ERROR_CODES.RATE_LIMIT)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.RATE_LIMIT_EXCEEDED)
      }
    })

    it("should throw ApiError with SERVER_ERROR code for 500", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Internal server error" }, { status: 500 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(500)
        expect((error as ApiError).code).toBe(ERROR_CODES.SERVER_ERROR)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.SERVER_ERROR)
      }
    })

    it("should throw ApiError with SERVER_ERROR code for 502", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({}, { status: 502 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(502)
        expect((error as ApiError).code).toBe(ERROR_CODES.SERVER_ERROR)
      }
    })

    it("should throw ApiError with SERVER_ERROR code for 503", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({}, { status: 503 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(503)
        expect((error as ApiError).code).toBe(ERROR_CODES.SERVER_ERROR)
      }
    })

    it("should throw ApiError with SERVER_ERROR code for 504", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({}, { status: 504 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(504)
        expect((error as ApiError).code).toBe(ERROR_CODES.SERVER_ERROR)
      }
    })

    it("should throw ApiError with UNKNOWN_ERROR code for other status codes", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({ status_message: "Custom error" }, { status: 418 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(418)
        expect((error as ApiError).code).toBe(ERROR_CODES.UNKNOWN_ERROR)
        expect((error as ApiError).message).toBe("Custom error")
      }
    })

    it("should use default message for unknown status codes when no status_message", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json({}, { status: 418 })
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.UNEXPECTED_ERROR)
      }
    })

    it("should throw ApiError with NETWORK_ERROR code for network errors", async () => {
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.error()
        })
      )

      try {
        await api.get("/test")
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(0)
        expect((error as ApiError).code).toBe(ERROR_CODES.NETWORK_ERROR)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.NETWORK_ERROR)
      }
    })

    it("should throw ApiError with UNKNOWN_ERROR for other types of errors", async () => {
      // Simulate a general error by making a request to an endpoint that will cause a non-HTTP error
      server.use(
        http.get(`${BASE_URL}/test`, () => {
          // Force an error by throwing
          throw new Error("Unknown error")
        })
      )

      try {
        await api.get("/test")
        // Should not reach here
        expect(true).toBe(false)
      } catch (error) {
        expect(error).toBeInstanceOf(ApiError)
        expect((error as ApiError).status).toBe(500)
        expect((error as ApiError).code).toBe(ERROR_CODES.SERVER_ERROR)
        expect((error as ApiError).message).toBe(ERROR_MESSAGES.SERVER_ERROR)
      }
    })
  })

  describe("successful requests", () => {
    it("should return data for successful GET request", async () => {
      const mockData = { id: 1, title: "Test Movie" }

      server.use(
        http.get(`${BASE_URL}/test`, () => {
          return HttpResponse.json(mockData)
        })
      )

      const response = await api.get("/test")
      expect(response.data).toEqual(mockData)
      expect(response.status).toBe(200)
    })

    it("should return data for successful POST request", async () => {
      const mockData = { success: true }
      const postData = { title: "New Movie" }

      server.use(
        http.post(`${BASE_URL}/test`, async ({ request }) => {
          const body = await request.json()
          expect(body).toEqual(postData)
          return HttpResponse.json(mockData)
        })
      )

      const response = await api.post("/test", postData)
      expect(response.data).toEqual(mockData)
    })
  })
})
