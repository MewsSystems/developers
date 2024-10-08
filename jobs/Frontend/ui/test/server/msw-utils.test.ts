import { HttpResponse, HttpResponseResolver, http } from "msw"
import { describe, it, expect, vi } from "vitest"

import { withJsonBody, server } from "./index"

describe("withJsonBody", () => {
  const expectedBody = { foo: "bar" }
  const fakeEndpointUrl = "/api/test"

  async function sendRequest<T>(body: T) {
    return fetch(fakeEndpointUrl, {
      method: "POST",
      body: JSON.stringify(body),
      headers: {
        "Content-Type": "application/json",
      },
    })
  }

  it("should only call the resolver if the request body matches the expected body", async () => {
    const resolver: HttpResponseResolver = vi.fn(() => {
      return HttpResponse.json()
    })

    server.use(http.post(fakeEndpointUrl, withJsonBody(expectedBody, resolver)))

    await sendRequest(expectedBody)

    expect(resolver).toHaveBeenCalledTimes(1)
  })

  it("should not call the resolver if the request body does not match the expected body", async () => {
    const resolver: HttpResponseResolver = vi.fn(() => {
      return HttpResponse.json()
    })

    const unexpectedBody = { foo: "baz" }

    // Avoid console noise as we expect the request to be unhandled.
    server.listen({ onUnhandledRequest: "bypass" })
    server.use(
      http.post(
        "http://localhost:3000/api/test",
        withJsonBody(expectedBody, resolver),
      ),
    )

    let error = undefined

    try {
      await sendRequest(unexpectedBody)
    } catch (err) {
      error = err
    }

    expect(error).toBeDefined()

    expect(resolver).not.toHaveBeenCalled()
  })
})
