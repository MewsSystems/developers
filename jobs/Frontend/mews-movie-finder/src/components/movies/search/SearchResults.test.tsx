import { describe, expect, it } from "vitest";

import { screen } from "@testing-library/react";

import "@testing-library/jest-dom";
import { SearchResults } from "./SearchResults";
import { renderWithClient } from "../../../tests/testUtils";
import { QueryClient } from "@tanstack/react-query";
import { movies } from "../../../tests/handlers/movieHandlers";

describe("SearchResults", () => {
  const queryClient = new QueryClient();

  it("should render search infor with default message", () => {
    renderWithClient(
      queryClient,
      <SearchResults
        search=""
        page={undefined}
        isLoading={false}
        setPage={() => {}}
      />
    );
    const searchInfo = screen.getByTestId("search-info");

    expect(searchInfo).toBeInTheDocument();
    expect(searchInfo.textContent).toBe(
      "A list of movies will display here..."
    );
  });

  it("should render search infor with keep typing message", () => {
    renderWithClient(
      queryClient,
      <SearchResults
        search="aaa"
        page={undefined}
        isLoading={false}
        setPage={() => {}}
      />
    );
    const searchInfo = screen.getByTestId("search-info");

    expect(searchInfo).toBeInTheDocument();
    expect(searchInfo.textContent).toBe("Keep typing...");
  });

  it("should render search infor with keep loading message", () => {
    renderWithClient(
      queryClient,
      <SearchResults
        search="aaaaa"
        page={undefined}
        isLoading={true}
        setPage={() => {}}
      />
    );
    const searchInfo = screen.getByTestId("search-info");

    expect(searchInfo).toBeInTheDocument();
    expect(searchInfo.textContent).toBe(`Loading movies for "aaaaa"...`);
  });

  it("should render search infor with movies found message", () => {
    renderWithClient(
      queryClient,
      <SearchResults
        search="aaaaa"
        page={{ page: 1, total_pages: 1, total_results: 1, results: movies }}
        isLoading={false}
        setPage={() => {}}
      />
    );
    const searchInfo = screen.getByTestId("search-info");

    expect(searchInfo).toBeInTheDocument();
    expect(searchInfo.textContent).toBe(`Found these movies for "aaaaa"`);
  });

  it("should render search infor with nothing found message", () => {
    renderWithClient(
      queryClient,
      <SearchResults
        search="aaaaa"
        page={{ page: 1, total_pages: 0, total_results: 0, results: [] }}
        isLoading={false}
        setPage={() => {}}
      />
    );
    const searchInfo = screen.getByTestId("search-info");

    expect(searchInfo).toBeInTheDocument();
    expect(searchInfo.textContent).toBe(`No movies found for "aaaaa"`);
  });

  it("should render a list of movies", () => {
    renderWithClient(
      queryClient,
      <SearchResults
        search="aaaaa"
        page={{ page: 1, total_pages: 1, total_results: 1, results: movies }}
        isLoading={false}
        setPage={() => {}}
      />
    );
    const movieList = screen.getByTestId("movie-list");

    expect(movieList).toBeInTheDocument();
  });
});
