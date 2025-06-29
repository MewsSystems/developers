import { renderHook, act, waitFor } from "@testing-library/react";
import { usePopularMoviesQuery } from "../useMoviesQueries";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import * as api from "../../api/requests";
import { beforeEach, afterEach, vi, describe, it, expect } from "vitest";

function createClient() {
  return new QueryClient({ defaultOptions: { queries: { retry: false } } });
}

const wrapper = ({ children }: any) => (
  <QueryClientProvider client={createClient()}>{children}</QueryClientProvider>
);

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
const page2 = {
  page: 2,
  total_pages: 2,
  total_results: 2,
  results: [
    {
      id: 2,
      title: "B",
      adult: false,
      backdrop_path: null,
      genre_ids: [],
      original_language: "en",
      original_title: "B",
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
  vi.spyOn(api, "getPopularMovies").mockImplementation((page = 1) =>
    Promise.resolve(page === 1 ? page1 : page2)
  );
});

afterEach(() => vi.restoreAllMocks());

describe("usePopularMoviesQuery", () => {
  it("loads first and next page", async () => {
    const { result } = renderHook(() => usePopularMoviesQuery(), {
      wrapper,
    });
    await waitFor(() => result.current.isSuccess);

    await waitFor(() => {
      expect(result.current.data?.pages[0]).toEqual(page1);
    });

    await act(async () => result.current.fetchNextPage());

    await waitFor(() => {
      expect(result.current.data?.pages[1]).toEqual(page2);
    });
    expect(result.current.hasNextPage).toBe(true);
  });
});
