import { fireEvent, render } from "@/tests";
import { MovieCard, MovieCardProps } from "./MovieCard";

describe("MovieCard", () => {
  const renderMovieCard = (props?: Partial<MovieCardProps>) => {
    return render(
      <MovieCard
        id={123}
        title="Title"
        releaseDate="2002-22-12"
        description="Description"
        genres={["Thriller", "Action"]}
        rating={4}
        imgPath={"imgPath"}
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

    expect(imageElem).toHaveAttribute("src", "imgPath");
  });

  it("displays a fallback image when the movie poster fails to load", () => {
    const { getByText } = renderMovieCard({ imgPath: null });

    expect(getByText("Image loading failed")).toBeInTheDocument();
  });

  it("navigates to the correct movie details page when the 'See more' button is clicked", () => {
    const { getByText } = renderMovieCard();

    const btnElem = getByText("See more");
    fireEvent.click(btnElem);

    expect(window.location.pathname).toBe("/movie/123");
  });
});
