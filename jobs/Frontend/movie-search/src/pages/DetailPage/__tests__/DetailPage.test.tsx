import { render, screen } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { vi, describe, it, expect, beforeEach } from "vitest";
import "@testing-library/jest-dom";
import { DetailPage } from "../index";
import { MemoryRouter, Routes, Route } from "react-router";
import * as movieDetailsHook from "../../../hooks/useMovieDetailsQuery";

vi.mock("../../../hooks/useMovieDetailsQuery", () => ({
  useMovieDetails: vi.fn(),
}));

vi.mock("../../../utils/movieHelpers", () => ({
  formatRuntime: vi.fn((runtime) => {
    if (runtime == null || runtime <= 0) return "N/A";
    const hours = Math.floor(runtime / 60);
    const minutes = runtime % 60;
    const paddedMins = minutes.toString().padStart(2, "0");
    return `${hours}h ${paddedMins}m`;
  }),
  getImageUrl: vi.fn((path) => `https://image.tmdb.org/t/p/w500${path}`),
  getTranslatedTitle: vi.fn((isEnglish, originalTitle, translatedTitle) =>
    isEnglish ? originalTitle : `${originalTitle} (${translatedTitle})`
  ),
  getYearFromDate: vi.fn((date) => new Date(date).getFullYear().toString()),
}));

vi.mock("../../../assets/no-image-placeholder.jpg", () => ({
  default: "/src/assets/no-image-placeholder.jpg",
}));

function createTestQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

const wrapper = ({ children }: { children: React.ReactNode }) => {
  const queryClient = createTestQueryClient();
  return (
    <QueryClientProvider client={queryClient}>
      <MemoryRouter initialEntries={["/movie/123"]}>
        <Routes>
          <Route path="/movie/:id" element={children} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>
  );
};

const mockMovieData = {
  adult: false,
  backdrop_path: "/backdrop.jpg",
  belongs_to_collection: null,
  budget: 1000000,
  genres: [
    { id: 1, name: "Action" },
    { id: 2, name: "Adventure" },
  ],
  homepage: "https://example.com",
  id: 123,
  imdb_id: "tt1234567",
  original_language: "en",
  original_title: "Test Movie",
  overview: "This is a test movie overview with some interesting plot details.",
  popularity: 100,
  poster_path: "/poster.jpg",
  production_companies: [
    {
      id: 1,
      logo_path: null,
      name: "Test Studio",
      origin_country: "US",
    },
    {
      id: 2,
      logo_path: null,
      name: "Another Studio",
      origin_country: "UK",
    },
  ],
  production_countries: [{ iso_3166_1: "US", name: "United States" }],
  release_date: "2023-01-15",
  revenue: 5000000,
  runtime: 120,
  spoken_languages: [{ iso_639_1: "en", name: "English" }],
  status: "Released",
  tagline: "A test tagline for the movie",
  title: "Test Movie",
  video: false,
  vote_average: 7.5,
  vote_count: 1000,
};

