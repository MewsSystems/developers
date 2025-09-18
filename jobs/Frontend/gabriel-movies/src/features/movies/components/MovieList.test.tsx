import { describe, it, expect } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { MOVIE_INCEPTION, MOVIE_INTERSTELLAR } from "../fixtures/movies";
import { MovieList } from "./MovieList";

describe("MovieList", () => {
  it("renders two movie cards", () => {
    render(<MovieList items={[MOVIE_INCEPTION, MOVIE_INTERSTELLAR]} />);

    expect(screen.getByRole("list")).toBeInTheDocument();
    expect(screen.getAllByRole("img").length).toBe(2);
    expect(screen.getAllByRole("link").length).toBe(2);
  });
});
