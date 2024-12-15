# Movie Search Application

## Introduction

This is a simple Movie Search Application built with React and TypeScript, allowing users to search for movies, view a list of search results, load more results, and view detailed information about a selected movie.
The application implements the search functionality by fetching data from an external movie API: [TheMovieDb API](https://developers.themoviedb.org/3/getting-started/introduction).

## Features

- Search functionality: Users can search for movies by typing in the search input. Search results are fetched automatically after typing is finished (debounced input).
- Paginated results: The results are fetched in pages, and users can load more results by clicking the "Load More" button.
- Movie details view: Clicking on a movie will navigate to a detailed view of the movie, displaying additional information.
- Debounced search: The search input is debounced to avoid unnecessary API calls while typing.

## Technologies Used

- React for building the user interface
- TypeScript for static type checking
- Styled-Components for CSS-in-JS styling
- Vite for fast development and build tool
- React Router for navigation
- Axios for making HTTP requests
- Debounce logic for efficient searching

## Development Plan

1. Initialize Project: Set up with Vite, TypeScript, and testing tools.
2. Routing: Implement views for searching and viewing movie details.
3. Reusable Components: Build components like MovieCard, MoviesGrid, Input, and Button with tests.
4. Views Composition: Integrate components into SearchView and MovieDetailsView.
5. API Integration: Fetch data from TheMovieDb API.
6. Global State Management.
7. Testing: Add unit, integration, and end-to-end tests throughout.
8. Polishing: Optimize performance, add accessibility, and improvement.

## How to Run the Project

1. Clone the repository
   ```sh
   git clone {REPOSITORY_URL}
   ```
2. Install dependencies
   ```sh
   git install
   ```
3. Run the development server
   ```sh
   npm run dev
   ```
4. Run tests
   ```sh
   npm test
   ```

## Development Setup

This project is set up using [Vite](https://vitejs.dev/) as the build tool, providing fast development and build times. It is configured with TypeScript and ESLint for code quality.

### Vite Setup

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

### Expanding the ESLint configuration

If you are developing a production application, we recommend updating the configuration to enable type aware lint rules:

- Configure the top-level `parserOptions` property like this:

```js
export default tseslint.config({
	languageOptions: {
		// other options...
		parserOptions: {
			project: ["./tsconfig.node.json", "./tsconfig.app.json"],
			tsconfigRootDir: import.meta.dirname,
		},
	},
});
```

- Replace `tseslint.configs.recommended` to `tseslint.configs.recommendedTypeChecked` or `tseslint.configs.strictTypeChecked`
- Optionally add `...tseslint.configs.stylisticTypeChecked`
- Install [eslint-plugin-react](https://github.com/jsx-eslint/eslint-plugin-react) and update the config:

```js
// eslint.config.js
import react from "eslint-plugin-react";

export default tseslint.config({
	// Set the react version
	settings: { react: { version: "18.3" } },
	plugins: {
		// Add the react plugin
		react,
	},
	rules: {
		// other rules...
		// Enable its recommended rules
		...react.configs.recommended.rules,
		...react.configs["jsx-runtime"].rules,
	},
});
```

## Approach

The following features and technical decisions have been made:
