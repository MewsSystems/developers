# 🎬 MovieApp

A modern movie search app built with React, TypeScript, and the TMDb API, emphasizing accessibility, maintainability, scalability, and alignment with Mews standards.

---

## 🚀 Features

- 🔍 Live search for movies
- 📄 Display popular movies with pagination
- 🎥 Detailed movie view (genres, score, runtime, language, homepage, etc.)
- 🧭 Client-side routing with React Router
- ⚙️ Fully typed with TypeScript
- ♿ Accessible and WCAG-compliant UI
- 🎨 Styled using `styled-components`
- 🧪 Visual and accessibility testing with Storybook

---

## 🛠️ Tech Stack

- **Frontend**: React, Vite, TypeScript
- **Routing**: React Router
- **Styling**: styled-components
- **Testing**: Storybook (visual & a11y)
- **API**: TMDb REST API

---

## 🧱 Component Structure

Component organization is similar to structure `mews-js/optimus` (source: https://www.mews.design/latest/onboarding-resources/engineering-path-iW0NM7XL-iW0NM7XL )

- Co-located `style.ts`, `types.ts`, `story.tsx`
- Design tokens (`fontSizes`, `spacing`) for consistent UI
- Clean separation of logic and layout

---

## 🎨 Styling

- Fully migrated to [`styled-components`](https://styled-components.com/) from vanilla CSS
- Removed all global and component-specific `.css` files
- Design tokens prepared for future scalability

---

## ♿ Accessibility

This project prioritizes accessibility to comply with modern legal standards and ensure an inclusive user experience for everyone.

- Complies with [WCAG](https://www.w3.org/WAI/standards-guidelines/wcag/) standards
- Semantic HTML structure
- Keyboard navigability
- Visible focus indicators (keyboard-only)
- Storybook a11y addon used for auditing components

---

## 📦 Visual Testing

- Storybook configured with:
  - Docs view for component overview
  - Accessibility checks via `@storybook/addon-a11y`
  - Visual snapshots for the `Button` component

---

## 🔐 Environment Variables

To configure the environment variables for this app, follow these steps:

1. Copy the example environment file to create your own local `.env` file:
`cp .env.example .env`

2. Open the newly created `.env` file and replace the placeholder value for the TMDb API key with your own key.

3. If you do not have a TMDb API key, you can request one by emailing me at `borka34@gmail.com`, and I will provide you with mine.

---

## 🔗 Live Demo

➡️ [View the live demo here](https://tmdb-search-app.netlify.app/)