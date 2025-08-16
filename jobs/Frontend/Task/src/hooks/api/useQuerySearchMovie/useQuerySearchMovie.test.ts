import { renderHook, waitFor } from "@testing-library/react";
import { Wrapper } from "../../../mocks/Wrapper";
import { searchMovieMockData } from "../../../mocks/data/searchMovie";
import { useQuerySearchMovie } from "./useQuerySearchMovie";

describe("useQuerySearchMovie", () => {
  it("should return the correct data and loading state", async () => {
    const { result } = renderHook(() => useQuerySearchMovie({ 
      query: "test",
      page: 1
    }), { wrapper: Wrapper });

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false);
    });

    expect(result.current.data).toEqual(searchMovieMockData);
  });
});
