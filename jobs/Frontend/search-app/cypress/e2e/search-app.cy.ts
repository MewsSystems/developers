describe("search-app", () => {
  beforeEach(() => {
    cy.visit("https://search-app-ste.herokuapp.com/");
  });

  it("should fetch search results", () => {
    // checks search movie results
    cy.get('input[type="text"]').type("green").should("have.value", "green");
    cy.get("h2").should("be.visible");
    cy.get("button").should("have.length.gte", 1);
    cy.get("li").should("have.length.gte", 1);

    // clearing search removes all results
    cy.get('input[type="text"]').clear();
    cy.get("h2").should("not.exist");
    cy.get("button").should("not.exist");
    cy.get("li").should("not.exist");
  });

  it("should navigate to movie detail view", () => {
    cy.get('input[type="text"]')
      .type("forrest gump")
      .should("have.value", "forrest gump");

    // clicking on a search result opens a movie detail view
    cy.get("li h3").eq(0).should("have.text", "Forrest Gump").click();
    cy.get("h1").should("have.text", "Forrest Gump");

    // clicking on a back button should return back to search results
    cy.get("i").eq(0).click();
    cy.get('input[type="text"]').should("have.value", "forrest gump");
    cy.get("li h3").eq(0).should("have.text", "Forrest Gump");
  });
});
