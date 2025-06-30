import { renderHook, act, waitFor } from "@testing-library/react";
import { usePopularMoviesQuery } from "../useMoviesQueries";
import { QueryClientProvider } from "@tanstack/react-query";
import * as api from "../../api/requests";
import { beforeEach, afterEach, vi, describe, it, expect } from "vitest";
import { createTestQueryClient } from "../../test-utils/wrappers";
import { moviesPage1, moviesPage2 } from "../../mocks/data";

const wrapper = ({ children }: any) => (
  <QueryClientProvider client={createTestQueryClient()}>
    {children}
  </QueryClientProvider>
);

beforeEach(() => {
  vi.spyOn(api, "getPopularMovies").mockImplementation((page = 1) =>
    Promise.resolve(page === 1 ? moviesPage1 : moviesPage2)
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
      expect(result.current.data?.pages[0]).toEqual(moviesPage1);
    });

    await act(async () => result.current.fetchNextPage());

    await waitFor(() => {
      expect(result.current.data?.pages[1]).toEqual(moviesPage2);
    });
    expect(result.current.hasNextPage).toBe(true);
  });
});
