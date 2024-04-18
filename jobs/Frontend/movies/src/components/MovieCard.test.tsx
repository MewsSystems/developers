import { render } from "@testing-library/react";
import { MovieCard } from "./MovieCard";
import { movieResultMock } from "../services/movies/movies.factory";

describe("MovieCard", () => {
  it("render movie card", () => {
    const movie = movieResultMock.build();
    const screen = render(<MovieCard movie={movie} />);

    expect(screen.getByText(/harry potter and the chamber of secrets/i)).toBeInTheDocument();
    expect(screen.getByText(/cars fly/i)).toBeInTheDocument();
    expect(screen.getByText(/7.7/i)).toBeInTheDocument();
  });
});
