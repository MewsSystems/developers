import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";

import MovieImg from "./MovieImg";

describe("MovieImg", () => {
  it("renders MovieImg component without poster", () => {
    render(<MovieImg />);

    const imgElement = screen.getByTestId("movie-img");

    expect(imgElement).toBeInTheDocument();
    expect(imgElement).toHaveStyle("height: 200px");
    expect(imgElement).toHaveStyle("width: 300px");
    expect(imgElement).toHaveStyle(
      "backgroundImage: linear-gradient(to right bottom, #02011D, #474860)",
    );
  });

  it("renders MovieImg component with provided posterPath", () => {
    const posterPath = "/example-poster.jpg";

    render(<MovieImg posterPath={posterPath} />);

    const imgElement = screen.getByTestId("movie-img");

    expect(imgElement).toBeInTheDocument();
    expect(imgElement).toHaveStyle(
      `backgroundImage: url('https://media.themoviedb.org/t/p/w220_and_h330_face${posterPath}')`,
    );
  });

  it("renders MovieImg component with isDetail prop", () => {
    render(<MovieImg isDetail />);

    const imgElement = screen.getByTestId("movie-img");

    expect(imgElement).toBeInTheDocument();
    expect(imgElement).toHaveStyle("height: 600px");
    expect(imgElement).toHaveStyle("width: 400px");
  });
});
