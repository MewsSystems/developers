describe('Primary use case', () => {
  it('Search for a movie and see the detail page', () => {
    cy.visit('http://localhost:5173')

    // select the input element and type a value
    cy.get('input').type('matrix')

    // wait for the search results to appear
    // several div with the data-test attribute set to "card" should be displayed
    cy.get('[data-test=card]').should('have.length.gt', 0)
    
    // click on the first card containing the text "The Matrix"
    cy.get('[data-test=card]').contains('The Matrix').click()
    
    // check that the div containing the overview text is displayed
    cy.get('[data-test=overview]').contains('tells the story of a computer hacker who joins a group')
  })
})