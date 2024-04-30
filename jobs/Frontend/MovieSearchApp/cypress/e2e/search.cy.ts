describe("template spec", () => {
  it("should search for a movie", () => {
    cy.visit("http://localhost:3000/")

    cy.get("#search").should("be.visible").should("be.focused")

    cy.fixture("movie.json").then((movie) => {
      cy.get("#search").type("test") // cypress warm up workaround
      cy.get("#search")
        .clear()
        .type(`${movie.title} ${movie.year}`, { delay: 100 })

      cy.get("ul").find("li").contains(movie.title).contains(movie.year).click()

      cy.url().should("include", "/movie-details/")
      cy.get("h2").should("contain", `${movie.title} (${movie.year})`)
    })
  })
})
