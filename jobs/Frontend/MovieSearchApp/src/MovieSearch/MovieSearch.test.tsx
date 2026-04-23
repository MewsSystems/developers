import { afterEach, describe, expect, test, vi } from "vitest";
import { cleanup, render, screen, waitForElementToBeRemoved } from "@testing-library/react";
import { TestProviders } from "../../testUtils";
import { Movies, useGetMovies } from "./useGetMovies";
import { MovieSearch } from "./MovieSearch";
import userEvent from "@testing-library/user-event";

vi.mock("./useGetMovies");

describe("MovieSearch", () => {
  afterEach(() => {
    vi.resetAllMocks();
    vi.resetModules();
    cleanup();
  });

  test(`renders a search input and a welcome message`, async () => {
    render(<MovieSearch />, { wrapper: TestProviders });

    expect(screen.getByRole("textbox", { name: /search movies/i })).toBeVisible();
    expect(screen.getByText(/Search TheMovieDB for movies!/i)).toBeVisible();
  });

  describe(`when user searches for a movie`, () => {
    describe(`when query returns data`, () => {
      test(`when some movies are found,
          renders a table with 3 columns - release year, language and title
          and a paginator`, async () => {
        vi.mocked(useGetMovies, { partial: true }).mockReturnValue({
          data: testMovies,
          isError: false,
        });

        const user = userEvent.setup();

        render(<MovieSearch />, { wrapper: TestProviders });

        const searchInput = screen.getByRole("textbox", { name: /search movies/i });
        await user.type(searchInput, "exciting movie");

        expect(await screen.findByRole("cell", { name: /exciting movie/i })).toBeVisible();
        expect(await screen.findByRole("cell", { name: /en/i })).toBeVisible();
        expect(await screen.findByRole("cell", { name: /2024/i })).toBeVisible();
        expect(await screen.findByText(/page 1 of 1/i)).toBeVisible();
        expect(await screen.findByRole("button", { name: /next/i })).toBeVisible();
        expect(await screen.findByRole("button", { name: /previous/i })).toBeVisible();
        expect(await screen.findByRole("button", { name: /first/i })).toBeVisible();
        expect(await screen.findByRole("button", { name: /last/i })).toBeVisible();
      });

      test(`when no movies are found,
          renders "Found 0 movies" message`, async () => {
        vi.mocked(useGetMovies, { partial: true }).mockReturnValue({
          data: testNoMovies,
          isError: false,
        });
        const user = userEvent.setup();

        render(<MovieSearch />, { wrapper: TestProviders });

        const searchInput = screen.getByRole("textbox", { name: /search movies/i });
        await user.type(searchInput, "exciting movie");

        expect(await screen.findByText(/Found 0 movies./i)).toBeVisible();
      });

      test(`When movies fail to get fetched, renders "Ooops, something went wrong." message)`, async () => {
        vi.mocked(useGetMovies, { partial: true }).mockReturnValue({
          data: undefined,
          isError: true,
        });
        const user = userEvent.setup();

        render(<MovieSearch />, { wrapper: TestProviders });

        const searchInput = screen.getByRole("textbox", { name: /search movies/i });
        await user.type(searchInput, "exciting movie");

        expect(await screen.findByText(/Ooops, something went wrong./i)).toBeVisible();
      });

      test(`when user clicks on a row, renders a dialog with movie details`, async () => {
        vi.mocked(useGetMovies, { partial: true }).mockReturnValue({
          data: testMovies,
          isError: false,
        });
        const user = userEvent.setup();

        render(<MovieSearch />, { wrapper: TestProviders });

        const searchInput = screen.getByRole("textbox", { name: /search movies/i });
        await user.type(searchInput, "movie");
        const row = await screen.findByRole("row");
        user.click(row);

        expect(await screen.findByRole("dialog")).toBeVisible();
      });
    });

    test(`when query returns no data, renders just the search input with the value that was typed in
          and no welcome message or table`, async () => {
      vi.mocked(useGetMovies, { partial: true }).mockReturnValue({
        data: undefined,
        isError: false,
      });
      const user = userEvent.setup();

      render(<MovieSearch />, { wrapper: TestProviders });

      const searchInput = screen.getByRole("textbox", { name: /search movies/i });
      await user.type(searchInput, "exciting movie");

      await waitForElementToBeRemoved(() => screen.queryByText(/Search TheMovieDB for movies!/i));
      expect(screen.getByRole("textbox", { name: /search movies/i })).toHaveValue("exciting movie");
      expect(screen.queryByRole("table", { name: /list of movies/i })).not.toBeInTheDocument();
    });
  });
});

const testMovies: Movies = {
  page: 1,
  total_pages: 1,
  total_results: 20,
  results: [
    {
      id: 1,
      original_title: "Exciting movie",
      release_date: "2024-1-1",
      original_language: "en",
    },
  ],
};

const testNoMovies: Movies = {
  page: 1,
  total_pages: 1,
  total_results: 0,
  results: [],
};
