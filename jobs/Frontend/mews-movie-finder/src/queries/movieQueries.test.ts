import { describe, expect, it } from "vitest";
import { getMovieDetails, getMovies } from "./movieQueries";

describe("movie queries", () => {
  it("fetches data", async () => {
    const result = await getMovies("lord", 1);

    expect(result.total_results).toBeGreaterThan(0);
  })

  it("throws error", async () => {
    const result = getMovies("error", 1);

    expect(result).rejects.toThrowError();
  })
});

describe("movie detail queries", () => {
  it("fetches data", async () => {
    const result = await getMovieDetails(120);

    expect(result).not.toBeUndefined();
  })

  it("throws error", async () => {
    const result = getMovieDetails(42);

    expect(result).rejects.toThrowError();
  })
});
