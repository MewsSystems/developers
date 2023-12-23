## Getting Started

Before you run the app locally, please create the .env.local file in the root folder, and add `NEXT_PUBLIC_TMDB_API_KEY` into it. API key can be found [`HERE`](https://github.com/MewsSystems/developers/tree/master/jobs/Frontend).

Run the development server with:

```bash
pnpm install
pnpm dev
```

(or your favorite package manager. 😊)

## Techstack

- React, Redux Toolkit, RTK Query, Typescript, Next.js
- Styled Components, Tailwind
- Prettier, EsLint
- Playwright, Jest
- Pnpm

## Highlights

- Built on top of the Next.js 14 and the app router.
- Shared components reside in `src/components`, while app-specific components are collocated within the app folder.
- Utilizes RTK Query for asynchronous state management. (and Redux for local state management).
- Features a unique background created with React Three Fiber.
- GitHub Actions have been set up.
- The design is responsive + isMobile hook.

## To run tests

### Playwright

(just once)

```bash
pnpm exec playwright install
```

and run with

```bash
pnpm test:e2e
```

### Unit tests

```bash
pnpm test
```

## To improve

- Currently, the URL only reflects the search query; it would be beneficial to also reflect pagination.
- Both Unit and End-to-End tests are set up, but the app has undergone only brief testing.
- Since the tech stack is predominantly client-side, the app could benefit from the use of loaders or skeletons to enhance the user experience.
