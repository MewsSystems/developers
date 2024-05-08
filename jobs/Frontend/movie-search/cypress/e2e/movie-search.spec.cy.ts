describe('Movie Search application', () => {
  it('Visits Movie Search', () => {
    cy.visit('http://localhost:3000/');
    cy.get('h2[data-test=heading]').contains('Movie Search')
  })
})