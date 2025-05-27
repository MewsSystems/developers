# 🎬 Movie Search App

A movie search application built with React, TypeScript, and Vite. This project demonstrates a modular and scalable architecture, with a focus on clean separation of concerns, strong typing, and long-term maintainability.

---

## Tech Stack

- **React 19**
- **Vite**
- **TypeScript**
- **Styled-components**
- **React Router DOM**
- **Vitest** – unit testing
- **Playwright** – end-to-end testing
- **Husky + lint-staged + ESLint + Prettier** – code quality and formatting

---

## 📁 Project Structure 

![Screenshot 2025-05-27 at 16 12 30](https://github.com/user-attachments/assets/e5abd9d2-a138-43c2-a3be-2cc13ddf22f6)


---

## 🧱 Architectural Overview

### `core/`
Contains the **domain logic**. For example, `movie/` defines:

- What a movie is in our application (`types`)
- How we adapt external API data into our internal model (`adapter`)
- How we access movie-related data (`services/api`)
- Validation of external data

This design keeps external APIs abstracted away from the internal application logic, ensuring full control over the data structure we use.

### `services/`
Handles external service configuration. In this case, we use a REST client, but the architecture is flexible enough to support other clients in the future.

### `lib/`
Shared reusable building blocks — components, hooks, and utility logic — that can be used across the app.

### `pages/`
Each route (page) manages its own responsibilities. Local components live here unless they are reused elsewhere (then they move to `lib/`).

---
## ✨ Features

- **Movie search** powered by **TMDB API**
- **Pagination** of search results
- **Movie detail view** with title, image, overview, rating, etc.
- **Image fallback** added to the `<img>` tag to handle broken posters
- **Basic error handling** for feedback on failures (UI-level message)
- **End-to-end testing** using **Playwright** to simulate real user flows
- **Lazy Routes**
- **404 page**

---

## 📦 Setup Instructions

### Requirements

- **Node.js v18+**
- A `.env` file with the following variables:

```env
VITE_TMDB_API_URL=https://api.themoviedb.org/3
VITE_TMDB_API_KEY=your_api_key_here
```

---

## ⚠️ **Important:**  
To run the end-to-end tests locally, you must start the application first:  
- Run `pnpm dev` in a separate terminal before executing any Playwright tests.

### 📌 Limitations & To-Do

## Search query is not persisted in the URL
This means that if a user searches for a movie and navigates to a detail page, going back will reset to the homepage.
→ I planned to persist the search term in the URL to preserve state on navigation, but I didn’t have enough time.

## Error handling is minimal
- There is a basic UI feedback if something goes wrong, but it's generic. I would like to:

- Add specific error messages

- Implement a proper Error Boundary component

- Integration tests are not included yet, but are part of the future roadmap.

## Roadmap
- Add integration tests

- Improve accessibility

- Enhance UI, including a new “Featured Movies” section on the movie detail page

- Persist search term in the URL to maintain search state on navigation

- Add a complete Error Boundary and more granular error feedback