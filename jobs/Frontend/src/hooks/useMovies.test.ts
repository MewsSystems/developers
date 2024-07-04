import { renderHook, act, waitFor } from "@testing-library/react";
import fetchMock from "fetch-mock";
import type { MovieResponse, MovieDetails } from "../api";
import { useMovies } from "./useMovies";

const MockedMovieResponse: DeepPartial<MovieResponse> = {
  results: [
    { id: 1, title: "Movie 1" },
    { id: 2, title: "Movie 2" },
  ],
};

const MockedMovieDetailResponse: DeepPartial<MovieDetails> = {
  id: 1,
  title: "Movie 1",
};

fetchMock.get(
  "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&query=success&page=1",
  MockedMovieResponse
);

fetchMock.get(
  "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&query=failure500&page=1",
  500
);

fetchMock.get(
  "https://api.themoviedb.org/3/movie/1?api_key=03b8572954325680265531140190fd2a",
  MockedMovieDetailResponse
);

fetchMock.get(
  "https://api.themoviedb.org/3/movie/failure500?api_key=03b8572954325680265531140190fd2a",
  500
);

describe("useMovies", () => {
  beforeEach(() => {
    vi.resetAllMocks();
  });

  it("should fetch movies and update state correctly", async () => {
    const { result } = renderHook(() => useMovies());

    console.log("result", result);
    act(() => {
      result.current.searchMovies("success", 1);
    });

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.movies).toEqual([
      { id: 1, title: "Movie 1" },
      { id: 2, title: "Movie 2" },
    ]);
    expect(result.current.loading).toBe(false);
    expect(result.current.error).toBe(null);
  });

  it("should handle error when fetching movies", async () => {
    const { result } = renderHook(() => useMovies());

    act(() => {
      result.current.searchMovies("failure500", 1);
    });

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.movies).toEqual([]);
    expect(result.current.loading).toBe(false);
    expect(result.current.error).toBe(
      "Impossible to fetch movies. Status code 500"
    );
  });

  it("should fetch movie detail and update state correctly", async () => {
    const { result } = renderHook(() => useMovies());

    act(() => {
      result.current.getMovieDetail("1");
    });

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.movieDetail).toEqual({ id: 1, title: "Movie 1" });
    expect(result.current.loading).toBe(false);
    expect(result.current.error).toBe(null);
  });

  it("should handle error when fetching movie details", async () => {
    const { result } = renderHook(() => useMovies());

    act(() => {
      result.current.getMovieDetail("failure500");
    });

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.movieDetail).toBe(null);
    expect(result.current.loading).toBe(false);
    expect(result.current.error).toBe(
      "Impossible to fetch movie details. Status code 500"
    );
  });

    it("should reset movies state", async () => {
      const { result } = renderHook(() => useMovies());

      act(() => {
        result.current.searchMovies("success", 1);
      });

      expect(result.current.loading).toBe(true);

      await waitFor(() => {
        expect(result.current.loading).toBe(false);
      });

      act(() => {
        result.current.resetMovies();
      });

      expect(result.current.movies).toEqual([]);
    });
});
