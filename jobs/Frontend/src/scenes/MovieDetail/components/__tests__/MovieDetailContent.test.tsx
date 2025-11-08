import { render, screen } from "@testing-library/react";
import MovieDetailContent from "@/scenes/MovieDetail/components/MovieDetailContent";
import { testMovieDetail } from "@/jest/fakeData/movieDetail";

describe("MovieDetailContent", () => {
  beforeEach(() => {
    render(<MovieDetailContent movie={testMovieDetail} />);
  });
  it("renders title and tagline", () => {
    expect(screen.getByRole("heading", { name: "Test Movie" })).toBeVisible();
    expect(screen.getByText("Test tagline")).toBeVisible();
  });
  it("renders overview", () => {
    expect(screen.getByText("Test overview")).toBeVisible();
  });
  it("renders release date", () => {
    expect(screen.getByText("Released on 1/1/2021")).toBeVisible();
  });
  it("renders runtime", () => {
    expect(screen.getByText("100 minutes")).toBeVisible();
  });
  it("renders genres", () => {
    expect(screen.getByText("Test Genre")).toBeVisible();
    expect(screen.getByText("Test Genre 2")).toBeVisible();
  });
  it("renders imdb link", () => {
    expect(screen.getByRole("link", { name: "IMDB" })).toHaveAttribute(
      "href",
      "https://www.imdb.com/title/tt123456",
    );
  });
  it("renders homepage link", () => {
    expect(screen.getByRole("link", { name: "Homepage" })).toHaveAttribute(
      "href",
      "https://www.test.com",
    );
  });
});
