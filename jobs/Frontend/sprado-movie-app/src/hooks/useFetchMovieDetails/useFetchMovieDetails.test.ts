import { renderHook } from "@testing-library/react";
import { waitFor } from "@testing-library/react";

import { useFetchMovieDetails } from "./useFetchMovieDetails";
import type { Movie } from "../../types";

const mockedMovie: Movie = {
  id: 1,
  title: "Inception",
  overview:
    "A thief who steals corporate secrets through dream-sharing technology.",
  vote_average: 8.8,
  poster_path: "/poster-inception.jpg",
  backdrop_path: "/backdrop-inception.jpg",
  release_date: "2010-07-16",
  adult: false,
  genre_ids: [28, 12],
  original_language: "en",
  original_title: "Inception",
  popularity: 40.0,
  vote_count: 25000,
  video: false,
};

const mockedError = new Error("Failed to fetch movie details");

const createMockResponse = (
  data: any,
  ok: boolean = true,
  status: number = 200
): Partial<Response> => ({
  ok,
  status,
  json: jest.fn().mockResolvedValue(data),
});

describe("useFetchMovieDetails", () => {
  const originalFetch = global.fetch;

  beforeAll(() => {
    process.env.REACT_APP_TMDB_API_KEY = "test_api_key";
  });

  beforeEach(() => {
    jest.spyOn(global, "fetch");
  });

  afterAll(() => {
    global.fetch = originalFetch;
  });

  it("returns initial state when no ID is provided", () => {
    const { result } = renderHook(() => useFetchMovieDetails(undefined));

    expect(result.current.movie).toBeNull();
    expect(result.current.isLoading).toBe(false);
    expect(result.current.error).toBeNull();
    expect(global.fetch).not.toHaveBeenCalled();
  });

  it("fetch movie details successfully", async () => {
    const mockFetch = global.fetch as jest.MockedFunction<typeof fetch>;

    mockFetch.mockResolvedValueOnce(
      createMockResponse(mockedMovie) as Response
    );

    const { result } = renderHook(() => useFetchMovieDetails("1"));

    expect(result.current.isLoading).toBe(true);
    expect(result.current.movie).toBeNull();
    expect(result.current.error).toBeNull();
    expect(mockFetch).toHaveBeenCalledWith(
      `https://api.themoviedb.org/3/movie/1?api_key=test_api_key&language=en-US`
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.movie).toEqual(mockedMovie);
    expect(result.current.error).toBeNull();
  });

  it("handles fetch failure with non-OK response", async () => {
    const mockFetch = global.fetch as jest.MockedFunction<typeof fetch>;

    mockFetch.mockResolvedValueOnce(
      createMockResponse({ message: "Not Found" }, false, 404) as Response
    );

    const { result } = renderHook(() => useFetchMovieDetails("999"));

    expect(result.current.isLoading).toBe(true);
    expect(result.current.movie).toBeNull();
    expect(result.current.error).toBeNull();
    expect(mockFetch).toHaveBeenCalledWith(
      `https://api.themoviedb.org/3/movie/999?api_key=test_api_key&language=en-US`
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.isLoading).toBe(false);
    expect(result.current.movie).toBeNull();
    expect(result.current.error).toEqual(
      new Error("Failed to fetch movie details")
    );
  });

  it("handles network error during fetch", async () => {
    const mockFetch = global.fetch as jest.MockedFunction<typeof fetch>;

    mockFetch.mockRejectedValueOnce(mockedError);

    const { result } = renderHook(() => useFetchMovieDetails("1"));

    expect(result.current.isLoading).toBe(true);
    expect(result.current.movie).toBeNull();
    expect(result.current.error).toBeNull();
    expect(mockFetch).toHaveBeenCalledWith(
      `https://api.themoviedb.org/3/movie/1?api_key=test_api_key&language=en-US`
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.isLoading).toBe(false);
    expect(result.current.movie).toBeNull();
    expect(result.current.error).toEqual(mockedError);
  });
});
