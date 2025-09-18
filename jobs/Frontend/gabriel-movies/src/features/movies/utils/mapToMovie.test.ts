import { describe, it, expect } from "vitest";
import { mapToMovie } from "./mapToMovie";
import { MOVIE_INTERSTELLAR, TMDB_MOVIE_INTERSTELLAR } from "../fixtures/movies";

describe("mapToMovie", () => {
  it("maps TMDB payload to Movie", () => {
    const result = mapToMovie(TMDB_MOVIE_INTERSTELLAR);

    expect(result).toEqual(MOVIE_INTERSTELLAR);
  });
});
