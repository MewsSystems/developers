import React from "react";
import { render, screen } from "@testing-library/react";
import { useFetchMovieDetails } from "../../hooks/useFetchMovieDetails";
import { MovieDetailPage } from "./MovieDetailPage";
import { MemoryRouter, Route, Routes } from "react-router-dom";

jest.mock("../../hooks/useFetchMovieDetails", () => ({
  useFetchMovieDetails: jest.fn(),
}));

const mockNavigate = jest.fn();

jest.mock("react-router-dom", () => {
  const originalModule = jest.requireActual("react-router-dom");
  return {
    __esModule: true,
    ...originalModule,
    useNavigate: () => mockNavigate,
  };
});

const renderPage = () => {
  render(
    <MemoryRouter initialEntries={["/movie/123"]}>
      <Routes>
        <Route path="/movie/:id" element={<MovieDetailPage />} />
      </Routes>
    </MemoryRouter>
  );
};

describe("MovieDetailPage", () => {
  beforeEach(() => {
    mockNavigate.mockClear();
  });

  it('renders "Loading..." when movie is loading', () => {
    (useFetchMovieDetails as jest.Mock).mockReturnValue({
      movie: null,
      isLoading: true,
      error: null,
    });

    renderPage();

    expect(screen.getByText("Loading...")).toBeInTheDocument();
  });

    it('renders "No movie details found." when movie is not found', () => {
      (useFetchMovieDetails as jest.Mock).mockReturnValue({
        movie: null,
        isLoading: false,
        error: null,
      });

      renderPage();

      expect(screen.getByText("No movie details found.")).toBeInTheDocument();
    });

    it("renders error message when error occurs", () => {
      (useFetchMovieDetails as jest.Mock).mockReturnValue({
        movie: null,
        isLoading: false,
        error: new Error("Failed to fetch movie details"),
      });

      renderPage();

      expect(
        screen.getByText("Failed to fetch movie details")
      ).toBeInTheDocument();
    });

    it("renders movie details when movie is found", () => {
      (useFetchMovieDetails as jest.Mock).mockReturnValue({
        movie: {
          id: "1",
          title: "Movie Title",
          overview: "Movie Overview",
          release_date: "2021-01-01",
          poster_path: "/poster.jpg",
        },
        isLoading: false,
        error: null,
      });

      renderPage();

      expect(screen.getByText("Movie Title")).toBeInTheDocument();
      expect(screen.getByText("Movie Overview")).toBeInTheDocument();
      expect(screen.getByText("2021-01-01")).toBeInTheDocument();
      expect(screen.getByAltText("Movie Title")).toBeInTheDocument();
    });
});
