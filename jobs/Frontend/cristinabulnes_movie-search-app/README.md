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
- Context API and Redux for global state management (two approaches demonstrated)

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

### State Management Options

#### Local State Management

For a task like this (creating a movie search application with a search and a detail view) local state management is often sufficient. The state is primarily view-specific:

- Search View:

  - Search query tied to the input field.
  - Paginated list of movies fetched based on the query.
  - Loading and error states for API handling.

- Movie Detail View: State for the detailed information about the selected movie.

##### Issue with this approach

While this approach works well for smaller apps, it introduces a limitation: when navigating between views, the local state (the search query and results) is reset.
You can mitigate this by using React Router State or URL Parameters to preserve the search query. However, the fetch request will indeed be triggered again when revisiting the search view.

#### Global State Management

Using Context API or Redux provides a centralized state that persists across views:

- The search query, movie results, and other states can be globally managed, ensuring the search view restores seamlessly when navigating back.
- This eliminates redundant API calls, enhances performance, and improves user experience.

##### What’s the Best Approach?

- For a small app, Context API suffices to maintain global state and demonstrate effective state management.
- For a scalable app, tools like React Query or Redux are more suitable for managing state and caching efficiently.

#### Solution implemented. Showcasing Skills

Although Context API would be sufficient for the requirements of this exercise, I have chosen an approach that allows for a future transition to Redux. Initially, I implemented Context to demonstrate how to manage global state effectively, while keeping in mind how the app could evolve to use Redux. This showcases my proficiency with both Context and Redux, as well as my ability to transition between state management solutions when needed.

Additionally, the API used provides a significant amount of information, which suggests that this application could be scaled in the future. While the current implementation focuses on the specific requirements of the exercise, this approach anticipates the potential for adding more features later.

### Re-rendering Issue When Navigating Back

When navigating to a movie's detail page and returning to the search results, the SearchMoviesView component would re-render, causing the API to re-fetch movie data and resetting the scroll position. This disrupted the user experience and caused redundant network calls.

#### Solution: Switching to a Modal for Movie Details

To address this, the movie detail page was implemented as a modal. This approach allows the SearchMoviesView to remain mounted while displaying movie details in an overlay.

Key Benefits:

- Maintains the state of the search results.
- Prevents unnecessary re-fetching of data when navigating back.
- Improves overall user experience and performance.

## Project Structure

```graphql
src/
├── components/         # Reusable UI components (e.g., MovieCard, MoviesGrid, Input, Button)
├── contexts/           # Context API logic and providers (e.g., SearchMovieContext)
├── hooks/              # Custom hooks (e.g., useMovieSearch, useDebounce)
├── pages/              # Page components for routing (e.g., SearchView, MovieDetailsView)
├── redux/              # Redux-related logic
│   ├── slices/         # Redux slices (e.g., movieSearchSlice)
│   ├── thunk/          # Async thunks for API calls
│   └── store.ts        # Redux store configuration
├── services/           # API service functions (e.g., fetchMovies)
├── theme/              # Theme files
├── App.tsx             # Application root component
├── main.tsx            # Entry point for the React application
└── vite.config.ts      # Vite configuration

```

#### Key Directories

`components/`
This directory contains reusable, presentational components used across the app, such as:

- MovieCard: Displays movie details in a card format.
- MoviesGrid: Arranges multiple movie cards in a grid layout.
- Input: Custom input component for the search bar.
- Button: Reusable button component for actions like "Load More."

`contexts/`
Contains the logic for managing global state using Context API.

- SearchMovieContext.tsx: Implements state and functions for managing the search query, movies, and pagination.

`hooks/`
Custom hooks that encapsulate reusable logic.

- useDebounce.ts: Implements debouncing for efficient API calls.
- useIntersectionObserver.ts:

To showcase proficiency with both Context API and Redux, as I've implemented these 2 approach, two custom hooks have been created for each state management:

- useMovieSearch (Context API):

  - Manages global state via the Context API using the SearchMovieContext.
  - Handles the search query, paginated results, and error/loading states.
  - Implements debounced search functionality with a custom useDebounce hook.

- useMovieSearchRedux (Redux):

  - Uses Redux to manage the global state for the movie search functionality.
  - Dispatches actions (setQuery, loadMore, etc.) and uses selectors to manage query, pagination, and movie results.
  - Implements debounced search functionality with a custom useDebounce hook.

You just have to wrap the App with the corresponding provider and use the corresponding hook in the SearchMovieView.ts to use one or the other.

`redux/`
Holds all Redux-related logic, structured for scalability:

- `slices/`: Contains movieSearchSlice.ts, defining reducers and actions.
- `thunk/`: Contains asynchronous logic for fetching movies (movieSearchThunk.ts).
- `store.ts`: Configures the Redux store with middleware and slices.

`services/`
This directory contains functions for making API calls:

- `movieService.ts`: Implements fetchMovies, an abstraction over Axios for interacting with TheMovieDb API.

`pages/`
High-level components representing views in the application:

- `SearchMobiesView.tsx`: Displays the movie search input and the results of the search.
- `MovieDetails.tsx`: Displays detailed information about a selected movie.
