import { vi, beforeEach, describe, it, expect } from "vitest"
import { z } from "zod"

import modelFactory from "test/model-factory"

import { withValidation } from ".."

const schema = z.object({
  name: z.string(),
})

const mockFn = vi.fn()

beforeEach(() => {
  mockFn.mockClear()
})

describe("When Request Body is Valid", () => {
  const requestBody = { name: "123" }
  const validRequest = modelFactory.request({
    json: async () => requestBody,
  })
  beforeEach(async () => {
    const validatedFn = withValidation(mockFn, schema)

    await validatedFn(validRequest)
  })
  it("should call the callback function", () => {
    expect(mockFn).toHaveBeenCalledWith(validRequest, requestBody)
  })
})

describe("When Request Body is Invalid", () => {
  let response: Response

  beforeEach(async () => {
    const invalidRequest = modelFactory.request({
      json: async () => ({ name: 123 }), // name should be a string, not a number
    })

    const validatedFn = withValidation(mockFn, schema)

    response = await validatedFn(invalidRequest)
  })

  it("should return a 422 status code", () => {
    expect(response?.status).toBe(422)
  })

  it("should return the validation errors", async () => {
    const expectedResponse = [
      {
        code: "invalid_type",
        expected: "string",
        message: "Expected string, received number",
        path: ["name"],
        received: "number",
      },
    ]
    expect(await response?.json()).toStrictEqual(expectedResponse)
  })

  it("should not call the callback function", () => {
    expect(mockFn).not.toHaveBeenCalled()
  })
})

describe("When an Unexpected Error Occurs", () => {
  let response: Response

  beforeEach(async () => {
    const errorRequest = modelFactory.request({
      json: async () => {
        throw new Error("Unexpected error")
      },
    })

    const validatedFn = withValidation(mockFn, schema)
    response = await validatedFn(errorRequest)
  })

  it("should return a 500 status code", () => {
    expect(response?.status).toBe(500)
  })

  it("should return an error message", async () => {
    expect(await response?.text()).toBe("Unexpected Validation Error")
  })
})
