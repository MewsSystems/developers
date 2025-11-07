# Movie searcher

Live link: https://developers-juan-sierras-projects-2885f3b2.vercel.app/

## Description

This is a movie searcher app that allows you to search for movies by title and see the details of each movie. At the moment, it only supports the English language.

## Setup

We are using yarn to set up and run this application. In order to get started, you need to setup the environment variables.

### Environment variables

Create a `.env.local` file in the root of the project and add the following variables:

```bash
VITE_API_KEY = YOUR_API_KEY
VITE_API_URL =  https://api.themoviedb.org/3/
```

We are fetching the movies from the [Movie Database](https://www.themoviedb.org/) website. Where you can get your API key.

### Install dependencies

```bash
yarn install
```

### Run the application

```bash
yarn dev
```

### Run the tests

We used Playwright to run the tests. To run the tests, run the following command:

```bash
yarn test
```

In case you want to run the tests in a UI mode, run the following command:

```bash
yarn test:ui
```
