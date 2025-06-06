BELOW IS A LIST OF IMPROVEMENTS MADE BASED ON FEEDBACK BY A MORE SENIOR DEVELOPER:

1. ATTENTION TO DETAIL

- README and Project Metadata: README and Project Metadata customized.
- UI & UX Readability: Both movie titles and navigation buttons follow the recommended color contrast ratios. The Previous/Next buttons only have a lower contrast ratio on purpose when the user is at the first or the last page or the selection to signal that there are no more pages to go to in that direction. I also disable cursor change and hover effect when trying to click on disabled buttons.

2. ROUTING AND STATE MANAGEMENT

- Routing Functionality: Page and query hooks refactored, users can now share links to their search without the URL params reseting on render.
- Debounce Implementation: Refactored.
- Component Props: All props were used in their destructured version except for he pagination element when props were needed within styled-components. This is now corrected.

3. API AND BACKEND PRACTICES

- API Key Security: API is now loaded from a hidden .env file. Ideally, it would be stored on backend.
- Error Handling: Homepage now shows an error page when incorrect URL is entered, same with the movie deatil page.

4. TYPE USAGE

- Depth of Types: I did not use any advanced types, but will be aiming to learn more about using TS in dynamic API responses and form handling.

5. CSS AND STYLING

- Global Configuration: Created global variables and styles.
- Redundant Styling: Refactored styled-components.
- CSS Accuracy: The calc function has been corrected and I installed an extension for styled-components into my VSC to help pinpoint these types of syntax errors.

6. HTML AND ACCESSIBILITY

- Semantic Structure: Labels added, buttons and links were already used according to their purpose, title levels corrected.
- Alt Text: Fixed, each poster now has a unique alt.

FINAL COMMENTS:

- The detail page could be further decomposed into separate elements for better navigation through the code, but in such a small project, I find this unneccessary and counterproductive. Same could be said about the API calls, which are all currently in the same folder.
