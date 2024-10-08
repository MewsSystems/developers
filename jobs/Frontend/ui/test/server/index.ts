import { HttpResponseResolver } from "msw"
import { setupServer } from "msw/node"

import { handlers } from "./handlers"

export const server = setupServer(...handlers)

function isEqual(a: unknown, b: unknown) {
  return JSON.stringify(a) === JSON.stringify(b)
}

export function withJsonBody<TExpectedBody>(
  expectedBody: TExpectedBody,
  resolver: HttpResponseResolver,
): HttpResponseResolver {
  return async (args) => {
    const { request } = args

    const contentType = request.headers.get("Content-Type") || ""
    if (!contentType.includes("application/json")) {
      return
    }

    const actualBody = await request.clone().json()

    if (!isEqual(actualBody, expectedBody)) {
      return
    }

    return resolver(args)
  }
}
