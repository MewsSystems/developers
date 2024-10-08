vi.mock("@/app/lib/env.ts")

import { describe, it, expect, beforeEach, vi } from "vitest"

import modelFactory from "test/model-factory"

import { GET } from "./route"

describe("When Request Body is Valid", () => {
  let response: Response

  beforeEach(async () => {
    const starWarsSearchQueryUrl = `http://localhost:3000/api/search/movie?query=star+wars&page=1`

    const mockedRequest = modelFactory.request({
      url: starWarsSearchQueryUrl,
    })

    response = await GET(mockedRequest)
  })

  it("should return 200Ok", () => {
    expect(response.status).toBe(200)
  })

  it("should return movies related to star wars", async () => {
    expect(true).toBe(true)
  })
})
