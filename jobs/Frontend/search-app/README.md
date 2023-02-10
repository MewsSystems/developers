### Live Demo

https://search-app-ste.herokuapp.com/

### Available Scripts

Available scripts in the project directory:

- `start` - serves the application locally
- `assemble` - checks file formatting and then builds the application
- `test` - runs unit tests
- `test-ui` - runs ui (visual) tests
- `test-e2e` - runs e2e tests
- `prettier-check` - checks files formatting
- `prettier-fix` - automatically fixes all file formatting issues

### Key features

- support for routing - searching and page browsing gets reflected in the routes
- debounce search - it doesn't trigger HTTP request after every key press
- automatic file formatting setup - when running assemble script, it fails when a file is incorrectly formatted, setting the script into the build pipeline would force the file formatting across the whole project
- application is fully set for testing - it uses jest unit tests, cypress components tests for visual tests of more complex components and cypress e2e tests running on the live application

#### Possible improvements

- adding eslint to extend prettier functionality
- extend test coverage
- fix page flickering - smother navigation to a different app states
- migrate to Next.js to extend the functionality of Create React App
