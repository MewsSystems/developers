import React from "react";
import { MemoryRouter } from "react-router-dom";
import { renderWithProviders } from "../../testing/mockProvider";

import { mockState } from "../../testing/mockState";

import { MovieDetail } from "./MovieDetail";
import { mockMovieDetail } from "../../public-api";

describe("<MovieDetail />", () => {
  it("should render on the screen", () => {
    const { asFragment } = renderWithProviders(
      <MemoryRouter>
        <MovieDetail />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: mockState,
        },
      }
    );
    cy.mount(asFragment);
  });

  it("should render all its components", () => {
    renderWithProviders(
      <MemoryRouter>
        <MovieDetail />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: {
            ...mockState,
            movieDetail: mockMovieDetail,
            movieDetailId: mockMovieDetail.id.toString(),
          },
        },
      }
    );
    cy.get("h1").should("have.text", "Fight Club");

    cy.get('[data-test="tag"]').should("have.text", "Drama");
    cy.get('[data-test="subtitle"]').should(
      "have.text",
      mockMovieDetail.tagline
    );
    cy.get('[data-test="overview"]').should(
      "have.text",
      mockMovieDetail.overview
    );

    cy.get('[data-test="rating"] span').eq(0).should("have.text", "7.8 / 10");
    cy.get('[data-test="rating"] span').eq(1).should("have.text", "1 %");
  });
});
