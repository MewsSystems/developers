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

# MewsFlix Movie App

A simple movie search application. By default the user can see the list of popular movies. On search they can refine the list and search for the movie that they are looking for. They can click on each movie card to see the details of the movie on a new page or scroll through the list to load more.

```sh
npm install
```

## Dev

```sh
npm run dev
```

## Test

```sh
npm test
```

[Based on vite react template](README.VITE.md)
