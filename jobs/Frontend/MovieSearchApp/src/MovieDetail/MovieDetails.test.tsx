import { afterEach, describe, expect, test, vi } from "vitest";
import { cleanup, render, screen } from "@testing-library/react";
import { TestProviders } from "../../testUtils";
import { MovieDetail } from "./MovieDetail";
import { Movie } from "./useGetMovie";
import { useGetMovie } from "./useGetMovie";

vi.mock("./useGetMovie");

describe("MovieDetail", () => {
  afterEach(() => {
    vi.resetAllMocks();
    vi.resetModules();
    cleanup();
  });

  describe("When movie data is fetched successfuly", () => {
    test(`when all data is present,
          renders movie details(original title, image placeholder, genres, production countries, runtime)`, async () => {
      vi.mocked(useGetMovie, { partial: true }).mockReturnValue({
        isPending: false,
        data: testMovieAllData,
        isError: false,
      });

      render(<MovieDetail movieId={1} movieTitle="Lord of the rings: Two Towers" onClose={() => {}} />, {
        wrapper: TestProviders,
      });

      expect(await screen.findByText(/Lord of the rings: Two Towers/i)).toBeVisible();
      expect(await screen.findByText(/loading image/i)).toBeDefined();
      expect(await screen.findByText(/Fantasy/i)).toBeVisible();
      expect(await screen.findByText(/Adventure/i)).toBeVisible();
      expect(await screen.findByText(/USA/i)).toBeVisible();
      expect(await screen.findByText(/2002/i)).toBeVisible();
      expect(await screen.findByText(/160/i)).toBeVisible();
    });

    test(`when genres, production countries, release year or runtime is missing,
          renders the unknown message and a movie title`, async () => {
      vi.mocked(useGetMovie, { partial: true }).mockReturnValue({
        isPending: false,
        data: testMovieDataMissing,
        isError: false,
      });

      render(<MovieDetail movieId={1} movieTitle="Lord of the rings: Two Towers" onClose={() => {}} />, {
        wrapper: TestProviders,
      });

      expect(await screen.findByText(/Lord of the rings: Two Towers/i)).toBeVisible();
      expect(await screen.findByText(/Genres unknown/i)).toBeVisible();
      expect(await screen.findByText(/Production countries unknown/i)).toBeVisible();
      expect(await screen.findByText(/Release year unknown/i)).toBeVisible();
      expect(await screen.findByText(/Runtime unknown/i)).toBeVisible();
    });
  });

  test(`When movie data is being fetched,
   renders "Fetching movie details..." loading message and a movie title)`, async () => {
    vi.mocked(useGetMovie, { partial: true }).mockReturnValue({
      isPending: true,
      data: undefined,
      isError: false,
    });

    render(<MovieDetail movieId={1} movieTitle="Cool movie" onClose={() => {}} />, {
      wrapper: TestProviders,
    });

    expect(await screen.findByText(/cool movie/i)).toBeVisible();
    expect(await screen.findByText(/Fetching movie details.../i)).toBeVisible();
  });

  test(`When movie data fails to get fetched,
   renders "Ooops, something went wrong." message and a movie title)`, async () => {
    vi.mocked(useGetMovie, { partial: true }).mockReturnValue({
      isPending: false,
      data: undefined,
      isError: true,
    });

    render(<MovieDetail movieId={1} movieTitle="Cool movie" onClose={() => {}} />, {
      wrapper: TestProviders,
    });

    expect(await screen.findByText(/cool movie/i)).toBeVisible();
    expect(await screen.findByText(/Ooops, something went wrong./i)).toBeVisible();
  });
});

const testMovieAllData: Movie = {
  id: 1,
  original_title: "Lord of The Rings: Two Towers",
  genres: [
    { id: 1, name: "Fantasy" },
    { id: 2, name: "Adventure" },
  ],
  production_countries: [{ name: "USA" }],
  release_date: "2002-10-10",
  runtime: 160,
};

const testMovieDataMissing: Movie = {
  id: 1,
  original_title: "Movie",
};
