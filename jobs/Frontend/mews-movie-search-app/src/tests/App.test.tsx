import { render, screen } from "@testing-library/react";
import App from "../App";
import * as useMovies from "../useMovies";

const useMoviesSpy = vi.spyOn(useMovies, "useMovies");

describe("App", () => {
  afterAll(() => {
    vi.clearAllMocks();
  });

  function Arrange() {
    render(<App />);
  }

  it("renders the headline", async () => {
    Arrange();
    const headline = await screen.findByText("Mews Movie Search App");
    expect(headline).toBeDefined();
  });
  it("renders the input search", async () => {
    Arrange();
    const input = await screen.findByLabelText("Search a movie");
    expect(input).toBeDefined();
  });
  it("renders a list of movies", async () => {
    useMoviesSpy.mockReturnValue({
      movies: {
        page: 1,
        total_pages: 1,
        total_results: 1,
        results: [
          {
            adult: false,
            backdrop_path: "/xOMo8BRK7PfcJv9JCnx7s5hj0PX.jpg",
            genre_ids: [878, 12],
            id: 693134,
            original_language: "en",
            original_title: "Dune: Part Two",
            overview:
              "Follow the mythic journey of Paul Atreides as he unites with Chani and the Fremen while on a path of revenge against the conspirators who destroyed his family. Facing a choice between the love of his life and the fate of the known universe, Paul endeavors to prevent a terrible future only he can foresee.",
            popularity: 2938.734,
            poster_path: "/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
            release_date: "2024-02-27",
            title: "Dune: Part Two",
            video: false,
            vote_average: 8.3,
            vote_count: 2958,
          },
        ],
      },
    });

    Arrange();

    const list = await screen.findByRole("list");
    expect(list).toBeDefined();
    const movies = await screen.findAllByRole("listitem");
    expect(movies).toHaveLength(1);
  });
});
