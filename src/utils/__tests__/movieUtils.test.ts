import { describe, expect, it } from "vitest"
import { formatNumber, formatRuntime, getImageUrl, getYear } from "../movieUtils"

describe("movieUtils", () => {
  describe("getImageUrl", () => {
    it("should return full TMDB image URL when path is provided", () => {
      const path = "/example.jpg"
      const result = getImageUrl(path)
      expect(result).toBe("https://image.tmdb.org/t/p/w500/example.jpg")
    })

    it("should return full TMDB image URL with custom size", () => {
      const path = "/example.jpg"
      const size = "w300"
      const result = getImageUrl(path, size)
      expect(result).toBe("https://image.tmdb.org/t/p/w300/example.jpg")
    })

    it("should return null when path is null", () => {
      const result = getImageUrl(null)
      expect(result).toBeNull()
    })

    it("should return null when path is empty string", () => {
      const result = getImageUrl("")
      expect(result).toBeNull()
    })
  })

  describe("getYear", () => {
    it("should extract year from release date string", () => {
      const releaseDate = "2023-05-15"
      const result = getYear(releaseDate)
      expect(result).toBe(2023)
    })

    it("should handle different date formats", () => {
      const releaseDate = "2020-12-31"
      const result = getYear(releaseDate)
      expect(result).toBe(2020)
    })

    it("should handle edge case years", () => {
      const releaseDate = "1999-01-01"
      const result = getYear(releaseDate)
      expect(result).toBe(1999)
    })
  })

  describe("formatRuntime", () => {
    it("should format runtime correctly for hours and minutes", () => {
      const minutes = 150
      const result = formatRuntime(minutes)
      expect(result).toBe("2h 30m")
    })

    it("should handle exact hours", () => {
      const minutes = 120
      const result = formatRuntime(minutes)
      expect(result).toBe("2h 0m")
    })

    it("should handle less than an hour", () => {
      const minutes = 45
      const result = formatRuntime(minutes)
      expect(result).toBe("0h 45m")
    })

    it("should handle long runtimes", () => {
      const minutes = 200
      const result = formatRuntime(minutes)
      expect(result).toBe("3h 20m")
    })

    it("should handle zero minutes", () => {
      const minutes = 0
      const result = formatRuntime(minutes)
      expect(result).toBe("0h 0m")
    })
  })

  describe("formatNumber", () => {
    it("should format numbers with default locale (en-US)", () => {
      const number = 1234567
      const result = formatNumber(number)
      expect(result).toBe("1,234,567")
    })

    it("should format small numbers", () => {
      const number = 123
      const result = formatNumber(number)
      expect(result).toBe("123")
    })

    it("should format numbers with thousands separator", () => {
      const number = 1000
      const result = formatNumber(number)
      expect(result).toBe("1,000")
    })

    it("should handle zero", () => {
      const number = 0
      const result = formatNumber(number)
      expect(result).toBe("0")
    })

    it("should format with custom locale", () => {
      const number = 1234567
      const result = formatNumber(number, "de-DE")
      expect(result).toBe("1.234.567")
    })
  })
})
