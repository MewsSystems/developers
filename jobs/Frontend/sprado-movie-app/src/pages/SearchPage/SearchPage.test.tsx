import React from "react";
import { render, screen } from "@testing-library/react";
import { SearchPage } from "./SearchPage";
import { useFetchMovies } from "../../hooks/useFetchMovies/useFetchMovies";
import { useDebouncedValue } from "../../hooks/useDebouncedValue/useDebouncedValue";
import { MemoryRouter, Route, Routes } from "react-router-dom";

jest.mock("../../hooks/useFetchMovies");

jest.mock("../../hooks/useDebouncedValue/useDebouncedValue");
const useMoviesFetchMock = useFetchMovies as jest.Mock;
const useDebouncedValueMock = useDebouncedValue as jest.Mock;

const renderPage = () => {
  render(
    <MemoryRouter>
      <Routes>
        <Route path="/" element={<SearchPage />} />
      </Routes>
    </MemoryRouter>
  );
};

const movies = [
  {
    id: 1,
    title: "Movie One",
    release_date: "2021-01-01",
    overview: "Overview of Movie One",
    vote_average: 8.5,
    poster_path: "/poster1.jpg",
    backdrop_path: "/backdrop1.jpg",
  },
  {
    id: 2,
    title: "Movie Two",
    release_date: "2021-02-01",
    overview: "Overview of Movie Two",
    vote_average: 7.2,
    poster_path: "/poster2.jpg",
    backdrop_path: "/backdrop2.jpg",
  },
];

describe("SearchPage", () => {
  it("renders the search page with header and search bar", () => {
    useDebouncedValueMock.mockReturnValue("");
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: false,
      error: null,
    });

    renderPage();

    expect(screen.getByText("Cinematic")).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText("Search for a movie...")
    ).toBeInTheDocument();
  });

  it("renders loading state", () => {
    useDebouncedValueMock.mockReturnValue("initial");
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: true,
      error: null,
    });

    renderPage();

    expect(screen.getByText("Loading...")).toBeInTheDocument();
  });

  it("renders error state", () => {
    useDebouncedValueMock.mockReturnValue("initial");
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: false,
      error: new Error("An error occurred"),
    });

    renderPage();

    expect(screen.getByText("An error occurred")).toBeInTheDocument();
  });

  it("renders no results message", () => {
    useDebouncedValueMock.mockReturnValue("search query");
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: false,
      error: null,
    });

    renderPage();

    expect(screen.getByText("No movies found.")).toBeInTheDocument();
  });

  it("renders movie list and pagination when movies are available", () => {
    useDebouncedValueMock.mockReturnValue("search query");
    useMoviesFetchMock.mockReturnValue({
      data: movies,
      totalPages: 2,
      isLoading: false,
      error: null,
    });

    renderPage();

    expect(screen.getByText("Movie One")).toBeInTheDocument();
    expect(screen.getByText("Movie Two")).toBeInTheDocument();

    expect(screen.getByRole("button", { name: "1" })).toBeInTheDocument();
  });
});
