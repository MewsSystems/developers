# Getting Started

This project was bootstrapped with [Vite](https://vitejs.dev/guide).

It uses the TMDB-API and is subject to [TMDB's terms of use](https://www.themoviedb.org/api-terms-of-use).

### 📽️ [API Documentation](https://developer.themoviedb.org/docs/getting-started)

## Important
The `.env` file contains the TMBD API key solely for demonstration purposes.

## Available Scripts

In the project directory, you can run:

### `npm run dev`

Runs the app in the development mode.\
Open [http://localhost:5173](http://localhost:5173) to view it in the browser.

### `npm run build`

Creates an optimized production build of your project.

- Executes `tsc` to compile TypeScript code into JavaScript.
- Runs `vite build` to bundle the compiled JavaScript and project assets for deployment.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm run lint`

Analyzes your code for potential errors, stylistic inconsistencies, and unused code constructs.

- Leverages the `eslint` linter to scan all files (`.`) with extensions `.ts` and `.tsx`.
- Flags unused `eslint-disable` directives to ensure proper linting behavior.
- Sets the maximum warning threshold to 0, meaning any linting issue will be reported.

### `npm run preview`

Starts a development server to preview your application in a web browser.

- Executes `vite preview` to initiate a development server using the Vite build tool.
- This allows you to make code changes and see them reflected in the browser without manual rebuilds.

### `npm run test`

Runs unit tests to verify the correctness of your application's functionality.

- Employs the `vitest` test runner to execute unit tests.

### `npm run test:coverage`

Executes unit tests and generates a code coverage report to assess which parts of your codebase are covered by tests.

- Runs `vitest run` with the `--coverage` flag to capture code coverage data during test execution.
- Disables test watching (`--watch=false`) to generate a more accurate coverage report after all tests have completed.

### `npm run prettify`

Automatically formats the project's code files according to Prettier's style guidelines.

- Utilizes the `prettier` code formatter with the `--write` flag to apply formatting changes directly to files.
- Applies formatting to all files within the project (`.**`) with extensions `.js`, `.jsx`, `.ts`, `.tsx`, `.html`, `.css`, and `.scss`.

### `npm run interface`

Generates TypeScript interface definitions for i18next translation resources.

- Executes the `i18next-resources-for-ts` tool with the `interface` subcommand.
- Specifies the input directory (`-i`) containing translation resources (e.g., `./src/locales/en`).
- Defines the output file (`-o`) for the generated TypeScript interface definitions (e.g., `./src/@types/resources.d.ts`). This file will provide type safety for your internationalization code.

## Learn More

To learn React, check out the [React documentation](https://reactjs.org/).

