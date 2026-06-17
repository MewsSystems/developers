import { render } from "@testing-library/react";
import { MovieDetail } from "./MovieDetail";
import { movieMock } from "../services/movies/movies.factory";

describe("MovieDetail", () => {
  it("render a movie", () => {
    const movie = movieMock.build();
    const screen = render(<MovieDetail movie={movie} />);

    expect(
      screen.getByText(/harry potter and the chamber of secrets/i),
    ).toBeInTheDocument();
    expect(screen.getByText(/cars fly/i)).toBeInTheDocument();
    expect(screen.getByText(/7.7/i)).toBeInTheDocument();
    expect(screen.getByText(/161 min/i)).toBeInTheDocument();
    expect(screen.getByText(/warner/i)).toBeInTheDocument();
    expect(screen.getByText(/fantasy/i)).toBeInTheDocument();
  });
});
