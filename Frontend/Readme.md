# Movie DB App

The app is consiting of 4 scenes, Home, Search, Detail and Page not found scenes.

Some data the app is using is stored in redux store using redux, redux-thunk
and redux-persist

To be more specific configuration, trending movies and search phrase
The rest is being loaded using axios (searched movies, movie detail and credits).
I decided not to store these things, because they tend to change very often.

On the other hand all trending movies are stored and persisted and rehydrated
every week. Configuration is stored and persisted as well and search phrase
is stored without persisting.

I also used react-18next for all copy that can be translated.
All translation files are located in public/locales/{language}.

Configuration file containing GA tracking code, base URI and api key for Movie DB api
is located in public/config/envConfig.js.

base URI and api key together with app language is being added in src/axiosConfig.ts
using axios interceptors.

Each page view is being tracked together with some events using react-ga.
To track pageview and events, I am using custom hooks useTrackEvent and useTrackPageView
located in scr/hooks.

Whole app is tested with jest/enzyme. It is also using travis-ci
(https://travis-ci.org/vojtechportes/developers) to run all tests, linters etc. with every
pushed commit. Config is placed in root of this repository.

## Installation

```
yarn
yarn start
```

## Testing

```
yarn test
yarn test:coverage
```

## Linting

```
yarn lint:ts
yarn lint:css
```