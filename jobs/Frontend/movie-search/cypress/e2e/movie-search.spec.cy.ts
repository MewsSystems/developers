const gotoApp = () => {
  cy.visit('http://localhost:3000/');
}

beforeEach(() => {
  gotoApp();
});

describe('Movie Search application', () => {
  it('displays the logo link', () => {
    cy.get('.logo-link').should('exist');
  });

  it('navigates to SearchView route by default', () => {
    cy.get('.search-view').should('exist');
  });

  it('displays search box', () => {
    cy.get('input[type="text"]').should('exist');
  });
});




