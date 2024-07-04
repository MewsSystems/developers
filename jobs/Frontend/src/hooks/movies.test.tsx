import { PropsWithChildren } from "react";
import { renderHook, waitFor } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import fetchMock from "fetch-mock";
import type { MovieResponse, MovieDetails } from "../api";
import { useSearchMovies, useGetMovieDetail } from "./movies";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: false,
    },
  },
});

const wrapper = ({ children }: PropsWithChildren<{}>) => (
  <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
);

const MockedMovieResponse: DeepPartial<MovieResponse> = {
  results: [
    { id: 1, title: "Movie 1" },
    { id: 2, title: "Movie 2" },
  ],
};

const MockedMovieDetailResponse: DeepPartial<MovieDetails> = {
  id: 1,
  title: "Movie 1",
};

fetchMock.get(
  "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&query=success&page=1",
  MockedMovieResponse,
);

fetchMock.get(
  "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&query=failure500&page=1",
  500,
);

fetchMock.get(
  "https://api.themoviedb.org/3/movie/1?api_key=03b8572954325680265531140190fd2a",
  MockedMovieDetailResponse,
);

fetchMock.get(
  "https://api.themoviedb.org/3/movie/failure500?api_key=03b8572954325680265531140190fd2a",
  500,
);

describe("useMovies", () => {
  beforeEach(() => {
    queryClient.clear();
  });

  it("should fetch movies and update state correctly", async () => {
    const { result } = renderHook(() => useSearchMovies("success"), {
      wrapper,
    });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data?.pages).toEqual([
      [
        { id: 1, title: "Movie 1" },
        { id: 2, title: "Movie 2" },
      ],
    ]);
    expect(result.current.error).toBe(null);
  });

  it("should handle error when fetching movies", async () => {
    const { result } = renderHook(() => useSearchMovies("failure500"), {
      wrapper,
    });

    await waitFor(() => expect(result.current.isError).toBe(true));

    expect(result.current.data).toEqual(undefined);
    expect(result.current.isLoading).toBe(false);
    expect(result.current.error?.message).toBe(
      "Impossible to fetch movies. Status code 500",
    );
  });

  it("should fetch movie detail and update state correctly", async () => {
    const { result } = renderHook(() => useGetMovieDetail("1"), { wrapper });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toEqual({ id: 1, title: "Movie 1" });
    expect(result.current.error).toBe(null);
  });

  it("should handle error when fetching movie details", async () => {
    const { result } = renderHook(() => useGetMovieDetail("failure500"), {
      wrapper,
    });

    await waitFor(() => expect(result.current.isError).toBe(true));

    expect(result.current.data).toEqual(undefined);

    expect(result.current.error?.message).toBe(
      "Impossible to fetch movie details. Status code 500",
    );
  });
});
