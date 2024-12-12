describe("Quick test about the search of the app", () => {
  beforeEach(() => {
    cy.viewport(1000, 1000);
    cy.visit("http://localhost:3000/");
  });
  it("Searching in the two fields works", () => {
    cy.get(".MuiAutocomplete-root.css-b36q3f-MuiAutocomplete-root");
    cy.get(
      ".sc-khLCKb > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("s");
    cy.get(
      ".sc-khLCKb > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("t");
    cy.get(
      ".sc-khLCKb > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("ar ");
    cy.get(
      ".sc-khLCKb > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("wars");

    cy.get(":nth-child(1) > .sc-cHqXqK > .sc-cEzcPc").should(
      "include.text",
      "Star Wars"
    );

    cy.wait(1000);
    cy.get(":nth-child(1) > .sc-cHqXqK > .sc-cEzcPc").should(
      "include.text",
      " "
    );

    cy.get(
      ".sc-kEfuZE > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("inception");
    cy.wait(1000);
    cy.get(
      ".sc-kEfuZE > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type(" ");
    cy.get(":nth-child(1) > .sc-cHqXqK > .sc-cEzcPc").should(
      "have.text",
      "InceptionLanguage: enRelease: 2010-07-15Popularity: 124.3Vote Average: 8.369/10Cobb, a skilled thief who commits corporate espionage by infiltrating the subconscious of his targets is offered a chance to regain his old life as payment for a task considered to be impossible: \"inception\", the implantation of another person's idea into a target's subconscious."
    );

    cy.get(":nth-child(1) > .sc-cHqXqK > .sc-cEzcPc > .sc-eOzmre").click();
    cy.get(".sc-jtQUzJ").should("be.visible");
    cy.get(".sc-iqyJx").should("have.text", "Inception");
    cy.get(".sc-CNKsk > :nth-child(4) > .sc-eWPXlR").should(
      "have.text",
      "Production Companies"
    );
    cy.get(":nth-child(1) > .sc-eWPXlR").should("have.text", "Genres");
    cy.get(":nth-child(6) > .sc-eWPXlR").should("have.text", "Origin Country");
    cy.get(":nth-child(9) > .sc-eWPXlR").should(
      "have.text",
      "Spoken Languages"
    );
  });
  it("Sliders and home features works", () => {
    cy.get(".sc-diYFot").should("have.text", "Movie Searcher");
    cy.get(".sc-diYFot").click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(2) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();

    cy.get(
      ":nth-child(3) > .sc-iuUfFv > .sc-eUlrpB > .sc-geXuza > :nth-child(2) > .sc-cHqXqK > .sc-cEzcPc"
    ).should(
      "have.text",
      "The GodfatherLanguage: enRelease: 1972-03-14Popularity: 170.4Vote Average: 8.689/10Spanning the years 1945 to 1955, a chronicle of the fictional Italian-American Corleone crime family. When organized crime family patriarch, Vito Corleone barely survives an attempt on his life, his youngest son, Michael steps in to take care of the would-be killers, launching a campaign of bloody revenge."
    );

    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();
    cy.get(
      ':nth-child(3) > .sc-iuUfFv > .eObrNj > [data-testid="KeyboardArrowRightIcon"]'
    ).click();

    cy.get(
      ":nth-child(3) > .sc-iuUfFv > .sc-eUlrpB > .sc-geXuza > :nth-child(8) > .sc-cHqXqK > .sc-cEzcPc"
    ).should(
      "have.text",
      "The Dark KnightLanguage: enRelease: 2008-07-16Popularity: 170.6Vote Average: 8.5/10Batman raises the stakes in his war on crime. With the help of Lt. Jim Gordon and District Attorney Harvey Dent, Batman sets out to dismantle the remaining criminal organizations that plague the streets. The partnership proves to be effective, but they soon find themselves prey to a reign of chaos unleashed by a rising criminal mastermind known to the terrified citizens of Gotham as the Joker."
    );
  });

  it("The quick search in searchers works", () => {
    cy.get(
      ".sc-khLCKb > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("the lo");
    cy.get("#customized-autocomplete-option-0 > .sc-gtLWhw").click();
    cy.get(".sc-iqyJx").should(
      "have.text",
      "The Lord of the Rings: The Fellowship of the Ring"
    );
    cy.get(".sc-diYFot").click();

    cy.get(
      ".sc-kEfuZE > .MuiStack-root > .MuiAutocomplete-root > .MuiFormControl-root > .MuiInputBase-root > #customized-autocomplete"
    ).type("Harry");
    cy.get(
      "#customized-autocomplete-option-1 > .sc-gtLWhw > .sc-dntaoT"
    ).click();
    cy.get(".sc-iqyJx").should("include.text", "Harry Potter");
  });
});
