# Mews frontend homework

# Running
1. Add a GitHub classic token to `.npmrc` (replace GITHUB_AUTH_TOKEN) which needs a `read:packages` permission 
1. `npm ci`
2. Add `.env.local` with a `NEXT_PUBLIC_ENV_TMDBAPI_TOKEN` or add it to the `.env` file
2. `npm run dev` to run
3. `npm run test` to test

# Notes

Implementation is very basic with more focus on code and logic, than design and responsivity.
Both of these are fairly easy to add, however I do not think it is the end goal of this exercise.

I've used the stack mentioned in the tasks read me, with some additions:
- styled-system: my favorite way for declarative styling (and easy responsivity)
- RTK Query Openapi generator: With TMDB Api having an OpenAPI spec we can use a generator to create query hooks and interfaces for easy type safety
- Next.js: I'm most familiar with it, not utilised to its potential

# What I would do if this was real production app
1. Make it Pretty
2. Loading Skeletons
3. While searching show a list of options (?) 
3. Show more data on movie detail (Actors, recommendations based on current movie, Collections, etc...)
4. Switch to a different directory and component structure (move to a more design system approach)
5. Probably switch from styled-components to [vanilla-extract](https://vanilla-extract.style/) and [sprinkles](https://vanilla-extract.style/documentation/packages/sprinkles/) to loose runtime overhead (Shopify is using this)
3. Utilize Framework features (image loading, possible on demand static generation, maybe App diretory), plus make sure redux slices correctly code splits
4. Honestly, drop Redux in favor of TanStack Query and utilize Suspense (which redux & RTK Query does not support) 
5. For local state use Zustand
5. Switch from pagination to infinite scroll or add a "Load More" button 
5. Keep search and pagination in the URL for better UX
2. Big animations with Framer Motion
3. Micro interactions animations with Lottie
4. Add Prettier and config ESLint better
5. E2E tests
6. CI/CD integration
7. Advanced testing could use some common setup helpers
