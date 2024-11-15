/// <reference types="cypress" />

describe("Primary use cases", () => {
  beforeEach(() => {
    // intercept the call to the API to return a fixed response
    cy.intercept(
      "GET",
      "https://api.themoviedb.org/3/search/movie?api_key=**&query=matrix&page=1",
      {
        fixture: "searchMatrix.json",
      },
    ).as("searchMatrix");

    cy.intercept("GET", "https://api.themoviedb.org/3/movie/603?api_key=**", {
      fixture: "movie603.json",
    }).as("movie603");

    cy.visit("http://localhost:5173");
  });
  it("search for a movie and see the detail page", () => {
    // select the input element and type a value
    cy.get("input").type("matrix");

    // wait for the search results to appear
    // several div with the data-test attribute set to "card" should be displayed
    cy.get("[data-test=card]").should("have.length.gt", 0);

    // click on the first card containing the text "The Matrix"
    cy.get("[data-test=card]").contains("The Matrix").click();

    // check that the div containing the overview text is displayed
    cy.get("[data-test=overview]").contains(
      "tells the story of a computer hacker who joins a group",
    );
  });
  it("should display 20 additional movies when click on the load more button", () => {
    // select the input element and type a value
    cy.get("input").type("matrix");

    // wait for the search results to appear
    // several div with the data-test attribute set to "card" should be displayed
    // Then check that there is 20 cards
    cy.get("[data-test=card]").should("have.length", 20);

    // click on the load more button
    cy.get("[data-test=load-more]").click();

    // check that there is 40 cards
    cy.get("[data-test=card]").should("have.length", 40);
  });
  it("should empty the search results when the input is empty", () => {
    // select the input element and type a value
    cy.get("input").type("matrix");

    // wait for the search results to appear
    // several div with the data-test attribute set to "card" should be displayed
    cy.get("[data-test=card]").should("have.length.gt", 0);

    // clear the input
    cy.get("input").clear();

    // check that the search results are empty
    cy.get("[data-test=card]").should("have.length", 0);

    // check that the message "Please, type a movie title to start!" is displayed
    cy.contains("Please, type a movie title to start!");
  });
  describe("Error handling", () => {
    describe("Search page", () => {
      it("should display an error message when the API call fails", () => {
        // intercept the call to the API to return a 500 error
        cy.intercept(
          "GET",
          "https://api.themoviedb.org/3/search/movie?api_key=**&query=matrix&page=1",
          {
            statusCode: 500,
            body: "Internal Server Error",
          },
        ).as("searchMatrix");

        // select the input element and type a value
        cy.get("input").type("matrix");

        // wait for the error message to appear
        cy.contains("Error: Impossible to fetch the data");
      });
    });
    describe("Detail page", () => {
      it("should display an error message when the API call fails", () => {
        // intercept the call to the API to return a 500 error
        cy.intercept(
          "GET",
          "https://api.themoviedb.org/3/movie/603?api_key=**",
          {
            statusCode: 500,
            body: "Internal Server Error",
          },
        ).as("movie603");

        cy.visit("http://localhost:5173/movie/603");

        // check that the error message is displayed
        cy.contains("Error: Impossible to fetch the data");
      });
    });
  });
});
