describe("Home", () => {
  beforeEach(() => {
    cy.visit("/"); // uses baseUrl from cypress.config.ts
  });

  it("renders the movie search input", () => {
    cy.get('input[type="search"]').should("be.visible");
  });

  it("searches for a movie", () => {
    // Type 'test' into the search input
    cy.get('input[type="search"]').type("test");

    // Check if the first movie in the list contains 'test' in the title
    cy.get('[data-testid="movieTitle0"]').first().should("contain", "Test");
  });
});
