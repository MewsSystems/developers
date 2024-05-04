# Mews frontend developer task

## Overview

Accessible here - https://developers-frontend-jake-towers-projects.vercel.app. For the purposes of this task I've used a feature branch, but in the real world this would be a master/main branch and driven from dev/staging/production environments.

As requested, I have created a Pull Request which can be found here - TODO

## Tech

- [Next.js](https://nextjs.org/) - React Framework for the project to allow for client/server-side rendering and ease of deployment using Vercel's free hosting
- [Vercel](https://vercel.com/) - Platform for hosting the application, hooks into the repository to allow for CI/CD. See Deployment section
- [TypeScript](https://www.typescriptlang.org/) - Strongly typed language that builds on JavaScript
- [Styled Components](https://styled-components.com/) - Styling for the project
- [ESLint](https://nextjs.org/docs/basic-features/eslint/) - For linting within the project
- [Prettier](https://prettier.io/) - For formatting the project
- [husky](https://github.com/typicode/husky/) - Allows for a pre-commit hook that ensure conditions like linting and tests pass on commit
- .nvmrc - Tells you the preferred Node version to use for this project
- [Playwright](https://playwright.dev/) - End to End (E2E) tests for the project
- [Jest](https://jestjs.io/) - Unit tests for the project

## Running locally

This is a [Next.js](https://nextjs.org/) project bootstrapped with [`create-next-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

Prerequisites:

- node - v20.11.1
- npm - 10.2.4

First, install the node packages:

```
npm i
```

Then run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

## Testing

### Unit Tests

```
npm run test
```

### E2E Tests

Install Playwright Browsers

```
npx playwright install
```

Run the application locally first, then run

```
npm run test:e2e
```

## Deployment

This application would be deployed to Vercel from the master/main branch.

For demonstration purposes, I've used this branch to deploy it to demonstrate how it can be done both through Vercel and using a GitHub Actions workflow.

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/deployment) for more details.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js/) - your feedback and contributions are welcome!

## The Task

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
