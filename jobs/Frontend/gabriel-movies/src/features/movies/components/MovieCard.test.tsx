import { describe, it, expect } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { MOVIE_INTERSTELLAR } from "../fixtures/movies";
import { MovieCard } from "./MovieCard";

describe("MovieCard", () => {
  it("renders poster image and link", () => {
    const movie = MOVIE_INTERSTELLAR;

    render(<MovieCard movie={MOVIE_INTERSTELLAR} />);

    const img = screen.getByRole("img", { name: /interstellar poster/i }) as HTMLImageElement;
    expect(img).toBeInTheDocument();
    expect(img).toHaveAttribute("src", movie.posterPath);

    const link = screen.getByRole("link", { name: /interstellar poster/i }) as HTMLAnchorElement;
    expect(link.href.endsWith(`/movie/${movie.id}`)).toBe(true);
  });
});
