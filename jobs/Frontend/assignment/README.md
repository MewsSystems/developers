# Overview:

This is a simple movie search app based on the-movie-db API v3. It is a part of the "Frontend" assignment for the MEWS tech interview.

# How to run:

## Prerequisites:

This project uses a `pnpm` package manager. Please make sure that you have it installed on your environment. If not, please install it by following the [installation guide](https://pnpm.io/installation).

## Steps to run:

1. Pull changes and go to the project's root - `jobs/Frontend/assignment`.
2. Install dependencies: `pnpm install`.
3. Run the project.
   3.1. In development mode: `pnpm dev`.
   3.2. In production mode: `pnpm build && pnpm serve`.

# UI Design:

I created some designs based on Material Design 3 UI KIT. Click on the Figma icon below.<br>
[<img src="https://www.bynder.com/images/meta/meta-figma.jpg" width="100">](<https://www.figma.com/file/gRJTP73ecDzbIUOBvH18fN/Material-3-Design-Kit-(Community)?type=design&node-id=54721%3A26913&mode=design&t=7EhMQFz9oViie8so-1>) <br>

# Tech stack:

## Requirements:

> TypeScript, React, Redux, and styled-components. Use of any additional libraries is allowed and it's up to you.

## React + TypeScript + Vite

For this project, I've used a Vite building tool with React and TypeScript + SWC template. I've chosen this stack because it's the most modern and fast one to start.

## Styling:

### styled-components:

I use a "styled-components" library, as it is a part of the requirements. I have a lot of experience with this library as it has been a part of my previous projects (we used it as a wrapper over Material UI, by the way when we upgraded to MUI v5 we migrated all styled-components to the similar, "emotion" library).

### material-ui/icons:

I use this library to get icons for the project as my design uses a "Material Design" concept.

## Redux:

I don't use Redux in this project as I can't find a use case for it. Probably I can move a "dark mode" logic there, but for now, I use react context.

## API client tmdb-ts:

See ["API connection"](#api-connection) section.

## Vitest:

See ["Unit testing"](#unit-testing) section.

# Folders structure:

## Short description of main folders and files:

- `src/component`: here you can find all components with their unit tests.
- `src/hooks`: here you can find all custom hooks.
- `src/pages`: here you can find all pages.
- `src/theme`: here you can find all theme-related files.
- `src/tests`: here you can find tests setup.
- `src/tmdbClient.ts`: here you can find a TMDB API v3 client setup that uses tmdb-ts library.

# API connection:

## tmdb-ts:

It is a community library for TMDB API v3. I've chosen it while searched for some types to reuse in my project and to not write them by myself. It is a very simple library that uses a "fetch API" under the hood. It has a lot of types and it's very easy to use. <br>
See the repository on GitHub - https://github.com/blakejoy/tmdb-ts.

### Advantages of tmdb-ts:

- Easy to use and set up, don't need to write own auth logic and providers.
- Fully typed data scheme for all endpoints at v3 API.
- Mostly all endpoints are covered.

### Disadvantages of tmdb-ts:

- It is not an official library, so it can be outdated.
- It does not have the ["Append To Response" ability](https://developer.themoviedb.org/docs/append-to-response). It means that I need to fetch related data separately using different requests. I can't get all the movie details page data in one big request.

# Tests:

## Unit testing:

### Vitest:

As mentioned above I use a "Vitest" as the main testing framework. I've chosen it because it's a part of the Vite ecosystem and it's very fast. I've used it in my previous projects and I like it. It is really similar to Jest, which makes it familiar to most developers. <br>
I use a "react-testing-library" for rendering components.

### Custom render for rtl:

As I need to wrap my components with some providers I've created a custom render function for "rtl" that wraps components with it. See documentation - https://testing-library.com/docs/react-testing-library/setup/#custom-render.

## E2E testing:

I haven't implemented E2E tests for this project as I don't have enough time for it. I would use a "Cypress" or "Playwright" for it.
