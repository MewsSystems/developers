import { render, screen } from "../../../test/utils";
import { SearchResults } from "./SearchResults";

describe("SearchResults", () => {
  it("Search results is empty with no movies", async () => {
    render(<SearchResults />);
    expect(screen.queryByText(/Search results for/i)).toBeNull();
  });

  it("Search results shows 2 movies", async () => {
    render(<SearchResults />, {
      state: {
        movies: {
          search: {
            results: [
              {
                adult: false,
                backdrop_path: "/g4a5YLWwi6OCp8TcvxsUNrXMbN5.jpg",
                genre_ids: [878, 28, 53, 12],
                id: 87101,
                original_language: "en",
                original_title: "Terminator Genisys",
                overview:
                  "The year is 2029. John Connor, leader of the resistance continues the war against the machines. At the Los Angeles offensive, John's fears of the unknown future begin to emerge when TECOM spies reveal a new plot by SkyNet that will attack him from both fronts; past and future, and will ultimately change warfare forever.",
                popularity: 117.228,
                poster_path: "/oZRVDpNtmHk8M1VYy1aeOWUXgbC.jpg",
                release_date: "2015-06-23",
                title: "Terminator Genisys",
                video: false,
                vote_average: 5.935,
                vote_count: 8098,
              },
            ],
            page: 0,
            total_pages: 3,
            loading: false,
          },
          query: "terminator",
          selectedMovie: null,
        },
      },
      withRouter: true,
    });
    expect(screen.queryByText(/Search results for/i)).toBeInTheDocument();
    expect(screen.queryByText(/Terminator Genisys/i)).toBeInTheDocument();
  });
});
