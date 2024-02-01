import {  describe, expect, it } from "vitest";

import {  screen } from "@testing-library/react";
import { renderWithClient } from "../../../tests/testUtils";
import { QueryClient } from "@tanstack/react-query";

import "@testing-library/jest-dom";
import { MovieDetail } from "./MovieDetail";

describe("MovieDetail", () => {
  const queryClient = new QueryClient();


  it("shows movie details", async () => {
    renderWithClient(queryClient, <MovieDetail selectedMovieId={120} />);

    const titleHeader = await screen.findByText("Original Title:");
    expect(titleHeader).toBeInTheDocument();
    // Could check the other elements but no need for the demo
  });
});
