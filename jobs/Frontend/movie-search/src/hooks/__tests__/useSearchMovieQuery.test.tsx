import { renderHook } from "@testing-library/react";
import { useSearchMoviesQuery } from "../useSearchMoviesQuery";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import * as api from "../../api/requests";
import { vi, describe, it, expect, beforeEach, afterEach } from "vitest";
import { waitFor } from "@testing-library/react";

function createClient() {
  return new QueryClient({ defaultOptions: { queries: { retry: false } } });
}

const wrapper = ({ children }: any) => (
  <QueryClientProvider client={createClient()}>{children}</QueryClientProvider>
);

describe("useSearchMoviesQuery", () => {
  const page1 = {
    page: 1,
    total_pages: 2,
    total_results: 2,
    results: [
      {
        id: 1,
        title: "A",
        adult: false,
        backdrop_path: null,
        genre_ids: [],
        original_language: "en",
        original_title: "A",
        overview: "",
        poster_path: null,
        popularity: 0,
        release_date: "2023-01-01",
        video: false,
        vote_average: 0,
        vote_count: 0,
      },
    ],
  };

  beforeEach(() => {
    vi.spyOn(api, "getSearchMovies").mockImplementation(() =>
      Promise.resolve(page1)
    );
  });

  afterEach(() => vi.restoreAllMocks());

  it("fetches when term provided", async () => {
    const { result } = renderHook(() => useSearchMoviesQuery("foo"), {
      wrapper,
    });
    expect(result.current.isLoading).toBe(true);
    await waitFor(() => result.current.isSuccess);
    await waitFor(() => {
      expect(result.current.data?.pages[0]).toEqual(page1);
    });
    expect(api.getSearchMovies).toHaveBeenCalledWith("foo", 1);
  });
});
