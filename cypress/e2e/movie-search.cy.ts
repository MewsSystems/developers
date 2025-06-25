/// <reference types="cypress" />

describe("Movie Search Application with MSW", () => {
  beforeEach(() => {
    cy.visit("/")
    cy.contains("TMDB Movie Search").should("be.visible")
  })

  it("should load popular movies", () => {
    cy.contains("TMDB Movie Search").should("be.visible")
    cy.contains("Popular Movies").should("be.visible")

    cy.get('[data-testid="movie-card"]', { timeout: 10000 }).should("have.length.greaterThan", 0)

    cy.contains("Final Destination Bloodlines").should("be.visible")
  })

  it("should complete a full user journey", () => {
    cy.get('[data-testid="movie-card"]', { timeout: 10000 }).should("have.length.greaterThan", 0)

    cy.get('[data-testid="movie-card"]').first().click()

    cy.url().should("include", "/movie/")

    cy.get("button").contains("Back to Search").should("be.visible")
    cy.get("h1").should("be.visible").and("not.be.empty")
    cy.get('[data-testid="movie-rating"]').should("be.visible")

    cy.get("button").contains("Back to Search").click()
    cy.url().should("include", Cypress.config().baseUrl)
    cy.contains("Popular Movies").should("be.visible")
  })

  it("should handle search functionality", () => {
    cy.get('input[placeholder="Search for movies..."]').type("final destination")

    cy.contains('Search Results for "final destination"', { timeout: 10000 }).should("be.visible")

    cy.get('[data-testid="movie-card"]').should("have.length.greaterThan", 0)
    cy.contains("Final Destination Bloodlines").should("be.visible")
  })

  it("should handle empty search results", () => {
    cy.get('input[placeholder="Search for movies..."]').type("nonexistentmovie123")

    cy.contains("No movies found. Try a different search term.", { timeout: 10000 }).should(
      "be.visible"
    )
  })

  it("should handle pagination", () => {
    cy.get('[data-testid="pagination"]', { timeout: 10000 }).should("be.visible")

    cy.contains("Page 1 of 500").should("be.visible")
  })

  it("should clear search and return to popular movies", () => {
    cy.get('input[placeholder="Search for movies..."]').type("final destination")
    cy.contains('Search Results for "final destination"', { timeout: 10000 }).should("be.visible")

    cy.get('input[placeholder="Search for movies..."]').clear()

    cy.contains("Popular Movies", { timeout: 10000 }).should("be.visible")
    cy.get('[data-testid="movie-card"]').should("have.length.greaterThan", 0)
  })
})
