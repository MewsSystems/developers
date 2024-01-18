import { describe, expect, test, vi } from "vitest";
import MoviesMother from "../backoffice/movies/movies.mother";
import { render, screen } from "@testing-library/react";
import movies from "../../src/services/movies";
import MoviesDetails from "@/components/MoviesDetailsView/MoviesDetails";

describe("Movie Card", () => {
  test("should render modal with movie info", () => {
    //given
    const movie = MoviesMother.random();
    console.log(movie);
    vi.spyOn(movies, "getOne").mockResolvedValue(movie);

    //when
    render(<MoviesDetails movieDetails={movie} />);
    const movieTitle = screen.getByRole("heading", { level: 1 });
    console.log(movieTitle);

    //expect
    expect(movieTitle.textContent).toMatch(movie.original_title);
  });
});
