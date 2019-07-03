# Documentation

## Setup

All the dependencies were updated. webpack.config.js fixed to work with the latest webpack version.
Added code quality control tools (tslint, eslint).

### Avaliable scripts

```
  "start-server": "node server",
  "start": "webpack-dev-server --hot --inline --progress",
  "dev": "concurrently \"node server\" \"webpack-dev-server --hot --inline --progress\"",
  "lint": "eslint --fix . && echo 'Lint complete.'",
  "build": "webpack"
```

I've added concurrently to Dev Dependencies to run both servers with single command (`dev`).

### Additional packages

Typescript support added (with all `@types/` coming with it). ES lint and TS lint are there for code quality control. `Babel` was updated to the latest version and empovered by additional plugins and presets.
`ramda` package was added to work easier with object transformation. `qs` is used to transform array to query string.
This application makes use of `fetch`, so to make sure it works on IE (just in case someone travels forward in time from 1999) I added `isomorphic-fetch` with it's peer dependencies.
`redux` is used with `redux-saga` for assync functions.
To make use of `styled-components` and CSS in general I installed corresponding packages. I decided not to go with `css-modules`, as I find them not that much scalable and flexible as `styled-components`.

### Persistant data

Once config recieved from the endpoint `redux-saga` puts it to `local-storage`, so it can be retrieved even after app restart.

### Redux store

I've put everything to the store. It's not the best practise and if you ask me I'd rather kept most (if not all data) out of the global store. This app uses technologies that are overkill.
Store structure is as follows:

```javascript
store {
config,
rates,
filtered
}
```

I think this is pretty straight forward, so no need to go deeper.

# That's it! Thank you for your attention!
