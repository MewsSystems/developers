import type { ConfigurationDetails } from "~/utils/get-configuration-details.server"
import { describe, expect, test } from "vitest"
import { getPosterImageSource } from "~/utils/get-poster-image-source.server"

describe("getPosterImageSource", () => {
  const configurationDetails: ConfigurationDetails = {
    images: {
      secure_base_url: "https://image.tmdb.org/t/p/",
      backdrop_sizes: ["w300", "w780", "w1280", "original"],
      poster_sizes: ["w92", "w154", "w185", "w342", "w500", "w780", "original"],
    },
  }

  test("returns null when imagePath is null", () => {
    expect(
      getPosterImageSource({
        size: 0,
        imagePath: null,
        configurationDetails,
      })
    ).toBe(null)
  })

  test("returns the poster image source", () => {
    expect(
      getPosterImageSource({
        size: 0,
        imagePath: "/abc.jpg",
        configurationDetails,
      })
    ).toBe("https://image.tmdb.org/t/p/w92/abc.jpg")
  })
})
