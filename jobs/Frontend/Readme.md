# Mews frontend developer task

You should start with creating a fork of the repository. When you're finished with the task, you should create a pull request.

Your task will be to create a simple movie search application. The application will have 2 views - search and movie detail. The search view is the default view, and should contain search input and display paginated list of found movies with a way to load additional batch. Search should start automatically after typing into the input is finished - there is no need for a search button. Clicking on a movie gets you to the movie detail view where detailed information about the movie should be listed.

To retrieve information about movies, use [TheMovieDb API](https://developers.themoviedb.org/3/getting-started/introduction). You can use our api key to authorize requests:

```
03b8572954325680265531140190fd2a
```

## Required technologies

To test your proficiency with technologies we use the most, we require the solution to be written with use of TypeScript, React, Redux and styled-components. Use of any additional libraries is allowed and it's up to you.

## Implementation remarks

### Starters

To implement this web-app, I decided to use [vite](https://vitejs.dev/), so that I'd get a quick development environment. I also went with [vitest](https://vitest.dev/) instead of `jest` for the sake of configuration simplicity.

### Structure

I tried to organize my folders structure in a way similar to how I would do it in larger projects:

1. `components` folder holds generic presentational components, that hold little to no logic within.
2. `containers` folder holds the logical parts, separated by domain (in this case, only movies). I typically include in this folder the state and it's logics, side-effects, utils functions that are only pertinent to a specific domain and reusable components that is touching the global state for this domain.
3. `pages` folder holds, essentially, an entry per each route in the app. It knows how to render the pages layouts and which containers and components to render.
4. `utils` folder holds utility and helper methods.
5. `api`, although is just a file here, could be a folder, if more configuration would be required (in a graphql context, for instance).

### Shortcuts taken

I took some shortcuts to write some of the components in order to spend less time there. Here are a few of them:

- I didn't handle errors as well as I could. Instead, I simply set an error state and added a very fancy "something went wrong" message on the `Search` page, and not even that on the `Details` page.
- I shamelessly copied the `debounce` function from somewhere on the web. I typically don't like installing a dependency for a single (and rather simple) method like `debouce`.
- I also copied the Loader component (well, technically it's CSS) from somewhere in the web. I realize now that I should've kept the references for credits, but, unfortunately, they're gone now.
- I followed a less structured approach to the development, having to do a lot of "code dump". Since I didn't have the chance to work for hours straight on the app and could only do 30 minutes (tops) of heads down time at a time, I couldn't come up with consice commits. Instead, I ended up with this single commit. Sorry!
- There are some code repetitions that I managed to abstract, but I left some behind.
- I was a bit inconsistent with having the styled-components in same file or extracted to a separate file. The truth is that I don't have a strong feeling about either approach, and just wanted to show that I'm ok with either one.

### Time spent

I'd say that I spent, in total, around 4-5h in this project, but, as I mentioned before, unfortunately I couldn't do it in deep focus mode.

## Running the app

Install the dependecies by running

```sh
yarn install
```

After that, to get your local server running, run

```sh
yarn start
```

Now, navigate to localhost:3000 to see the app running. That's it!