describe("SearchPage", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("allows the user to search for a movie and see results", () => {
    cy.get('[data-testid="search-input"]').should("be.visible");

    cy.get('[data-testid="search-input"]').type("Inception");

    cy.get('[data-testid="loading-indicator"]').should("be.visible");
    cy.get('[data-testid="loading-indicator"]').should("not.exist");

    cy.get('[data-testid="movie-list"]')
      .find('[data-testid="movie-card"]')
      .should("have.length.at.least", 1);

    cy.contains("Inception").should("be.visible");
  });

  it("navigates to the next page and fetches more movies", () => {
    cy.get('[data-testid="search-input"]').type("Lord");

    cy.get('[data-testid="loading-indicator"]').should("not.exist");

    cy.get('[data-testid="next-page-button"]').click();

    cy.get('[data-testid="loading-indicator"]').should("not.exist");

    cy.get('[data-testid="movie-list"]')
      .find('[data-testid="movie-card"]')
      .should("have.length.at.least", 1);
  });

  it("navigates to the detail page when a movie card is clicked", () => {
    cy.get('[data-testid="search-input"]').type("Inception");

    cy.get('[data-testid="loading-indicator"]').should("not.exist");

    cy.get('[data-testid="movie-card"]').first().click();

    cy.url().should("include", "/movie/");
  });
});

export {};
