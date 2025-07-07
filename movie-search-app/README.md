# Movie search application

A movie search application set up with React, Typescript and Vite. The application provides the user with a paginated list of popular movies and allows them to search and also click into a movie to view more details on that specific title.

The project was setup using vite create template. This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

# Main stack:

React + TypeScript + Vite
Styled Components
React Router
Vitest + Jest

# Resources
(https://github.com/vitejs/vite/tree/main/packages/create-vite)
(https://vite.dev/guide/)
(https://reactrouter.com/start/declarative/installation)
(https://styled-components.com/docs/basics#getting-started)
(https://developer.themoviedb.org/docs/getting-started)
(https://vitest.dev/api/)

# Setup steps

Vite requires Node.js version 20.19+, 22.12+. Please upgrade if your package manager warns about it.

example using nvm: nvm install v24.2.0 then nvm use v24.2.0

Once the project has been cloned you can run an install to grab the project dependancies.

`npm install`

You will then need to provide an auth token to be able to query the movie database api.

You can either update options.ts and replace the `token` value or you can add a .env.local and add the value here

`VITE_API_TOKEN={your_token}`

After these steps you should then be able to run the application locally

`npm run dev`

This should load the app at the correct route automatically if not it should appear at

`http://localhost:5173/movies/1`

# Testing

The testing framework I used was vitest to setup some initial tests. I wrote two test files one for the Body.tsx file and the other for the Pagination.tsx file.

I tried to focus these on testing the functionality of the application to validate it worked as expected.

The tests can be ran with the command:

`npx vitest run`