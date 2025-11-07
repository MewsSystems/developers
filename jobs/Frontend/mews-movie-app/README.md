## Getting started

First, rename `.env.example` to `.env.local` and fill in your TMDB API key.

Then, install dependencies and run the development server:

```bash
npm install
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

## My solution

I chose Next.js with Typescript as a framework for React, RTK Query for data fetching, and Styled Components for styling. For testing, I used a combination of Jest and React Testing Library.

### Redux

I didn't use Redux directly, but I believe that RTK Query I used, is in this scenario sufficient. For data fetching and also as a state manager. Also it uses Redux store under the hood.

For better type safety, I generated types for TMDB API using `openapi-typescript`, but I'm not completely satisfied with this solution, as using these types is not very convenient. RTK Query has also a [tool for code generation](https://redux-toolkit.js.org/rtk-query/usage/code-generation#openapi), but I haven't managed to get satisfying results in a reasonable time. However, for a real project it would be better alternative. Another option is to use a library like `zod`, but it requires defining schemas for all endpoints manually."

### Styled components

As this was my first experience with `styled-components`, I'm not sure if I followed all the best practices.

For better consistency, in a real project it would be better to use some theme or design system, with a set of predefined tokens. However, for the sake of simplicity, I decided only to use some custom css properties defined globally in the `:root`.

### Tests and linting

I only wrote few unit tests, to illustrate how I would write them. I picked the React Testing Library for rendering React and Jest as a test runner.

There is also `eslint` set up for linting and `prettier` to maintain code formatting.

## Conclusion

I believe, this application is in the state, we can discuss it and I would be more than happy to do so. I hope it is enough to illustrate my experience and thoughts about frontend development. However, there still might be some edge cases not covered, or perhaps some best practices not in use, as few libraries were new to me. 

If something is missing or should be done differently, please let me know, and I would be more than happy to address it.