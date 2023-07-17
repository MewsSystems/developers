import { vi, describe, it, expect, beforeEach, afterEach } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { Store } from "@reduxjs/toolkit";

import { RootState, store } from "../../store";
import * as api from "../../api";
import { DEFAULT_PLACEHOLDER as SEARCH_BOX_DEFAULT_PLACEHOLDER } from "../../components/SearchInput";
import { GetMoviesResponse } from "../../containers/Movies";
import { Search } from "./Search";
import mockMovieResponseJSON from "./fixture.json";

const MOCK_MOVIES_RESPONSE = mockMovieResponseJSON as GetMoviesResponse;

const getStore = (): Store<RootState> => {
  return Object.create(store) as Store<RootState>;
};

const renderSearchPage = () => (
  <Provider store={getStore()}>
    <MemoryRouter initialEntries={["/"]}>
      <Search />
    </MemoryRouter>
  </Provider>
);

const searchForMovie = async (movie: string) => {
  const input = screen.getByPlaceholderText(SEARCH_BOX_DEFAULT_PLACEHOLDER);

  await userEvent.type(input, movie);

  vi.runAllTimers();
};

const mockRequestWithWholePage = () =>
  vi
    .spyOn(api, "getMovies")
    .mockImplementation(() => Promise.resolve(MOCK_MOVIES_RESPONSE));

describe("Search page", () => {
  beforeEach(() => {
    vi.useFakeTimers({
      shouldAdvanceTime: true,
    });
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it("should properly render the markup pre-search", () => {
    render(renderSearchPage());

    expect(screen.getByRole("heading", { name: "Find your favorite movie!" }));
    expect(
      screen.getByPlaceholderText(SEARCH_BOX_DEFAULT_PLACEHOLDER)
    ).toBeDefined();
  });

  it("should properly render the markup during fetching and after results are loaded", async () => {
    const spy = mockRequestWithWholePage();

    render(renderSearchPage());

    const MOVIE_NAME = "Mission";

    await searchForMovie(MOVIE_NAME);

    expect(spy).toHaveBeenCalledWith(MOVIE_NAME, 1);

    await waitFor(() => {
      expect(screen.getByRole("progressbar")).toBeDefined();
    });

    await waitFor(() => {
      expect(screen.getByRole("list")).toBeDefined();
      expect(screen.getAllByRole("listitem")).toHaveLength(
        MOCK_MOVIES_RESPONSE.results.length
      );
    });
  });

  it('should display "no results" label when response is empty', async () => {
    vi.spyOn(api, "getMovies").mockImplementation(() =>
      Promise.resolve({
        page: 1,
        results: [],
        total_pages: 1,
        total_results: 0,
      } as GetMoviesResponse)
    );

    render(renderSearchPage());

    await searchForMovie("Some random movie that doesnt exist");

    await waitFor(() => {
      expect(
        screen.getByText("There are no movies matching your search!")
      ).toBeDefined();
    });
  });
});
