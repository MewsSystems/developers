import { renderHook } from "@testing-library/react";
import { useFetchMovies } from "./useFetchMovies";
import { Movie } from "../../types";
import { waitFor } from "@testing-library/react";

const createMockResponse = (
  data: any,
  ok: boolean = true,
  status: number = 200
): Partial<Response> => ({
  ok,
  status,
  json: jest.fn().mockResolvedValue(data),
});

const mockedMovies: Movie[] = [
  {
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
  },
  {
    id: 2,
    title: "The Matrix",
    overview: "A computer hacker learns about the true nature of reality.",
    vote_average: 8.7,
    poster_path: "/poster-matrix.jpg",
    backdrop_path: "/backdrop-matrix.jpg",
    release_date: "1999-03-31",
    adult: false,
    genre_ids: [28, 12],
    original_language: "en",
    original_title: "Inception",
    popularity: 40.0,
    vote_count: 25000,
    video: false,
  },
];

const mockedError = new Error("Failed to fetch movies");

describe("useFetchMovies Hook", () => {
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

  const totalPagesMock = 5;

  it("returns initial state when no search term is provided", () => {
    const baseURL =
      "https://api.themoviedb.org/3/search/movie?api_key=test_api_key&language=en-US";

    const { result } = renderHook(() => useFetchMovies(baseURL, "", 1));

    expect(result.current.data).toEqual([]);
    expect(result.current.totalPages).toBe(0);
    expect(result.current.isLoading).toBe(false);
    expect(result.current.error).toBeNull();
    expect(global.fetch).not.toHaveBeenCalled();
  });

  it("fetch movies successfully", async () => {
    const baseURL =
      "https://api.themoviedb.org/3/search/movie?api_key=test_api_key&language=en-US";
    const search = "Inception";
    const page = 1;

    const mockFetch = global.fetch as jest.MockedFunction<typeof fetch>;

    mockFetch.mockResolvedValueOnce(
      createMockResponse({
        results: mockedMovies,
        total_pages: totalPagesMock,
      }) as Response
    );

    const { result } = renderHook(() => useFetchMovies(baseURL, search, page));

    expect(result.current.isLoading).toBe(true);
    expect(result.current.data).toEqual([]);
    expect(result.current.totalPages).toBe(0);
    expect(result.current.error).toBeNull();
    expect(mockFetch).toHaveBeenCalledWith(
      `${baseURL}&query=${encodeURIComponent(search)}&page=${page}`
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.isLoading).toBe(false);
    expect(result.current.data).toEqual(mockedMovies);
    expect(result.current.totalPages).toBe(totalPagesMock);
    expect(result.current.error).toBeNull();
  });

  it("handles fetch failure with non-OK response", async () => {
    const baseURL =
      "https://api.themoviedb.org/3/search/movie?api_key=test_api_key&language=en-US";
    const search = "UnknownMovie";
    const page = 1;

    const mockFetch = global.fetch as jest.MockedFunction<typeof fetch>;

    mockFetch.mockResolvedValueOnce(
      createMockResponse({ message: "Not Found" }, false, 404) as Response
    );

    const { result } = renderHook(() => useFetchMovies(baseURL, search, page));

    expect(result.current.isLoading).toBe(true);
    expect(result.current.data).toEqual([]);
    expect(result.current.totalPages).toBe(0);
    expect(result.current.error).toBeNull();
    expect(mockFetch).toHaveBeenCalledWith(
      `${baseURL}&query=${encodeURIComponent(search)}&page=${page}`
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.isLoading).toBe(false);
    expect(result.current.data).toEqual([]);
    expect(result.current.totalPages).toBe(0);
    expect(result.current.error).toEqual(new Error("Failed to fetch movies"));
  });

  it("handles network errors during fetch", async () => {
    const baseURL = "https://api.themoviedb.org/3/search/movie?api_key=test_api_key&language=en-US";
    const search = "Interstellar";
    const page = 1;
  
    const mockFetch = global.fetch as jest.MockedFunction<typeof fetch>;
  
    mockFetch.mockRejectedValueOnce(new Error("Network Error"));
  
    const { result } = renderHook(() => useFetchMovies(baseURL, search, page));
  
    expect(result.current.isLoading).toBe(true);
    expect(result.current.data).toEqual([]);
    expect(result.current.totalPages).toBe(0);
    expect(result.current.error).toBeNull();
    expect(mockFetch).toHaveBeenCalledWith(
      `${baseURL}&query=${encodeURIComponent(search)}&page=${page}`
    );
  
    await waitFor(() => expect(result.current.isLoading).toBe(false));
  
    expect(result.current.isLoading).toBe(false);
    expect(result.current.data).toEqual([]);
    expect(result.current.totalPages).toBe(0);
    expect(result.current.error).toEqual(new Error("Network Error"));
  });
  
});
