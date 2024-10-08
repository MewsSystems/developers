import { trace, SpanStatusCode } from "@opentelemetry/api"
import { z } from "zod"

export function withValidation<T extends z.ZodTypeAny>(
  callBack: (request: Request, validatedBody: z.infer<T>) => Promise<Response>,
  schema: T,
) {
  return async (request: Request) => {
    return await trace
      .getTracer("example-app")
      .startActiveSpan("validateRequestBody", async (span) => {
        try {
          const body = await request.clone().json()

          const validatedBody = await schema.parseAsync(body)

          span.setStatus({ code: SpanStatusCode.OK })

          return callBack(request, validatedBody)
        } catch (error) {
          span.setStatus({
            code: SpanStatusCode.ERROR,
            message: "Unexpected error while validating request body",
          })

          if (error instanceof z.ZodError) {
            span.addEvent("Validation Errors", {
              issues: error.errors.map((e) => JSON.stringify(e)),
            })

            return Response.json(error.errors, { status: 422 })
          }

          span.recordException(new Error(String(error), { cause: error }))

          return new Response("Unexpected Validation Error", { status: 500 })
        } finally {
          span.end()
        }
      })
  }
}
