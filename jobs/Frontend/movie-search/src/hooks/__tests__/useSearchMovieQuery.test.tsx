import { renderHook } from "@testing-library/react";
import { useSearchMoviesQuery } from "../useSearchMoviesQuery";
import { QueryClientProvider } from "@tanstack/react-query";
import * as api from "../../api/requests";
import { vi, describe, it, expect, beforeEach, afterEach } from "vitest";
import { waitFor } from "@testing-library/react";
import { createTestQueryClient } from "../../test-utils/wrappers";
import { moviesPage1 } from "../../mocks/data";

const wrapper = ({ children }: any) => (
  <QueryClientProvider client={createTestQueryClient()}>
    {children}
  </QueryClientProvider>
);

describe("useSearchMoviesQuery", () => {
  beforeEach(() => {
    vi.spyOn(api, "getSearchMovies").mockImplementation(() =>
      Promise.resolve(moviesPage1)
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
      expect(result.current.data?.pages[0]).toEqual(moviesPage1);
    });
    expect(api.getSearchMovies).toHaveBeenCalledWith("foo", 1);
  });
});
