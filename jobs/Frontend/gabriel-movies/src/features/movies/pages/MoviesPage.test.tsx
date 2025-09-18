import { describe, it, expect, vi, beforeEach } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import "@/test-utils/mocks/Loader.mock";
import "@/test-utils/mocks/EmptyState.mock";
import MoviesPage from "./MoviesPage";
import { MOVIE_INTERSTELLAR } from "../fixtures/movies";
import type { Movie } from "../types";
import type { UseSearchMoviesProps } from "../hooks/useSearchMovies";

vi.mock("../components/MovieList", () => ({
  MovieList: ({ items }: { items: Movie[] }) => <div data-testid="movie-list" data-count={items.length} />
}));

const useSearchMoviesMock = vi.fn();
vi.mock("../hooks/useSearchMovies", () => ({
  useSearchMovies: (q: string) => useSearchMoviesMock(q)
}));

const useSearchMoviesState = (overrides: Partial<UseSearchMoviesProps> = {}): UseSearchMoviesProps => {
  return {
    data: [],
    isLoading: false,
    isFetchingNextPage: false,
    hasNextPage: false,
    fetchNextPage: vi.fn(),
    ...overrides,
  };
}

describe("MoviesPage", () => {
  beforeEach(() => {
    vi.resetAllMocks();
  });

  it("shows loader when fetching", () => {
    useSearchMoviesMock.mockReturnValue(useSearchMoviesState({ isLoading: true }));

    render(<MoviesPage />, { initialEntries: ["/?q=anything"] });

    expect(screen.getByTestId("loader")).toBeInTheDocument();
    expect(screen.queryByTestId("empty-state")).not.toBeInTheDocument();
    expect(screen.queryByTestId("movie-list")).not.toBeInTheDocument();
  });

  it("shows empty state when no results", () => {
    useSearchMoviesMock.mockReturnValue(useSearchMoviesState());

    render(<MoviesPage />, { initialEntries: ["/?q=nomatch"] });

    expect(screen.getByTestId("empty-state")).toBeInTheDocument();
    expect(screen.queryByTestId("loader")).not.toBeInTheDocument();
    expect(screen.queryByTestId("movie-list")).not.toBeInTheDocument();
  });

  it("shows list when there are results", () => {
    useSearchMoviesMock.mockReturnValue(useSearchMoviesState({ data: [MOVIE_INTERSTELLAR] }));

    render(<MoviesPage />, { initialEntries: ["/?q=interstellar"] });

    const list = screen.getByTestId("movie-list");
    expect(list).toBeInTheDocument();
    expect(list).toHaveAttribute("data-count", "1");
    expect(screen.queryByTestId("loader")).not.toBeInTheDocument();
    expect(screen.queryByTestId("empty-state")).not.toBeInTheDocument();
  });
});
