import { render, screen } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { MemoryRouter } from "react-router";
import { userEvent } from "@testing-library/user-event";
import { useMovieResultsList } from "@/hooks/queries/useMovieResultsList";
import MovieSearchPage from "../MovieSearchPage/MovieSearchPage";
import { expect, describe } from "vitest";
import { ErrorReason } from "@/api/base";
import { MovieResultsList } from "@/api/schemas/movieSchema";

interface BaseQueryResult<T> {
  data: T | undefined;
  isLoading: boolean;
  isError: boolean;
  error: ErrorReason | null;
}

const queryClient = new QueryClient();

const renderWithProviders = () =>
  render(
    <QueryClientProvider client={queryClient}>
      <MemoryRouter>
        <MovieSearchPage />
      </MemoryRouter>
    </QueryClientProvider>
  );

const mockMovieSearchResults: MovieResultsList = {
  page: 1,
  results: [
    {
      id: 1,
      title: 'Test Movie 1',
      image: 'https://image.tmdb.org/t/p/w400/poster1.jpg',
      voteAverage: '7.5',
      voteCount: 1000,
      year: '2023',
      overview: 'Test overview 1',
    },
    {
      id: 2,
      title: 'Test Movie 2',
      image: 'https://image.tmdb.org/t/p/w400/poster2.jpg',
      voteAverage: '8.0',
      voteCount: 2000,
      year: '2023',
      overview: 'Test overview 2',
    }
  ],
  totalPages: 2,
  totalResults: 2
};

vi.mock("@/hooks/queries/useMovieResultsList", () => ({
  useMovieResultsList: vi.fn((): BaseQueryResult<MovieResultsList> => ({
    data: mockMovieSearchResults,
    isLoading: false,
    isError: false,
    error: null
  }))
}));

describe("MovieSearchPage", () => {
  afterEach(() => {
    vi.resetAllMocks();
  });

  it("renders search input field", () => {
    renderWithProviders();
    const input = screen.getByRole("textbox");

    expect(input).toBeInTheDocument();
  });

  it("updates the search query on user input", async () => {
    renderWithProviders();
    const searchInput = screen.getByRole("textbox");
    await userEvent.type(searchInput, "Inception");

    expect(searchInput).toHaveValue("Inception");
  });

  it("displays movie cards when search results are fetched", async () => {
    renderWithProviders();

    expect(await screen.findByText("Test Movie 1")).toBeInTheDocument();
    expect(await screen.findByText("Test Movie 2")).toBeInTheDocument();
  });

  it("shows loading skeletons when fetching movies", async () => {
    vi.mocked(useMovieResultsList).mockReturnValue({
      data: undefined,
      isLoading: true,
      isError: false,
      error: null,
    } as any);

    renderWithProviders();

    expect(screen.getAllByTestId("skeleton")).toHaveLength(4);
  });

  it("displays error screen when there's an error", async () => {
    vi.mocked(useMovieResultsList).mockReturnValue({
      data: undefined,
      isLoading: true,
      isError: true,
      error: 'unreachable',
    } as any);

    renderWithProviders();

    expect(screen.getByText("We're having trouble connecting to the server. Please try again later."))
      .toBeInTheDocument();
  });
});
