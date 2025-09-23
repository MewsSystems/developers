import { render, screen, waitFor } from "@testing-library/react";
import { vi } from "vitest";
import { MemoryRouter } from "react-router-dom";
import MovieDetail from "./MovieDetail";
import { fetchMovieDetail } from "../api/tmdb";
import { useParams } from "react-router-dom";

const mockFetchMovieDetail = vi.mocked(fetchMovieDetail);
const mockUseParams = vi.mocked(useParams);

describe("MovieDetail", () => {
  const mockMovieData = {
    id: 1,
    title: "Avengers: Endgame",
    overview:
      "The Avengers assemble once more to reverse the damage caused by Thanos.",
    release_date: "2019-04-26",
    vote_average: 8.4,
    poster_path: "/or06FN3Dka5tukK1e9sl16pB3iy.jpg",
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  const renderWithRouter = (component: React.ReactElement) => {
    return render(<MemoryRouter>{component}</MemoryRouter>);
  };

  it("shows loader when movie data is not loaded", () => {
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockImplementation(() => new Promise(() => {})); // Never resolves

    renderWithRouter(<MovieDetail />);

    expect(screen.getByTestId("loader")).toBeInTheDocument();
    expect(screen.getByTestId("layout")).toBeInTheDocument();
  });

  it("renders movie details when data is loaded", async () => {
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockResolvedValue(mockMovieData);

    renderWithRouter(<MovieDetail />);

    await waitFor(() => {
      expect(screen.getByText("Avengers: Endgame")).toBeInTheDocument();
    });

    expect(
      screen.getByText(
        "The Avengers assemble once more to reverse the damage caused by Thanos."
      )
    ).toBeInTheDocument();
    expect(screen.getByText("2019-04-26")).toBeInTheDocument();
    expect(screen.getByText("8.4")).toBeInTheDocument();
    expect(screen.getByText("Back to search")).toBeInTheDocument();
  });

  it("shows poster image when poster_path exists", async () => {
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockResolvedValue(mockMovieData);

    renderWithRouter(<MovieDetail />);

    await waitFor(() => {
      expect(screen.getByTestId("poster-image")).toBeInTheDocument();
    });

    const posterImage = screen.getByTestId("poster-image");
    expect(posterImage).toHaveAttribute(
      "src",
      "/or06FN3Dka5tukK1e9sl16pB3iy.jpg"
    );
    expect(posterImage).toHaveAttribute("alt", "Avengers: Endgame");
  });

  it("shows placeholder when poster_path is null", async () => {
    const movieWithoutPoster = { ...mockMovieData, poster_path: null };
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockResolvedValue(movieWithoutPoster);

    renderWithRouter(<MovieDetail />);

    await waitFor(() => {
      expect(screen.getByTestId("placeholder")).toBeInTheDocument();
    });

    expect(screen.getByText("No Image Available")).toBeInTheDocument();
    expect(screen.queryByTestId("poster-image")).not.toBeInTheDocument();
  });

  it("does not fetch movie when id is not provided", () => {
    mockUseParams.mockReturnValue({}); // No id

    renderWithRouter(<MovieDetail />);

    expect(mockFetchMovieDetail).not.toHaveBeenCalled();
    expect(screen.getByTestId("loader")).toBeInTheDocument();
  });

  it("fetches movie with correct id", async () => {
    mockUseParams.mockReturnValue({ id: "123" });
    mockFetchMovieDetail.mockResolvedValue(mockMovieData);

    renderWithRouter(<MovieDetail />);

    expect(mockFetchMovieDetail).toHaveBeenCalledWith("123");
    expect(mockFetchMovieDetail).toHaveBeenCalledTimes(1);
  });

  it("renders back to search link with correct href", async () => {
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockResolvedValue(mockMovieData);

    renderWithRouter(<MovieDetail />);

    await waitFor(() => {
      const backLink = screen.getByText("Back to search");
      expect(backLink).toHaveAttribute("href", "/");
    });
  });

  it("renders all movie detail sections", async () => {
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockResolvedValue(mockMovieData);

    renderWithRouter(<MovieDetail />);

    await waitFor(() => {
      expect(screen.getByText("Overview:")).toBeInTheDocument();
      expect(screen.getByText("Release Date:")).toBeInTheDocument();
      expect(screen.getByText("Rating:")).toBeInTheDocument();
      expect(screen.getByTestId("movie-text-container")).toBeInTheDocument();
    });
  });

  it("refetches movie when id changes", async () => {
    mockUseParams.mockReturnValue({ id: "1" });
    mockFetchMovieDetail.mockResolvedValue(mockMovieData);

    const { rerender } = renderWithRouter(<MovieDetail />);

    await waitFor(() => {
      expect(mockFetchMovieDetail).toHaveBeenCalledWith("1");
    });

    // Change the id
    mockUseParams.mockReturnValue({ id: "2" });
    mockFetchMovieDetail.mockResolvedValue({
      ...mockMovieData,
      id: 2,
      title: "Another Movie",
    });

    rerender(
      <MemoryRouter>
        <MovieDetail />
      </MemoryRouter>
    );

    await waitFor(() => {
      expect(mockFetchMovieDetail).toHaveBeenCalledWith("2");
    });

    expect(mockFetchMovieDetail).toHaveBeenCalledTimes(2);
  });
});