describe("DetailPage", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("renders loading state when movie details are loading", () => {
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: null,
      isLoading: true,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Loading movie details…")).toBeInTheDocument();
  });

  it("renders error state when movie details fail to load", () => {
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: null,
      isLoading: false,
      isError: true,
      error: { message: "Failed to fetch movie details" },
    } as any);

    render(<DetailPage />, { wrapper });

    expect(
      screen.getByText(
        "Error loading movie details: Failed to fetch movie details"
      )
    ).toBeInTheDocument();
  });

  it("renders movie details when data is loaded successfully", () => {
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: mockMovieData,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Test Movie (2023)")).toBeInTheDocument();

    expect(
      screen.getByText("A test tagline for the movie")
    ).toBeInTheDocument();

    expect(screen.getByText("Action")).toBeInTheDocument();
    expect(screen.getByText("Adventure")).toBeInTheDocument();

    expect(screen.getByText("2h 00m")).toBeInTheDocument();

    expect(screen.getByText("7.5")).toBeInTheDocument();

    expect(
      screen.getByText(
        "This is a test movie overview with some interesting plot details."
      )
    ).toBeInTheDocument();

    expect(screen.getByText("Test Studio")).toBeInTheDocument();
    expect(screen.getByText("Another Studio")).toBeInTheDocument();
  });

  it("renders movie without tagline when tagline is null", () => {
    const movieDataWithoutTagline = { ...mockMovieData, tagline: null };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieDataWithoutTagline,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Test Movie (2023)")).toBeInTheDocument();
    expect(
      screen.queryByText("A test tagline for the movie")
    ).not.toBeInTheDocument();
  });

  it("renders movie without genres when genres array is empty", () => {
    const movieDataWithoutGenres = { ...mockMovieData, genres: [] };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieDataWithoutGenres,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Test Movie (2023)")).toBeInTheDocument();
    expect(screen.queryByText("Action")).not.toBeInTheDocument();
    expect(screen.queryByText("Adventure")).not.toBeInTheDocument();
  });

  it("renders runtime when genres array is empty but runtime is present", () => {
    const movieDataWithoutGenres = { ...mockMovieData, genres: [] };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieDataWithoutGenres,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Test Movie (2023)")).toBeInTheDocument();
    expect(screen.queryByText("Action")).not.toBeInTheDocument();
    expect(screen.queryByText("Adventure")).not.toBeInTheDocument();
    expect(screen.getByText("2h 00m")).toBeInTheDocument();
  });

  it("renders genres when runtime is null but genres are present", () => {
    const movieDataWithoutRuntime = { ...mockMovieData, runtime: null };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieDataWithoutRuntime,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Test Movie (2023)")).toBeInTheDocument();
    expect(screen.getByText("Action")).toBeInTheDocument();
    expect(screen.getByText("Adventure")).toBeInTheDocument();
    expect(screen.queryByText("2h 00m")).not.toBeInTheDocument();
  });

  it("renders movie without production companies when array is empty", () => {
    const movieDataWithoutProduction = {
      ...mockMovieData,
      production_companies: [],
    };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieDataWithoutProduction,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(screen.getByText("Test Movie (2023)")).toBeInTheDocument();
    expect(screen.queryByText("Production")).not.toBeInTheDocument();
    expect(screen.queryByText("Test Studio")).not.toBeInTheDocument();
  });

  it("renders movie with placeholder image when poster_path is null", () => {
    const movieDataWithoutPoster = { ...mockMovieData, poster_path: null };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieDataWithoutPoster,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    const image = screen.getByAltText("No image for Test Movie");
    expect(image).toHaveAttribute(
      "src",
      "/src/assets/no-image-placeholder.jpg"
    );
  });

  it("renders movie with poster image when poster_path is available", () => {
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: mockMovieData,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    const image = screen.getByAltText("Poster of Test Movie");
    expect(image).toHaveAttribute(
      "src",
      "https://image.tmdb.org/t/p/w500/poster.jpg"
    );
  });

  it("handles non-English movie titles correctly", () => {
    const nonEnglishMovie = {
      ...mockMovieData,
      original_language: "es",
      original_title: "Película de Prueba",
      title: "Test Movie",
    };

    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: nonEnglishMovie,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(
      screen.getByText("Película de Prueba (Test Movie) (2023)")
    ).toBeInTheDocument();
  });

  it("displays 'No release date' when release_date is null", () => {
    const movieWithoutReleaseDate = { ...mockMovieData, release_date: null };
    vi.mocked(movieDetailsHook.useMovieDetails).mockReturnValue({
      data: movieWithoutReleaseDate,
      isLoading: false,
      isError: false,
      error: null,
    } as any);

    render(<DetailPage />, { wrapper });

    expect(
      screen.getByText("Test Movie (No release date)")
    ).toBeInTheDocument();
  });
});
