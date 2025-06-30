import { renderHook } from "@testing-library/react";
import { vi, describe, it, expect, beforeEach, afterEach } from "vitest";
import { waitFor } from "@testing-library/react";
import { useMovieDetails } from "../useMovieDetailsQuery";
import { QueryClientProvider } from "@tanstack/react-query";
import * as api from "../../api/requests";
import { createTestQueryClient } from "../../test-utils/wrappers";
import { movieDetails } from "../../mocks/data";

const wrapper = ({ children }: any) => (
  <QueryClientProvider client={createTestQueryClient()}>
    {children}
  </QueryClientProvider>
);

describe("useMovieDetails", () => {
  beforeEach(() => {
    vi.spyOn(api, "getMovieDetails").mockImplementation(() =>
      Promise.resolve(movieDetails)
    );
  });

  afterEach(() => vi.restoreAllMocks());

  it("fetches data when id > 0", async () => {
    const { result } = renderHook(() => useMovieDetails(1), {
      wrapper,
    });
    expect(result.current.isLoading).toBe(true);
    await waitFor(() => result.current.isSuccess);
    expect(result.current.data).toEqual(movieDetails);
    expect(api.getMovieDetails).toHaveBeenCalledWith(1);
  });
});
