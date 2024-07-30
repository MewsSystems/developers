import { describe, it, expect, vi } from "vitest";

import { MovieDetail } from "../MovieDetail.page";
import { render, screen, waitFor } from "@/testing-utils/render.utils";
import { mockMovie } from "@/testing-utils/fixtures.utils";
import { tmdb } from "@/app/services/tmdb";
import { Route } from "react-router-dom";

describe("MovieDetail", () => {
  it("fetches and renders the movie details", async () => {
    // Not an ideal type defintion, but vitest seems to me complaining
    const mockDetailsEndpoint = vi
      .spyOn(tmdb.movies, "details" as never)
      .mockResolvedValue(mockMovie);

    render(<Route path="/movie/:id" component={MovieDetail} />, {
      currentRoute: `/movie/${mockMovie.id}`,
    });

    // Api called
    await waitFor(() =>
      expect(mockDetailsEndpoint).toHaveBeenCalledWith(mockMovie.id, [
        "credits",
      ]),
    );

    const movieLabel = `(${new Date(mockMovie.release_date).getFullYear()})`;
    const genreLabel = mockMovie.genres.map((genre) => genre.name).join(", ");

    // Check that section titles are rendered
    expect(screen.getByLabelText("Overview")).toBeInTheDocument();
    expect(screen.getByLabelText("Audience score")).toBeInTheDocument();
    expect(screen.getByLabelText("Directed by")).toBeInTheDocument();
    expect(screen.getByLabelText("Stars")).toBeInTheDocument();

    // Check that information is displayed
    expect(screen.getByText(mockMovie.title)).toBeInTheDocument();
    expect(screen.getByText(movieLabel)).toBeInTheDocument();
    expect(screen.getByText(mockMovie.overview)).toBeInTheDocument();
    expect(screen.getByText(mockMovie.origin_country[0])).toBeInTheDocument();
    expect(screen.getByText(genreLabel)).toBeInTheDocument();

    // Check link
    // Reconstructing the final label was a bit tough, so we use hardcoded values
    expect(
      screen.getByRole("link", {
        name: getLinkLabel("Lana Wachowski"),
      }),
    ).toHaveTextContent("Lana Wachowski");
    expect(
      screen.getByRole("link", {
        name: getLinkLabel("Lilly Wachowski"),
      }),
    ).toHaveTextContent("Lilly Wachowski");
    expect(
      screen.getByRole("link", {
        name: getLinkLabel("Keanu Reeves"),
      }),
    ).toHaveTextContent("Keanu Reeves");
    expect(
      screen.getByRole("link", {
        name: getLinkLabel("Laurence Fishburne"),
      }),
    ).toHaveTextContent("Laurence Fishburne");
    expect(
      screen.getByRole("link", {
        name: getLinkLabel("Carrie-Anne Moss"),
      }),
    ).toHaveTextContent("Carrie-Anne Moss");

    // Check that the "More information" link is rendered with the correct href
    expect(screen.getByText("More information")).toHaveAttribute(
      "href",
      `https://www.themoviedb.org/movie/${mockMovie.id}`,
    );
  });
});

function getLinkLabel(name: string) {
  return `See more information about ${name}`;
}
