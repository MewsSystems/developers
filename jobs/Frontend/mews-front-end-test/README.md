# Mews Front End Test - Luke Phyall

## Task

Implement a movie search application, to meet the following acceptance criteria:

- Application should have two views: the search page and a detail page.
- The search page should include a input and display a paginated list of movies.
- There should be controls to load the next page, if one exists for the given search query.
- The search should begin on input.
- On clicking a movie returned for the given search query, the user should be taken to a details view with further information about the selected film.
- The app should make use of React and TypeScript at a minimum.

## Setup

I used Yarn as the package manager on this task because I like it. To use npm instead, delete the yarn.lock file and `npm install`. Otherwise, to install dependencies:

### `yarn`

then 

### `yarn start`

to run the dev server.

The tests can be run with:

### `yarn test`

## Decisions

- This solution was spun up with Create React App. [As per the docs](https://react.dev/learn/start-a-new-react-project), the current advice is to implement a React app via use of a framework such as Next or Remix. Whilst I could have done this, I'd have been winging it, and any faulty design patterns would have been obvious to an experienced engineer. In an effort to adequately demonstrate competence for the sake of this test, I chose to do it the old fashioned way.
- The app implements page routing via React Router 6, which was added manually (due to the choice not to go with a framework).
- State management is achieved via Redux Toolkit. In reality, a project limited in scope to exactly the requirements given probably doesn't need it, and could be achieved via the use of the native Context API. However, I chose to use Redux here to demonstrate previous experience and familiarity with it.
- The styling solution chosen was Styled Components. This is a library that's been used at a number of companies I've worked at (including my current one), and is a solution I'm familiar with.
- Network requests are handled using the browser fetch API, which is more than adequate for the task.

## Further work

Part of the guidance received was that the solution didn't need to be complete as such, but it should be in production-ready state. As such, there are a number of items that haven't been implemented here that I would consider for ongoing work:

- The solution looks awful. Functional, but awful. Sorry about that. Normally I'd be working to-the-pixel from a design mock up.
- The search bar could really do with being debounced. I attempted this, but ended up in a situation where all I was doing was deferring the number of search requests the app was making. The issue was the need to determine the time between keystrokes; with the solution implemented here, a new network request fires each time the search query changes by a single character. Not a massive issue as far as the UX is concerned, given the speed of the movie API response time, but it'd be better to send one request rather than four for a string such as "star". Further, I didn't want to just resort to using something like Lodash's **debounce** for the sake of the test.
- One feature of the movie API is that images (or rather, a partial path) are available in individual movie objects. [As per the docs](https://developer.themoviedb.org/reference/configuration-details), there's a config endpoint you can hit to acquire the base URL and the file size for a give image, but the route requires adding the API token to the Authorization header as a bearer token. This didn't seem to be working. But it'd be nice to have pictures.
- Each movie object that comes back from 'https://api.themoviedb.org/3/search/movie?query=xxxx' contains an individual id that can be used to query for an expanded detail response. This could be implemented as the content on the /details page, rather than the base movie list object.
- Around testing, units were written for components using the automatically provided React Testing Library. I personally consider end-to-end/integration tests on single page apps to be almost mandatory, and as such would have liked to have added something like Cypress or Playwright.


Thank you for reviewing this test.
