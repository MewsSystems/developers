import { expect, test } from "vitest"
import { getMovieTitle } from "./get-movie-title"

test("getMovieTitle", () => {
  expect(getMovieTitle(undefined)).toBe("Unknown")
  expect(getMovieTitle("The Matrix")).toBe("The Matrix")
})
