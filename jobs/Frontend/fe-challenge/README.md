# Movie Search

An application for searching movies.

Check out the live version of the project [here](https://mews-fe-challenge.vercel.app/).

## Setup and Run

Install the dependencies:

```
npm install
```

Run the project:

```
npm run dev
```

Open [http://localhost:5173/](http://localhost:5173/) to view it in your browser.

### Tests

To run the tests:

```
npm run test
```

### Linting

To run linting:

```
npm run lint
```

### Format

To format the code with Prettier:

```
npm run format
```

## Build

Running this script will generate the app bundle in the `dist` folder.

```
npm run build
```

## Features

- Movie search view.
  - Infinite scroll pagination.
  - Debounced input search.
  - Search by query param in URL (eg. `http://localhost:5173/?q=Any+movie`)
  - Keep previous search when going back from detail view.
  - Requests caching for 30 minutes.
- Movie detail view.
  - Prefetch detail request in search view when hovering in the movie.
  - Request caching for 30 minutes.
- Implemented an ACL (Anti Corruption Layer) using Hexagonal Architecture that allows seamless future integrations with alternative APIs to TMDB.

## Technology stack

- [ViteJS](https://vitejs.dev/) - For running a local development server and building.
- [ReactJS](https://react.dev/) + [Typescript](https://www.typescriptlang.org/) - For building the application.
- [React Router](https://reactrouter.com/) - For building a SPA.
- [React Query](https://tanstack.com/query/latest) - For adding a server state to the app.
- [React Intersection Observer](https://www.npmjs.com/package/react-intersection-observer) - For detecting when an element enters the viewport.
- [React Query Devtools](https://tanstack.com/query/v4/docs/framework/react/devtools) - Devtools for React Query.
- [Prettier](https://prettier.io/) - For code formatting.
- [ESLint](https://eslint.org/) - For code analysis.
- [Vitest](https://vitest.dev/) - Testing framework.
- [React Testing Library](https://testing-library.com/) - For integration testing.
- [MSW](https://mswjs.io/) - For intercepting API requests for testing.
- [Tailwind CSS](https://tailwindcss.com/) - For styling the UI.

## Architecture

This application follows the principles of **Hexagonal Architecture**, also known as Ports and Adapters Architecture. It means that there is a clear separation of concerns, which makes the application more modular, flexible and easy to maintain.

It has been applied vertical slicing, so it is first separated by module (e.g. `movies`) and then by layers (`application`, `domain`, `infrastructure`).

```
.
└── src
    ├── ...
    ├── modules
    │   └── movies
    │       ├── application
    │       │   └── ...
    │       ├── domain
    │       │   └── ...
    │       └── infrastructure
    │           └── ...
    └── ...
```

### Domain

The `domain` layer contains the core business logic and entities of the application. It encapsulates the domain-specific rules and behaviors without any dependencies on external frameworks or libraries.

### Application

The `application` layer contains the use cases that implement specific features of the application. It orchestrate the interaction between the domain layer and the external interfaces.

### Infrastructure

The `infrastructure` layer provides implementations for interacting with the movies TMDB API.
All UI components and related logic are located outside of the `infrastructure` folder.

### SPA

The application is implemented as a Single Page Application using **React Router v6**.

The different views are located in the `pages` folder, and the router used in the `RouterProvider` is created in `router.tsx` file.

Within every page folder there is a `components` folder which contains the components used within the page view.
The `hooks` folder contains components-related logic.

```
.
└── src
    ├── ...
    ├── pages
    │   └── search
    │       ├── components
    │       │   └── ...
    │       ├── hooks
    │       │   └── ...
    │       ├── Search.tsx
    │       └── ...
    └── ...
```

The `components` folder under `src` contains all the components that are used across different parts of the application.

```
.
└── src
    ├── ...
    ├── components
    │   └── ...
    └── ...
```

The `queryClient` file is responsible for creating the React Query query client for using across the application. It contains custom error handling for managing all the errors that occur in the requests. For now, it is just a `console.error`.

### Data fetching / caching

For data fetching, it has been utilized **React Query**, which provides a powerful and intuitive API for handling asynchronous data operations seamlessly within React applications.

#### Movie search

Whenever a new search is initiated, the page fetches movie data by pages, employing an infinite scroll pagination technique. As the last movie element enters the viewport, a new request is automatically triggered to fetch the next page of results.

All this logic is encapsulated within two custom hooks: `useSearchMovie` and `useInfiniteScroll`, which handle the data fetching and pagination seamlessly.

To streamline the data fetching process, React Query's `useSuspenseInfiniteQuery` hook is used. This hook simplifies the implementation of infinite pagination by handling data fetching and caching in a straightforward way.

Furthermore, a stale time of 30 minutes has been set for the queries. This means that if a search has been performed within the last 30 minutes, React Query will return the previously fetched data, eliminating the need for additional HTTP requests and improving overall performance.

#### Movie detail

Movie detail page utilizes the `useGetMovieDetail` hook to fetch data, leveraging React Query's `useSuspenseQuery` hook.

Additionally, a prefetching technique is implemented. This ensures that when users navigate to the detail view, the necessary detail request has ideally been made in advance, triggered by hovering over the movie card in the search page.

Similarly to search page, a stale time of 30 minutes has been set for the query.

## Testing

For testing, it has been used **Vitest + MSW + React Testing Library**.

The implemented tests prioritize integration aspects over implementation details.

Within `utils/test-utils.tsx` it has been created a test utility function `renderWithQueryClientAndRouter`, a custom React Testing render that make it possible to render a component wrapped in a QueryClientProvider and a RouterProvider. By using this custom render you don't need to wrap your component within the providers every time you write a test.

Rather than relying on traditional mocking of fetch requests, the project utilizes **MSW**, an API mocking library that intercepts requests at the network level, enabling the definition of custom responses. This makes more realistic testing scenarios, that closely mimic interactions with a real API.

Within `src/mocks` there is a MSW server that intercepts all the requests defined in `handlers.ts`.

```
.
└── src
    ├── ...
    ├── mocks
    │   ├── data.ts
    │   ├── handlers.ts
    │   └── server.ts
    └── ...
```

## Next

- Enhance prefetching mechanism: prevent movie prefetching on hover while scrolling the page.
- Sort movies by release date (the search endpoint in TMDB API is not supporting it right now).
