import React from "react";
import { MemoryRouter } from "react-router-dom";
import { mockMovies, mockState, renderWithProviders } from "../../public-api";
import { MoviesList } from "../public-api";

describe("<MoviesList />", () => {
  it("should render on the screen", () => {
    const { asFragment } = renderWithProviders(
      <MemoryRouter>
        <MoviesList />
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
        <MoviesList />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: {
            ...mockState,
            movies: mockMovies,
            results: 2,
            totalPages: 1,
          },
        },
      }
    );

    cy.get("h2").should("have.text", "2 Results");

    cy.get("button").should("have.length", 1);

    cy.get("li").should("have.length", 2);

    cy.get("li h3").eq(0).should("have.text", "The Avengers");
    cy.get("li h3").eq(1).should("have.text", "The Lama Avenger");

    cy.get('li span[data-test="release"]').eq(0).should("have.text", "2012");
    cy.get('li span[data-test="release"]').eq(1).should("have.text", "1979");
  });
});
