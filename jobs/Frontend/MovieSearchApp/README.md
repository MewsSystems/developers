# Movie Search App 🎞️

A simple movie search application built with Remix and Vite.

## Features 

- Search for movie by title
- View movie details

## Tech Stack

- [React](https://reactjs.org)
- [Remix](https://remix.run)
- [Vite](https://vitejs.dev)
- [TypeScript](https://www.typescriptlang.org)
- [ESLint](https://eslint.org)
- [Prettier](https://prettier.io)
- [CSS Modules](https://github.com/css-modules/css-modules)
- [Vitest](https://vitest.dev)
- [React Testing Library](https://testing-library.com/docs/react-testing-library/intro)
- [Cypress](https://www.cypress.io)

## Prerequisites

- [Node.js](https://nodejs.org) (v20.12.0 or newer)
- [npm](https://www.npmjs.com) (v10.5.0 or newer)

## Getting Started

### Set up your TMDB API key

1. Duplicate the `.env.example` file and rename it to `.env` by running the following command
```shellscript
cp .env.example .env
```
2. Open the `.env` file
3. Paste your TMDB API key in the `TMDB_API_KEY` variable

### Install dependencies

```shellscript
npm install
```


### Development

```shellscript
# Start the Vite dev server
npm run dev 
```

### Testing

```shellscript
# Run unit and integration tests
npm run test

# Run end-to-end tests
npm run cypress:run
```

### Deployment

```shellscript
# Build the app for production
npm run build

# Start the app in production mode
npm start
```

## Remix Framework Documentation
📖 See the [Remix docs](https://remix.run/docs) and the [Remix Vite docs](https://remix.run/docs/en/main/guides/vite) for details on supported features.

