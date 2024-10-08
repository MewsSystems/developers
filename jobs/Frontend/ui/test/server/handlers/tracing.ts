import { http, HttpResponse } from "msw"

export const tracingHandlers = [
  http.post("*/v1/traces", () => {
    return HttpResponse.json({})
  }),
]
