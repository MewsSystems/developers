import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import { QueryClientProvider, QueryClient } from "@tanstack/react-query";
import { expect, test, describe, vi, afterEach } from "vitest";
import { cleanup } from "@testing-library/react";
import * as useMoviesQueryHooks from "../../hooks/useMoviesQuery";

import Home from "./Home";
const queryClient = new QueryClient();

class IntersectionObserver {
  root = null;
  rootMargin = "";
  thresholds = [];

  disconnect() {
    return null;
  }

  observe() {
    return null;
  }

  takeRecords() {
    return [];
  }

  unobserve() {
    return null;
  }
}
window.IntersectionObserver = IntersectionObserver;
global.IntersectionObserver = IntersectionObserver;

describe("Movies", () => {
  const useMoviesQuerySpy = vi.spyOn(useMoviesQueryHooks, "useMoviesQuery");

  afterEach(() => {
    useMoviesQuerySpy.mockClear();
    cleanup();
  });

  test("renders a list of movies", async () => {
    useMoviesQuerySpy.mockReturnValue({
      isLoading: false,
      movies: [
        {
          adult: false,
          backdrop_path: "/qrGtVFxaD8c7et0jUtaYhyTzzPg.jpg",
          genre_ids: [878, 28, 12],
          id: 823464,
          original_language: "en",
          original_title: "Godzilla x Kong: The New Empire",
          overview:
            "Following their explosive showdown, Godzilla and Kong must reunite against a colossal undiscovered threat hidden within our world, challenging their very existence – and our own.",
          popularity: 8350.714,
          poster_path: "/z1p34vh7dEOnLDmyCrlUVLuoDzd.jpg",
          release_date: "2024-03-27",
          title: "Godzilla x Kong: The New Empire",
          video: false,
          vote_average: 7.122,
          vote_count: 1619,
        },
        {
          adult: false,
          backdrop_path: "/fypydCipcWDKDTTCoPucBsdGYXW.jpg",
          genre_ids: [878, 12, 28],
          id: 653346,
          original_language: "en",
          original_title: "Kingdom of the Planet of the Apes",
          overview:
            "Several generations in the future following Caesar's reign, apes are now the dominant species and live harmoniously while humans have been reduced to living in the shadows. As a new tyrannical ape leader builds his empire, one young ape undertakes a harrowing journey that will cause him to question all that he has known about the past and to make choices that will define a future for apes and humans alike.",
          popularity: 1802.132,
          poster_path: "/gKkl37BQuKTanygYQG1pyYgLVgf.jpg",
          release_date: "2024-05-08",
          title: "Kingdom of the Planet of the Apes",
          video: false,
          vote_average: 7.197,
          vote_count: 384,
        },
      ],
      error: null,
      fetchNextPage: vi.fn(),
      hasNextPage: false,
      isFetching: false,
      isError: false,
    });

    render(
      <QueryClientProvider client={queryClient}>
        <Home />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const movieContainer = await screen.findByTestId("movie-card-container");

    expect(movieContainer.childNodes.length).toEqual(2);
  });

  test("renders a loader if movies are loading", async () => {
    useMoviesQuerySpy.mockReturnValue({
      isLoading: true,
      movies: [],
      error: null,
      fetchNextPage: vi.fn(),
      hasNextPage: false,
      isFetching: false,
      isError: false,
    });

    render(
      <QueryClientProvider client={queryClient}>
        <Home />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const spinner = await screen.findByTestId("spinner");

    expect(spinner).toBeDefined();
  });

  test("renders an error if there is an error", async () => {
    useMoviesQuerySpy.mockReturnValue({
      isLoading: false,
      movies: [],
      fetchNextPage: vi.fn(),
      hasNextPage: false,
      isFetching: false,
      isError: true,
      error: null,
    });

    render(
      <QueryClientProvider client={queryClient}>
        <Home />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const errorMessage = screen.findByText("An error has occurred");

    expect(errorMessage).toBeDefined();
  });
});
