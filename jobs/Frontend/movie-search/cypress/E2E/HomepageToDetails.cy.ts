describe("Homepage to Details Page", () => {
  beforeEach(() => {
    cy.visit("/"); // uses baseUrl from cypress.config.ts
  });

  it("renders the movie search input", () => {
    cy.get('[data-testid="MovieSearchInput"]').should("be.visible");
  });

  it("searches for a movie", () => {
    // Type 'test' into the search input
    cy.get('[data-testid="MovieSearchInput"]').type("Star Wars");

    // Check if the first movie in the list contains 'star wars' in the title
    cy.get('[data-testid="movieTitle0"]')
      .first()
      .should("contain", "Star Wars");

    // Click on the first movie
    cy.get('[data-testid="movieTitle0"]').first().click();

    // Check if the details page is rendered
    cy.get('[data-testid="movieDetails"]').should("be.visible");
  });

  it("searches for a movie and goes back to the homepage", () => {
    // Type 'Star Wars' into the search input
    cy.get('[data-testid="MovieSearchInput"]').type("Star Wars");

    // Check if the first movie in the list contains 'Star wars' in the title
    cy.get('[data-testid="movieTitle0"]')
      .first()
      .should("contain", "Star Wars");

    // Click on the first movie
    cy.get('[data-testid="movieTitle0"]').first().click();

    // Check if the details page is rendered
    cy.get('[data-testid="movieDetails"]').should("be.visible");

    // Click on the back button
    cy.get('[data-testid="MovieDetailsBackButton"]').click();

    // Check if the homepage is rendered
    cy.get('[data-testid="MovieSearchInput"]').should("be.visible");
  });
});
