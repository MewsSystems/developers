import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { vi } from "vitest";
import SearchPage from "./SearchPage";

// Mock custom hooks
const mockUseMovieSearch = vi.fn();
vi.mock("../hooks/useMovieSearch", () => ({
  default: () => mockUseMovieSearch(),
}));

const mockUseDebounce = vi.fn();
vi.mock("../hooks/useDebounce", () => ({
  default: (value: string, delay: number) => mockUseDebounce(value, delay),
}));

describe("SearchPage", () => {
  const mockSearch = vi.fn();
  const mockReset = vi.fn();
  const mockSetQuery = vi.fn();

  const defaultHookReturn = {
    query: "",
    setQuery: mockSetQuery,
    movies: [],
    loading: false,
    error: null,
    page: 1,
    search: mockSearch,
    reset: mockReset,
  };

  beforeEach(() => {
    vi.clearAllMocks();
    mockUseMovieSearch.mockReturnValue(defaultHookReturn);
    mockUseDebounce.mockImplementation((value) => value);
  });

  it("renders the search page with all basic elements", () => {
    render(<SearchPage />);

    expect(screen.getByText("Movie Search")).toBeInTheDocument();
    expect(screen.getByPlaceholderText("Search movies...")).toBeInTheDocument();
    expect(screen.getByTestId("layout")).toBeInTheDocument();
    expect(screen.getByTestId("movie-grid")).toBeInTheDocument();
  });

  it("handles input changes correctly", async () => {
    render(<SearchPage />);
    const user = userEvent.setup();

    const searchInput = screen.getByPlaceholderText("Search movies...");
    await user.type(searchInput, "batman");

    expect(mockSetQuery).toHaveBeenCalled();
    expect(mockSetQuery).toHaveBeenCalledTimes(6);
  });

  it("shows loader when loading is true", () => {
    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      loading: true,
    });

    render(<SearchPage />);
    expect(screen.getByTestId("loader")).toBeInTheDocument();
  });

  it("shows error state when there is an error", () => {
    const errorMessage = "Failed to fetch movies";
    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      error: errorMessage,
    });

    render(<SearchPage />);
    expect(screen.getByTestId("error-state")).toBeInTheDocument();
    expect(screen.getByText(errorMessage)).toBeInTheDocument();
  });

  it("shows empty state when no movies found and query exists", () => {
    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      query: "nonexistentmovie",
      movies: [],
      loading: false,
      error: null,
    });

    render(<SearchPage />);
    expect(screen.getByTestId("empty-state")).toBeInTheDocument();
    expect(
      screen.getByText("No movies found for your search.")
    ).toBeInTheDocument();
  });

  it("does not show empty state when query is empty", () => {
    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      query: "",
      movies: [],
      loading: false,
      error: null,
    });

    render(<SearchPage />);
    expect(screen.queryByTestId("empty-state")).not.toBeInTheDocument();
  });

  it("renders movie cards when movies are available", () => {
    const mockMovies = [
      { id: 1, title: "Avengers", poster_path: "/avengers.jpg" },
      { id: 2, title: "Spider-Man", poster_path: "/spiderman.jpg" },
    ];

    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      movies: mockMovies,
    });

    render(<SearchPage />);

    expect(screen.getByTestId("movie-card-1")).toBeInTheDocument();
    expect(screen.getByTestId("movie-card-2")).toBeInTheDocument();
    expect(screen.getByText("Avengers")).toBeInTheDocument();
    expect(screen.getByText("Spider-Man")).toBeInTheDocument();
  });

  it("shows load more button when movies are available and not loading", () => {
    const mockMovies = [
      { id: 1, title: "Avengers", poster_path: "/avengers.jpg" },
    ];

    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      movies: mockMovies,
      loading: false,
    });

    render(<SearchPage />);

    const loadMoreButton = screen.getByRole("button", { name: "Load More" });
    expect(loadMoreButton).toBeInTheDocument();
    expect(loadMoreButton).not.toBeDisabled();
  });

  it("does not show load more button when loading", () => {
    const mockMovies = [
      { id: 1, title: "Avengers", poster_path: "/avengers.jpg" },
    ];

    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      movies: mockMovies,
      loading: true,
    });

    render(<SearchPage />);

    // Button should not be present when loading
    expect(
      screen.queryByRole("button", { name: "Load More" })
    ).not.toBeInTheDocument();
  });

  it("calls search with next page when load more is clicked", async () => {
    const mockMovies = [
      { id: 1, title: "Avengers", poster_path: "/avengers.jpg" },
    ];

    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      query: "avengers",
      movies: mockMovies,
      page: 2,
      loading: false,
    });

    mockUseDebounce.mockReturnValue("avengers");

    render(<SearchPage />);
    const user = userEvent.setup();

    const loadMoreButton = screen.getByRole("button", { name: "Load More" });
    await user.click(loadMoreButton);

    expect(mockSearch).toHaveBeenCalledWith("avengers", 3);
  });

  it("calls search when debounced query changes", () => {
    const { rerender } = render(<SearchPage />);

    // Mock debounce to return different value
    mockUseDebounce.mockReturnValue("batman");

    // Force re-render to trigger useEffect
    rerender(<SearchPage />);

    expect(mockSearch).toHaveBeenCalledWith("batman");
  });

  it("calls reset when debounced query is empty", () => {
    const { rerender } = render(<SearchPage />);

    // Mock debounce to return empty string
    mockUseDebounce.mockReturnValue("");

    // Force re-render to trigger useEffect
    rerender(<SearchPage />);

    expect(mockReset).toHaveBeenCalled();
    expect(mockSearch).not.toHaveBeenCalled();
  });

  it("does not show load more button when no movies", () => {
    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      movies: [],
      loading: false,
    });

    render(<SearchPage />);

    expect(
      screen.queryByRole("button", { name: "Load More" })
    ).not.toBeInTheDocument();
  });

  it("does not show load more button when loading", () => {
    const mockMovies = [
      { id: 1, title: "Avengers", poster_path: "/avengers.jpg" },
    ];

    mockUseMovieSearch.mockReturnValue({
      ...defaultHookReturn,
      movies: mockMovies,
      loading: true,
    });

    render(<SearchPage />);

    expect(
      screen.queryByRole("button", { name: "Load More" })
    ).not.toBeInTheDocument();
  });
});
