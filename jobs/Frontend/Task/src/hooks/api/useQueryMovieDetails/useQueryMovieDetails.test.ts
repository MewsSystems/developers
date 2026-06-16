import { renderHook, waitFor } from "@testing-library/react";
import { Wrapper } from "../../../mocks/Wrapper";
import { movieDetailsMockData } from "../../../mocks/data/movieDetails";
import { useQueryMovieDetails } from "./useQueryMovieDetails";

describe("useQueryMovieDetails", () => {
  it("should return the correct data and loading state", async () => {
    const { result } = renderHook(() => useQueryMovieDetails({ 
      id: 1
    }), { wrapper: Wrapper });

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false);
    });

    expect(result.current.data).toEqual(movieDetailsMockData);
  });
});
