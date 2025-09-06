import { renderHook, act, waitFor } from "@testing-library/react";
import { vi } from "vitest";
import useMovieSearch from "./useMovieSearch";
import { fetchMovies } from "../api/tmdb";

// Use vi.mocked for type-safe access to the mocked function
const mockFetchMovies = vi.mocked(fetchMovies);

// Mock sessionStorage
const mockSessionStorage = {
  getItem: vi.fn(),
  setItem: vi.fn(),
  removeItem: vi.fn(),
  clear: vi.fn(),
};

Object.defineProperty(window, "sessionStorage", {
  value: mockSessionStorage,
});

describe("useMovieSearch", () => {
  const mockMoviesResponse = {
    results: [
      { id: 1, title: "Avengers", poster_path: "/avengers.jpg" },
      { id: 2, title: "Spider-Man", poster_path: "/spiderman.jpg" },
    ],
  };

  beforeEach(() => {
    vi.clearAllMocks();
    mockSessionStorage.getItem.mockReturnValue(null);
    mockFetchMovies.mockResolvedValue(mockMoviesResponse);
  });

  it("initializes with empty state when no sessionStorage data", () => {
    const { result } = renderHook(() => useMovieSearch());

    expect(result.current.query).toBe("");
    expect(result.current.movies).toEqual([]);
    expect(result.current.page).toBe(1);
    expect(result.current.loading).toBe(false);
    expect(result.current.error).toBe(null);
  });

  it("initializes with data from sessionStorage", () => {
    mockSessionStorage.getItem
      .mockReturnValueOnce("batman") // searchQuery
      .mockReturnValueOnce('[{"id":1,"title":"Batman"}]') // searchMovies
      .mockReturnValueOnce("2"); // searchPage

    const { result } = renderHook(() => useMovieSearch());

    expect(result.current.query).toBe("batman");
    expect(result.current.movies).toEqual([{ id: 1, title: "Batman" }]);
    expect(result.current.page).toBe(2);
  });

  it("updates query state", () => {
    const { result } = renderHook(() => useMovieSearch());

    act(() => {
      result.current.setQuery("superman");
    });

    expect(result.current.query).toBe("superman");
  });

  it("persists state to sessionStorage when state changes", async () => {
    const { result } = renderHook(() => useMovieSearch());

    act(() => {
      result.current.setQuery("batman");
    });

    await waitFor(() => {
      expect(mockSessionStorage.setItem).toHaveBeenCalledWith(
        "searchQuery",
        "batman",
      );
      expect(mockSessionStorage.setItem).toHaveBeenCalledWith(
        "searchMovies",
        "[]",
      );
      expect(mockSessionStorage.setItem).toHaveBeenCalledWith(
        "searchPage",
        "1",
      );
    });
  });

  it("performs search and updates movies on page 1", async () => {
    const { result } = renderHook(() => useMovieSearch());

    await act(async () => {
      await result.current.search("avengers");
    });

    expect(mockFetchMovies).toHaveBeenCalledWith("avengers", 1);
    expect(result.current.movies).toEqual(mockMoviesResponse.results);
    expect(result.current.page).toBe(1);
    expect(result.current.loading).toBe(false);
  });

  it("appends movies when searching subsequent pages", async () => {
    const { result } = renderHook(() => useMovieSearch());

    // First search
    await act(async () => {
      await result.current.search("avengers");
    });

    // Second page search
    const newMovies = {
      results: [{ id: 3, title: "Iron Man", poster_path: "/ironman.jpg" }],
    };
    mockFetchMovies.mockResolvedValueOnce(newMovies);

    await act(async () => {
      await result.current.search("avengers", 2);
    });

    expect(result.current.movies).toEqual([
      ...mockMoviesResponse.results,
      ...newMovies.results,
    ]);
    expect(result.current.page).toBe(2);
  });

  it("replaces movies when starting new search (page 1)", async () => {
    const { result } = renderHook(() => useMovieSearch());

    // First search
    await act(async () => {
      await result.current.search("avengers");
    });

    // New search with different term
    const newMovies = {
      results: [{ id: 10, title: "Batman", poster_path: "/batman.jpg" }],
    };
    mockFetchMovies.mockResolvedValueOnce(newMovies);

    await act(async () => {
      await result.current.search("batman", 1);
    });

    expect(result.current.movies).toEqual(newMovies.results);
    expect(result.current.page).toBe(1);
  });

  it("sets loading state during search", async () => {
    let resolvePromise;
    const searchPromise = new Promise((resolve) => {
      resolvePromise = resolve;
    });
    mockFetchMovies.mockReturnValueOnce(searchPromise);

    const { result } = renderHook(() => useMovieSearch());

    act(() => {
      result.current.search("avengers");
    });

    expect(result.current.loading).toBe(true);

    await act(async () => {
      resolvePromise(mockMoviesResponse);
      await searchPromise;
    });

    expect(result.current.loading).toBe(false);
  });

  it("handles search errors", async () => {
    const consoleSpy = vi.spyOn(console, "error").mockImplementation(() => {});
    mockFetchMovies.mockRejectedValueOnce(new Error("API Error"));

    const { result } = renderHook(() => useMovieSearch());

    await act(async () => {
      await result.current.search("avengers");
    });

    expect(result.current.error).toBe(
      "Something went wrong. Please try again.",
    );
    expect(result.current.loading).toBe(false);
    expect(consoleSpy).toHaveBeenCalled();

    consoleSpy.mockRestore();
  });

  it("does not search with empty term", async () => {
    const { result } = renderHook(() => useMovieSearch());

    await act(async () => {
      await result.current.search("");
    });

    expect(mockFetchMovies).not.toHaveBeenCalled();
    expect(result.current.movies).toEqual([]);
  });

  it("clears error on new search", async () => {
    const { result } = renderHook(() => useMovieSearch());

    // Set initial error
    mockFetchMovies.mockRejectedValueOnce(new Error("API Error"));
    await act(async () => {
      await result.current.search("avengers");
    });
    expect(result.current.error).toBe(
      "Something went wrong. Please try again.",
    );

    // New search should clear error
    mockFetchMovies.mockResolvedValueOnce(mockMoviesResponse);
    await act(async () => {
      await result.current.search("batman");
    });

    expect(result.current.error).toBe(null);
  });

  it("resets all state", async () => {
    const { result } = renderHook(() => useMovieSearch());

    // Set some initial state
    act(() => {
      result.current.setQuery("batman");
    });
    await act(async () => {
      await result.current.search("batman");
    });

    // Reset
    act(() => {
      result.current.reset();
    });

    expect(result.current.query).toBe("");
    expect(result.current.movies).toEqual([]);
    expect(result.current.page).toBe(1);
  });

  it("uses default page 1 when nextPage is not provided", async () => {
    const { result } = renderHook(() => useMovieSearch());

    await act(async () => {
      await result.current.search("avengers");
    });

    expect(mockFetchMovies).toHaveBeenCalledWith("avengers", 1);
  });
});
