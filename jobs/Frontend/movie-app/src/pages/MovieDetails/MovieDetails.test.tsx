import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import { QueryClientProvider, QueryClient } from "@tanstack/react-query";
import { expect, test, describe, vi, afterEach } from "vitest";
import { cleanup } from "@testing-library/react";
import * as useMovieDetailsQueryHooks from "../../hooks/useMovieDetailsQuery";

import MovieDetails from "./MovieDetails";
const queryClient = new QueryClient();

describe("MovieDetails", () => {
  const useMovieDetailsQuerySpy = vi.spyOn(
    useMovieDetailsQueryHooks,
    "useMovieDetailsQuery"
  );

  afterEach(() => {
    useMovieDetailsQuerySpy.mockClear();
    cleanup();
  });

  test("renders the movie details", async () => {
    useMovieDetailsQuerySpy.mockReturnValue({
      isLoading: false,
      movie: {
        adult: false,
        backdrop_path: "/qrGtVFxaD8c7et0jUtaYhyTzzPg.jpg",
        genres: [
          {
            id: 878,
            name: "Science Fiction",
          },
          {
            id: 12,
            name: "Adventure",
          },
          {
            id: 28,
            name: "Action",
          },
        ],
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
      error: null,
      isError: false,
      movieRuntime: {
        hours: 0,
        minutes: 0,
      },
    });

    render(
      <QueryClientProvider client={queryClient}>
        <MovieDetails />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const movieTitle = screen.getByRole("heading", {
      name: /godzilla x kong: the new empire/i,
    });
    const overview = screen.getByText(
      /following their explosive showdown, godzilla and kong must reunite against a colossal undiscovered threat hidden within our world, challenging their very existence – and our own\./i
    );
    const movieImage = screen.getByRole("img", {
      name: /godzilla x kong: the new empire/i,
    });
    expect(movieTitle).toBeDefined();
    expect(overview).toBeDefined();
    expect(movieImage).toBeDefined();
  });

  test("renders a loader if movie detail is loading", async () => {
    useMovieDetailsQuerySpy.mockReturnValue({
      isLoading: true,
      movie: {},
      error: null,
      isError: false,
      movieRuntime: {
        hours: 0,
        minutes: 0,
      },
    });

    render(
      <QueryClientProvider client={queryClient}>
        <MovieDetails />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const spinner = await screen.findByTestId("spinner");

    expect(spinner).toBeDefined();
  });

  test("renders an error if there is an error", async () => {
    useMovieDetailsQuerySpy.mockReturnValue({
      isLoading: false,
      movie: {},
      isError: true,
      error: null,
      movieRuntime: {
        hours: 0,
        minutes: 0,
      },
    });

    render(
      <QueryClientProvider client={queryClient}>
        <MovieDetails />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const errorMessage = screen.findByText("An error has occurred");

    expect(errorMessage).toBeDefined();
  });

  test("renders a back button", async () => {
    useMovieDetailsQuerySpy.mockReturnValue({
      isLoading: false,
      movie: {},
      isError: false,
      error: null,
      movieRuntime: {
        hours: 0,
        minutes: 0,
      },
    });

    render(
      <QueryClientProvider client={queryClient}>
        <MovieDetails />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const backButton = screen.getByRole("button", {
      name: /< back/i,
    });
    expect(backButton).toBeDefined();
  });
});
