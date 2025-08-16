import React from "react";
import { render, screen, waitFor } from "@testing-library/react";
import { Wrapper } from "../../../../../mocks/Wrapper";
import { server } from "../../../../../mocks/server";
import { movieDetailHandlers } from "../../../../../mocks/handlers";
import { MovieDetails } from "./MovieDetails";

describe("<MovieDetails />", () => {
  it("should render the component correctly when data is returned", async () => {
    render( <MovieDetails id={1} />, { wrapper: Wrapper });

    expect(screen.getByRole("progressbar")).toBeInTheDocument();

    await waitFor (() => {
      expect(screen.getByRole("heading", 
        { name: "The Lord of the Rings: The Two Towers" }
      )).toBeInTheDocument();
    });

    expect(screen.getByText("Adventure")).toBeInTheDocument();
    expect(screen.getByText("Fantasy")).toBeInTheDocument();
    expect(screen.getByText("Action")).toBeInTheDocument();

    expect(screen.getByText("Release date: 2002-12-18")).toBeInTheDocument();
    expect(screen.getByText("They are taking the Hobbits to Isengard")).toBeInTheDocument();
  });

  it("should render the component correctly when no data is returned", async () => {
    server.use(movieDetailHandlers.noDataHandler);

    render(<MovieDetails id={1} />, { wrapper: Wrapper });

    await waitFor (() => {
      expect(screen.getByText("No data was found")).toBeInTheDocument();
    });
  });

  it("should render the component correctly when an error is returned", async () => {
    server.use(movieDetailHandlers.errorHandler);

    render(<MovieDetails id={1} />, { wrapper: Wrapper });

    await waitFor (() => {
      expect(screen.getByText("Something went wrong")).toBeInTheDocument();
    });
  });
});
