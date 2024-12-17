# **Movie Search Application**

A modern, responsive movie search application built with React, TypeScript, and Tailwind CSS. Powered by [The Movie Database (TMDB) API](https://www.themoviedb.org/), users can search for movies, view details, and navigate through paginated results.

---

## **Features**

- 🎥 **Search Functionality**: Search for movies by title using The Movie Database API.
- 🧩 **Pagination**: Navigate through multiple pages of movie results.
- ⭐ **Movie Details**: View comprehensive details about a selected movie, including its overview, release date, and rating.
- ⚡ **Real-Time Feedback**: Displays loading states, error messages, and "no results" feedback.
- 🧵 **Debounced Search**: Ensures efficient API calls by debouncing the input.
- 🖥️ **Responsive UI**: Fully responsive design built with Tailwind CSS.
- ♿ **Accessibility**: Keyboard navigation and ARIA attributes for better accessibility.

---

## **Tech Stack**

| **Technology**     | **Purpose**                         |
|---------------------|-------------------------------------|
| **React**          | Component-based UI development      |
| **TypeScript**     | Static typing and code reliability  |
| **Tailwind CSS**   | Utility-first CSS framework         |
| **React Router**   | Routing for navigation              |
| **Cypress**        | End-to-end testing                  |
| **Jest**           | Unit testing for hooks and components |
| **TMDB API**       | Fetching movie data                 |

---

## **Folder Structure**

```bash
src/
├── assets/               # Images and static assets
├── components/           # UI components (e.g., MovieCard, SearchBar)
├── hooks/                # Custom React hooks (e.g., useFetchMovies)
├── pages/                # Application pages (e.g., SearchPage, MovieDetailPage)
├── utils/                # Utility functions (e.g., pagination functions)
├── types/                # TypeScript interfaces and types
└── App.tsx               # Root component
```

---

## **Available Scripts**

- `yarn start`: Runs the application in development mode.
- `yarn test`: Runs the unit tests with Jest.
- `yarn lint`: Runs ESLint to check code for errors and enforce coding standards.
- `yarn cypress:open`: Opens Cypress for end-to-end testing.
- `yarn cypress:run`: Runs Cypress tests.

---

## **Future Enhancements**

- 🔍 **Filter by Genre**: Allow users to filter movies based on their genre.
- 🎚️ **Sort by Rating**: Add sorting options for movies (e.g., ascending/descending ratings).

