import { describe, it, expect } from "vitest";
import {
  getImageUrl,
  getYearFromDate,
  getTranslatedTitle,
  getMovieDetailRoute,
  formatRuntime,
} from "../movieHelpers";

describe("movieHelpers", () => {
  describe("getImageUrl", () => {
    it("returns correct image URL with default size", () => {
      const result = getImageUrl("/test-poster.jpg");
      expect(result).toBe("https://image.tmdb.org/t/p/w500/test-poster.jpg");
    });

    it("returns correct image URL with custom size", () => {
      const result = getImageUrl("/test-poster.jpg", "w300");
      expect(result).toBe("https://image.tmdb.org/t/p/w300/test-poster.jpg");
    });

    it("returns fallback message when image URL is null", () => {
      const result = getImageUrl(null);
      expect(result).toBe("Image does not exist");
    });
  });

  describe("getYearFromDate", () => {
    it("extracts year from date string", () => {
      const result = getYearFromDate("2023-01-15");
      expect(result).toBe("2023");
    });
  });

  describe("getTranslatedTitle", () => {
    it("returns original title for English movies", () => {
      const result = getTranslatedTitle(true, "The Matrix", "The Matrix");
      expect(result).toBe("The Matrix");
    });

    it("returns translated title for non-English movies", () => {
      const result = getTranslatedTitle(false, "La La Land", "La La Land");
      expect(result).toBe("La La Land (La La Land)");
    });
  });

  describe("getMovieDetailRouter", () => {
    it("returns correct movie detail route", () => {
      const result = getMovieDetailRoute(123);
      expect(result).toBe("/movie/123");
    });
  });

  describe("formatRuntime", () => {
    it("formats runtime correctly", () => {
      const result = formatRuntime(125);
      expect(result).toBe("2h 05m");
    });

    it("returns N/A for null runtime", () => {
      const result = formatRuntime(null);
      expect(result).toBe("N/A");
    });

    it("returns N/A for zero runtime", () => {
      const result = formatRuntime(0);
      expect(result).toBe("N/A");
    });

    it("returns N/A for negative runtime", () => {
      const result = formatRuntime(-10);
      expect(result).toBe("N/A");
    });

    it("handles single digit minutes", () => {
      const result = formatRuntime(61);
      expect(result).toBe("1h 01m");
    });
  });
});
