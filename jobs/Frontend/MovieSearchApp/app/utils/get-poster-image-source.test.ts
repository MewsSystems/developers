import { expect, test, describe } from "vitest"
import { getPosterImageSource } from "./get-poster-image-source"

describe("getPosterImageSource", () => {
  test("returns poster image resource route", () => {
    expect(getPosterImageSource("/path.jpg", "w92")).toBe(
      "resources/poster-image/w92/path.jpg"
    )
  })
})
