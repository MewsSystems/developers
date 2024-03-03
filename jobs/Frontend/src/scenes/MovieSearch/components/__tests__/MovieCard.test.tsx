import { render, screen } from "@testing-library/react";
import MovieCard from "@/scenes/MovieSearch/components/MovieCard";
import { Movie } from "@/scenes/MovieSearch/services/types";
import { testMovie } from "@/jest/fakeData/movie";

describe("MovieCard", () => {
  it("shows correct information", () => {
    render(<MovieCard movie={testMovie} />);

    expect(screen.getByText("Test Movie")).toBeVisible();
    expect(screen.getByText("2021")).toBeVisible();
    expect(screen.getByRole("img", { name: "Test Movie" })).toBeVisible();
    expect(screen.getByRole("img", { name: "Test Movie" })).toHaveAttribute(
      "src",
      "https://image.tmdb.org/t/p/w500test.jpg",
    );
  });

  it("shows correct information with no poster", () => {
    render(
      <MovieCard
        movie={{
          ...testMovie,
          poster_path: null,
        }}
      />,
    );

    expect(screen.getByText("Test Movie")).toBeVisible();
    expect(screen.getByText("2021")).toBeVisible();
    expect(screen.queryByRole("img")).not.toBeInTheDocument();
  });

  it("links to the correct detail page", () => {
    render(<MovieCard movie={testMovie} />);
    const link = screen.getByRole("link", {
      name: "Test Movie 2021 Test Movie",
    });
    expect(link).toHaveAttribute("href", "/detail/1");
  });
});
