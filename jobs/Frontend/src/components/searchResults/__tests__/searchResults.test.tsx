import { render, screen } from "@testing-library/react";
import { SearchResults } from "../searchResults";
import { Movie } from "@/types";

jest.mock("next/image", () => () => <span></span>);

const defaultMovies: Movie[] = [
  {
    id: 1,
    title: "title1",
    overview: "test",
    posterImage: "",
  },
  {
    id: 2,
    title: "title2",
    overview: "test",
    posterImage: "",
  },
];

const defaultSearchResponse = {
  page: 0,
  movies: defaultMovies,
};

describe("Search results", () => {
  it("should display search results", () => {
    const list = <SearchResults results={defaultSearchResponse} />;
    render(list);
    expect(screen.getByRole("list")).toBeInTheDocument();
  });

  it("should display movie list items", () => {
    const list = <SearchResults results={defaultSearchResponse} />;
    render(list);
    expect(screen.getAllByRole("link")).toHaveLength(2);
  });

  it("should display movie item properties", () => {
    const list = <SearchResults results={defaultSearchResponse} />;
    render(list);
    expect(screen.getByText("title2")).toBeInTheDocument();
  });

  it("should have a movie detail link", () => {
    const list = <SearchResults results={defaultSearchResponse} />;
    render(list);
    expect(screen.getByRole("link", { name: /1/ })).toBeInTheDocument();
  });
});
