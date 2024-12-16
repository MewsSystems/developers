import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { SearchPage } from "./SearchPage";
import { useMoviesFetch } from "../../hooks/useMoviesFetch";

jest.mock("../../hooks/useMoviesFetch");
const useMoviesFetchMock = useMoviesFetch as jest.Mock;

describe("SearchPage", () => {
  it("renders the search page", () => {
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: false,
      error: null,
    });

    render(<SearchPage />);

    expect(screen.getByText("Cinematic")).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText("Search for a movie...")
    ).toBeInTheDocument();
  });

  it("renders loading state", () => {
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: true,
      error: null,
    });

    render(<SearchPage />);

    expect(screen.getByText("Loading...")).toBeInTheDocument();
  });

  it("renders error state", () => {
    useMoviesFetchMock.mockReturnValue({
      data: [],
      totalPages: 0,
      isLoading: false,
      error: new Error("An error occurred"),
    });

    render(<SearchPage />);

    expect(screen.getByText("An error occurred")).toBeInTheDocument();
  });

  it("renders movies", () => {
    useMoviesFetchMock.mockReturnValue({
      data: [
        {
          id: 1,
          title: "Movie 1",
          poster_path: "/movie1.jpg",
        },
        {
          id: 2,
          title: "Movie 2",
          poster_path: "/movie2.jpg",
        },
      ],
      totalPages: 1,
      isLoading: false,
      error: null,
    });

    render(<SearchPage />);

    expect(screen.getByText("Movie 1")).toBeInTheDocument();
    expect(screen.getByText("Movie 2")).toBeInTheDocument();
  });
});
