# MovieFan application where you can search the movie you want to know more about

To start the dev server:

- update the dependecies with `yarn`
- perform `yarn dev` after that

To check the production:

- run `yarn build`
- then `yarn preview`

To check unit tests:

- run `yarn test`
- or `yarn test:ci` for coverage

To check linting:

- run `yarn lint`

## More about my process of thinking when developing it

Besides required `React` and `TS`:

- Thought to not use any 3rd party state management for such a small project. It was enough for me to work with `useState()` and `useContext()` and to have a more lightweight bundle after all.
- Wanted to try out `Vite` bundler due to it's popularity, modern approach and cool features like per route bundling and prebundling deps.
- Decided to style everything with `styled-components` as your company's main styling tool.
- Configured `eslint/tslint` for the project, used `prettier` also.
- For unit testing I went with `jest` and `react-testing-library`. Covered a couple of tests to show my approach to that.
