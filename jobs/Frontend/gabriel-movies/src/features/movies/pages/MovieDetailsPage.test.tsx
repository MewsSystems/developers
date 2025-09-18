import { describe, it, expect, vi, beforeEach } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { MOVIE_INTERSTELLAR } from "../fixtures/movies";
import MovieDetailsPage from "./MovieDetailsPage";

vi.mock("react-router-dom", async () => {
  const actual = await vi.importActual<typeof import("react-router-dom")>("react-router-dom");
  return {
    ...actual,
    useParams: () => ({ id: "123" })
  };
});

vi.mock("../components/UserScore", () => ({
  UserScore: () => (
    <div data-testid="user-score" />
  )
}));

const useFetchMovieMock = vi.fn();
vi.mock("../hooks/useFetchMovie", () => ({
  useFetchMovie: (id: string | undefined) => useFetchMovieMock(id)
}));

describe("MovieDetailsPage", () => {
  beforeEach(() => {
    vi.resetAllMocks();
  });

  it("renders movie details", () => {
    useFetchMovieMock.mockReturnValue({ data: MOVIE_INTERSTELLAR });

    render(<MovieDetailsPage />);

    expect(useFetchMovieMock).toHaveBeenCalledWith("123");

    const img = screen.getByRole("img", { name: /interstellar poster/i }) as HTMLImageElement;
    expect(img).toBeInTheDocument();
    expect(img).toHaveAttribute("src", MOVIE_INTERSTELLAR.posterPath);

    expect(screen.getByRole("heading", { name: MOVIE_INTERSTELLAR.title })).toBeInTheDocument();
    expect(screen.getByText(MOVIE_INTERSTELLAR.formattedReleaseDate)).toBeInTheDocument();
    expect(screen.getByText(MOVIE_INTERSTELLAR.overview)).toBeInTheDocument();
    expect(screen.getByTestId("user-score")).toBeInTheDocument()
  });
});
