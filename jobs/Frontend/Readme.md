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

## Submission
The following technologies have been used:

### Instructions to use

    npm i
    npm run dev <--- local 

### Tech stack
 - NextJS/React
 - Material/UI
 - Typescript
 - Jest/React Testing Library

##### NextJS/React
The decision to use NextJS was taken as it provided a lot of features out of the box which would reduce the delivery time of the app, e.g. build, testing, linting, SSR and local dev server to name a few.
##### Material/UI
Initially the build consisted of a couple of hand rolled components with custom styling. Given that there was no visual design to work from, it was decided to use the UI library as it comes with its own visual language. Furthermore, the brief, when communicated introduced, included  commentary around "production-ready" code; taking that view, it makes sense to make use of a UI component library as it brings with it code which has been thoroughly tested code, consistent design and decent a11y support.
#####  Typescript
Strongly typed language with all the benefits that brings.
##### Jest/React Testing Library
The automated units tests have been written using the very common testing libs.

### Design
The app uses a combination of client side and server side rendering. The first page makes use of both rendering techniques. When launched for the first time the first page will be delivered with only an input. Typing into input will trigger the API call to search movies, whilst in progress a spinner will be shown and when input has text then a clear icon will be displayed. The app route will reflect the current search term and page without triggering a reload. When the results are shown then the pagination controls will be shown.

Server side rendering (SSR) will be used if the first page is loaded with query parameters `query` and `page`set. The second page will be rendered with SSR only.

### Gaps
Due to time constraints the following is missing/ not working:
 - There is no debounce on input
 - There is no cancelling of in flight requests, potential API calls may overlap
 - There are no cache controls set
 - Scroll position on page change not reaching top of page
 - Image loading failures may still potentially fail, even though errors on initial image load is handled
 - Not 100% test coverage
 - Some types not fully expressed


