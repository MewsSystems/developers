# Task Implementation Documentation
Hi! I'm Sam and this is how I implemented this task. In this short piece of documentation I would like to detail how I did it and how to run the example.

If you would like to preview the working app you can do so [here](https://samuel-bohovic-mews.netlify.app/).

## Setup and commands
The project is set up with Vite.js using the Typescript+React template. There is also a test setup using vitest which includes a coverage generator.
And as you saw above I set up a pipeline on Netlify for deployments, which includes any PRs made on my fork.

To run the app locally:
```
yarn install
yarn dev
```
To run the tests with watch which will also open a GUI in your browser to show test status and coverage:
```
yarn test
```
and without watch:
```
yarn test-noui
```

## Implementation
Since the task was to create a simple application I decided to go with the "Build the MVP and then refine requirements as you go" approach. What you are seeing is the MVP product based on the requirements provided which is ready to be extended based on feedback(hence why the somewhat convoluted folder structure).

Once this MVP was implemented I started covering it with tests, normally I would go with TDD but I set up the test framework after building the app in the first place in this case. During this I also did some clean up on components and actually fixed some bugs! 

# Mews frontend developer task

You should start with creating a fork of the repository. When you're finished with the task, you should create a pull request.

Your task will be to create a simple movie search application. The application will have 2 views - search and movie detail. The search view is the default view, and should contain search input and display paginated list of found movies with a way to load additional batch. Search should start automatically after typing into the input is finished - there is no need for a search button. Clicking on a movie gets you to the movie detail view where detailed information about the movie should be listed. 

To retrieve information about movies, use [TheMovieDb API](https://developers.themoviedb.org/3/getting-started/introduction). You can use our api key to authorize requests:
```
03b8572954325680265531140190fd2a
```

## Required technologies

To test your proficiency with technologies we use the most, we require the solution to be written with use of TypeScript, React, Redux and styled-components. Use of any additional libraries is allowed and it's up to you.
