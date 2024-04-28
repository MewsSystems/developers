import { describe, expect, test } from "vitest"
import { getMovieTitle } from "./get-movie-title"

describe("getMovieTitle", () => {
  test("returns given title", () => {
    expect(getMovieTitle("The Matrix")).toBe("The Matrix")
  })

  test("returns placeholder if title is undefined", () => {
    expect(getMovieTitle(undefined)).toBe("Unknown Movie")
  })
})
