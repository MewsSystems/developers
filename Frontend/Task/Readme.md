## Exchange Rate Client

### Technical overwiew

The app is processed by **Webpack 4**, it:
- integrates React and Babel
- adds support to SCSS modules
- minifies and extracts styles
- implements hot reload development server
- adds support to absolute paths

App logic is fully based on **React v16** (with Hooks), **React Router** and **Redux**. Some basic tests are handled by **Jest** (Enzyme support is added though). The graphical part uses modular **SCSS** with **normalize.css**, it has very simple RWD support. The configuration and filters data are stored in a **localStorage**.

### Folders structure

- **production_server** - a simple server file to serve production version
- **public** - root folder
- **server** - backed structure
- **src** - development assets
- **src\actions**  -- Redux actions
- **src\components**  -- React components
- **src\helperFunctions**  -- functions whose logic is abstracted away from components
- **src\reducers**  -- Redux reducers
- **src\routes**  -- React Router configuration
- **src\store**  -- Redux store configuration
- **src\styles**  -- SCSS files
- **src\tests**  -- tests content
- **src\app.js** - entry file

### Scripts
- `yarn install` - installs the app
- `build` - creates a production version via Webpack
- `serve` - serves a production version on port 3001
- `start-server` - starts backend server on port 3000
- `start` - starts Webpack development server on port 8080
- `test` - runs Jest test suite

### What I have learnt
- I discovered defaultChecked input property
- I practised React Hooks use
- I practised working with asynchronous code



