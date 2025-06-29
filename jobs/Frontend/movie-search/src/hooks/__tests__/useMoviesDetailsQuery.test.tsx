import { renderHook } from "@testing-library/react";
import { vi, describe, it, expect, beforeEach, afterEach } from "vitest";
import { waitFor } from "@testing-library/react";
import { useMovieDetails } from "../useMovieDetailsQuery";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import * as api from "../../api/requests";

function createClient() {
  return new QueryClient({ defaultOptions: { queries: { retry: false } } });
}

const wrapper = ({ children }: any) => (
  <QueryClientProvider client={createClient()}>{children}</QueryClientProvider>
);

describe("useMovieDetails", () => {
  const mockData = {
    adult: false,
    backdrop_path: null,
    belongs_to_collection: null,
    budget: 1000000,
    genres: [{ id: 1, name: "Action" }],
    homepage: null,
    id: 1,
    imdb_id: "tt1234567",
    original_language: "en",
    original_title: "Test Movie",
    overview: "A test movie overview",
    popularity: 100,
    poster_path: null,
    production_companies: [
      {
        id: 1,
        logo_path: null,
        name: "Test Studio",
        origin_country: "US",
      },
    ],
    production_countries: [{ iso_3166_1: "US", name: "United States" }],
    release_date: "2023-01-01",
    revenue: 5000000,
    runtime: 120,
    spoken_languages: [{ iso_639_1: "en", name: "English" }],
    status: "Released",
    tagline: "A test tagline",
    title: "Test Movie",
    video: false,
    vote_average: 7.5,
    vote_count: 1000,
  };

  beforeEach(() => {
    vi.spyOn(api, "getMovieDetails").mockImplementation((movieId: number) =>
      Promise.resolve(mockData)
    );
  });

  afterEach(() => vi.restoreAllMocks());

  it("fetches data when id > 0", async () => {
    const { result } = renderHook(() => useMovieDetails(1), {
      wrapper,
    });
    expect(result.current.isLoading).toBe(true);
    await waitFor(() => result.current.isSuccess);
    expect(result.current.data).toEqual(mockData);
    expect(api.getMovieDetails).toHaveBeenCalledWith(1);
  });
});
