import { SpanStatusCode, trace, Span } from "@opentelemetry/api"
import { vi, describe, expect, it, beforeEach } from "vitest"
import { z } from "zod"

import modelFactory from "test/model-factory"

import { withValidation } from ".."

vi.mock("@opentelemetry/api")

const testSchema = z.object({
  name: z.string(),
})

const mockCallback = vi.fn()

const mockSpan: Partial<Span> = {
  setStatus: vi.fn(),
  addEvent: vi.fn(),
  recordException: vi.fn(),
  end: vi.fn(),
}

beforeEach(() => {
  trace.getTracer = vi.fn().mockReturnValue({
    startActiveSpan: vi
      .fn()
      .mockImplementation((_, callback) => callback(mockSpan)),
  })
})

describe("When Validation Succeeds", () => {
  beforeEach(async () => {
    const validRequest = modelFactory.request({
      json: async () => ({ name: "123" }),
    })

    const handler = withValidation(mockCallback, testSchema)
    await handler(validRequest)
  })

  it("should set span status to OK", () => {
    expect(mockSpan.setStatus).toHaveBeenCalledWith({
      code: SpanStatusCode.OK,
    })
  })

  it("should end the span once the validation is complete", () => {
    expect(mockSpan.end).toHaveBeenCalled()
  })
})

describe("When Validation Fails", () => {
  beforeEach(async () => {
    const invalidRequest = modelFactory.request({
      json: async () => ({ name: 123 }),
    })

    const handler = withValidation(mockCallback, testSchema)
    await handler(invalidRequest)
  })

  it("should set span status to ERROR", () => {
    expect(mockSpan.setStatus).toHaveBeenCalledWith({
      code: SpanStatusCode.ERROR,
      message: "Unexpected error while validating request body",
    })
  })

  it("should add event with the validation errors", () => {
    const expectedAttributes = {
      issues: [
        '{"code":"invalid_type","expected":"string","received":"number","path":["name"],"message":"Expected string, received number"}',
      ],
    }
    expect(mockSpan.addEvent).toHaveBeenCalledWith(
      "Validation Errors",
      expectedAttributes,
    )
  })

  it("should end the span once the validation is complete", () => {
    expect(mockSpan.end).toHaveBeenCalled()
  })
})

describe("When an Unexpected Error Occurs", () => {
  beforeEach(async () => {
    const errorRequest = modelFactory.request({
      json: async () => {
        throw new Error("Unexpected error")
      },
    })

    const handler = withValidation(mockCallback, testSchema)
    await handler(errorRequest)
  })

  it("should set span status to ERROR", () => {
    expect(mockSpan.setStatus).toHaveBeenCalledWith({
      code: SpanStatusCode.ERROR,
      message: "Unexpected error while validating request body",
    })
  })

  it("should record the exception", () => {
    expect(mockSpan.recordException).toHaveBeenCalledWith(
      new Error("Error: Unexpected error"),
    )
  })

  it("should end the span once the validation is complete", () => {
    expect(mockSpan.end).toHaveBeenCalled()
  })
})
