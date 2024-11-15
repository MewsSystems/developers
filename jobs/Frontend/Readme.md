# Mews frontend developer task

You should start with creating a fork of the repository. When you're finished with the task, you should create a pull request.

Your task will be to create a simple movie search application. The application will have 2 views - search and movie detail. The search view is the default view, and should contain search input and display paginated list of found movies with a way to load additional batch. Search should start automatically after typing into the input is finished - there is no need for a search button. Clicking on a movie gets you to the movie detail view where detailed information about the movie should be listed.

To retrieve information about movies, use [TheMovieDb API](https://developers.themoviedb.org/3/getting-started/introduction). You can use our api key to authorize requests:

```
03b8572954325680265531140190fd2a
```

## Required technologies

To test your proficiency with the technologies we use the most, we require the solution to be written in React and TypeScript.
We use styled-components as our main CSS-in-JS framework, yet feel free to use other solutions you are more familiar with.
The use of any additional library is allowed and up to you.

## Requirements

- Node.js >= 20.14.0 (LTS)

## Tech Stack

- TypeScript
- React
- Tailwind CSS
- Vite
- pnpm

## How to Install

- Install node_modules \
  `pnpm install`
- Optionally, make a copy of `.env.example` as `.env` \
  Set `VITE_THEMOVIEDB_API_URL` as the API URL of THEMOVIEDB \
  Set `VITE_THEMOVIEDB_API_KEY` as the API KEY of THEMOVIEDB \
  `cp .env.example .env`

## How to Run

- `pnpm dev` to run the app in local

## How to Build

- `pnpm build` to build the app
- `pnpm preview` to preview the productioin in local

## Commands for Devlopment

- `pnpm lint` to check and fix lint errors
- `pnpm format` to enforce coding styles
- `pnpm test` to run automated tests
