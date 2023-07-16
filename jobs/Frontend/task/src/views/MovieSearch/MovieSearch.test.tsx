import "whatwg-fetch";
import { act, fireEvent, screen } from "@testing-library/react";
import { MovieSearch } from "./MovieSearch";
import { renderWithProviders } from "src/store/test-utils";

/**
 * Tests MovieList component functionality
 * NOTE: Made a couple of tests here as an example of how would are integration tests done
 * I couldn't get mocks for useLazyGetMoviesQuery to work in a reasonable time, so I will provide comments in the tests
 *
 * This is how would integration tests be done for each flow of a new feature or an existing one
 */
describe("MovieSearch", () => {
  beforeEach(() => jest.clearAllMocks());

  it("renders the MovieSearch component and its parts", () => {
    renderWithProviders(<MovieSearch />);

    expect(screen.getByText("Movie Searcher")).toBeInTheDocument();
    expect(screen.getByText("Movie Searcher")).toBeInTheDocument();
    expect(screen.getByPlaceholderText("Search movies..")).toBeInTheDocument();

    // pagination is not present if there are no movies
    expect(screen.queryByTestId("pagination")).not.toBeInTheDocument();
    expect(screen.queryByTestId("loader")).not.toBeInTheDocument();
  });

  it("searches for movies onDebounce and onEnter", () => {
    const mockMovies = [
      { id: 1, title: "Apple" },
      { id: 2, title: "Apple 2" },
    ];
    const mockFetchMovies = jest.fn();
    const mockUseLazyGetMoviesQuery = jest
      .fn()
      .mockReturnValue([
        mockFetchMovies,
        { isLoading: false, data: { results: mockMovies } },
      ]);

    jest.mock("src/store/slices/moviesSlice", () => ({
      useLazyGetMoviesQuery: mockUseLazyGetMoviesQuery,
    }));

    renderWithProviders(<MovieSearch />);
    const inputElement = screen.getByRole("textbox");

    fireEvent.change(inputElement, { target: { value: "Apple" } });
    act(() => jest.runAllTimers());

    // here is where the test fails, couldn't make mocking of the hook to work
    expect(mockUseLazyGetMoviesQuery).toHaveBeenCalledTimes(1);
    expect(mockFetchMovies).toHaveBeenCalledTimes(1);
    // test would check if fetching was called after debounce on input happened
    // afterwards it would check for the rendering of movieList and mocked movies

    // next check would be to type into the input again and use onEnter event
    // again checking for movieList and movies to be rendered
  });
  it("should display MovieList and use PaginationControls if many movies are found", () => {
    // would check if pagination next and previous works,
    // whether it is disabled for next page and previous page
  });
  it("should navigate to MovieScreen when clicked on a movie", () => {
    // would check if clicking on singular movie works
  });
});
