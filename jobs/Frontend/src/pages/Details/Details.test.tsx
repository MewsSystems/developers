import { vi, describe, it, expect, afterEach } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { Provider } from "react-redux";

import { store } from "../../store";
import { Details } from "./Details";
import * as api from "../../api";
import { MovieDetails } from "../../containers/Movies";

const renderDetailsPage = (movieId = "1") => (
  <Provider store={store}>
    <MemoryRouter initialEntries={[`/movies/${movieId}`]}>
      <Routes>
        <Route path="/movies/:movieId" element={<Details />} />
      </Routes>
    </MemoryRouter>
  </Provider>
);

const MOVIE_ID = "123";

const MOCK_MOVIE = {
  id: Number(MOVIE_ID),
  title: "Mission Impossible",
  poster_path: "/test.jpg",
  tagline: "We all share the same fate.",
  overview:
    "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Sint magni cupiditate libero soluta esse repellat qui ipsa, deserunt doloribus mollitia voluptatibus impedit aperiam commodi eos modi, voluptatem amet rerum! Voluptatum.",
} as MovieDetails;

describe("Details page", () => {
  afterEach(() => {
    vi.clearAllMocks();
  });

  it("should properly render the markup", async () => {
    vi.spyOn(api, "getMovieDetail").mockImplementation(() =>
      Promise.resolve(MOCK_MOVIE)
    );

    render(renderDetailsPage());

    await waitFor(() => {
      expect(
        screen.getByRole("heading", { name: MOCK_MOVIE.title })
      ).toBeDefined();
      expect(
        screen.getByRole("heading", { name: MOCK_MOVIE.tagline })
      ).toBeDefined();
      expect(screen.getByText(MOCK_MOVIE.overview)).toBeDefined();
      expect(
        screen.getByRole("img", { name: `${MOCK_MOVIE.title} Banner` })
      ).toBeDefined();
    });
  });

  it("should make fetch request for route param with matching ID", () => {
    const fetchMock = vi
      .spyOn(api, "getMovieDetail")
      .mockImplementation(() => Promise.resolve(MOCK_MOVIE));

    render(renderDetailsPage(MOVIE_ID));

    expect(screen.getByRole("progressbar")).toBeDefined();

    expect(fetchMock).toHaveBeenCalledWith(MOVIE_ID);
  });

  // Some of the other tests I'd write would include:
  // 1. the screen properly displays the partial data from the selector, when that exists
  // 2. clicking back button works as expected
  // 3. if bad image is passed, it properly displays the default image
  // ...
});
