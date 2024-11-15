// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --

// // Example: cy.get('#id').moveTo({ x: 0, y: -100 })
// Cypress.Commands.add("moveTo", { prevSubject: true }, (subject, options) => {
//   const offsetTop = subject.offset().top
//   const offsetLeft = subject.offset().left
//   cy.get(subject)
//     .trigger("mousedown", { which: 1, button: 0 })
//     .trigger("mousemove", {
//       pageX: offsetLeft + options.x,
//       pageY: offsetTop + options.y,
//     })
//   cy.get("body").trigger("mouseup", {
//     pageX: offsetLeft + options.x,
//     pageY: offsetTop + options.y,
//   })
// })

//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
