import { render } from "@/tests";
import { MovieCard, MovieCardProps } from "./MovieCard";
import { Movie } from "tmdb-ts";
import { MEDIA_300_BASE_URL } from "@/tmdbClient";

const movieMock: Movie = {
  id: 123,
  poster_path: "/imgPath",
  adult: false,
  overview: "Description",
  release_date: "2002-22-12",
  genre_ids: [1, 2],
  original_title: "Original Title",
  original_language: "en",
  title: "Title",
  backdrop_path: "backdropPath",
  popularity: 7.5,
  vote_count: 100,
  video: false,
  vote_average: 8.5,
};

describe("MovieCard", () => {
  const renderMovieCard = (props?: Partial<MovieCardProps>) => {
    return render(
      <MovieCard
        movie={{ ...movieMock, ...props?.movie }}
        genres={["Thriller", "Action"]}
        {...props}
      />,
    );
  };

  it("displays the correct text content", () => {
    const { queryByText } = renderMovieCard();

    expect(queryByText("Title")).toBeInTheDocument();
    expect(queryByText("Release date: 2002-22-12")).toBeInTheDocument();
    expect(queryByText("Description")).toBeInTheDocument();
    expect(queryByText("Thriller")).toBeInTheDocument();
    expect(queryByText("Action")).toBeInTheDocument();
  });

  it("displays the correct movie poster", () => {
    const { getByAltText } = renderMovieCard();
    const imageElem = getByAltText("Movie poster");

    expect(imageElem).toHaveAttribute("src", MEDIA_300_BASE_URL + "/imgPath");
  });

  it("displays a fallback image when the movie poster fails to load", () => {
    const { getByRole } = renderMovieCard({ movie: { ...movieMock, poster_path: "" } });

    expect(getByRole("img")).toHaveAttribute("src", "/src/assets/mocks/fallback.jpg");
  });

  it("navigates to the correct movie details page when the 'See more' button is clicked", () => {
    const { getByText } = renderMovieCard();

    expect(getByText("See more")).toHaveAttribute("href", "/movie/123");
  });
});
