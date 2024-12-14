# Movie Search Application

A responsive, clean and simple movie search application built with React and TypeScript that helps users discover and explore films through an intuitive interface.

## Features

- **Dynamic Search**: Dynamic search functionality were the search input automatically starts searching after typing is finished (via Debouncing). 
- **Results Display**: The results are displayed in a paginated list with an option to load additional batches after a short intentional delay.
- **Movie Detail**: Displays detailed info about a selected movie, together with the rating and the poster (when available).

## Technical Features

- **Dark theme toggle**: Toggle between dark and light themes, with dark mode as the default for reduced eye strain.
- **Context-Based Movie Cachingt**: Efficient storage of search results using React Context to minimize API calls.
- **Debounced Search**: Optimized search implementation that triggers only after users finish typing.
- **Design System**: Token-based styling architecture enabling consistent and maintainable design updates.

### What is next:
- Accesibilty can be further improved throught the application, making it more accesible to users with screen readers.
- More rigorous testing can be introduced to further cover edge cases that might be encountered in production. 
- Error states can be remimplemented to better inform the user and provide more information in case he contacts the maintainters
- Protection agains webscrapers could be introduced as to prevent api usage abuse
- And much more! There are always tiny things you can keep improving :D

### What is next:
- Accessibility: Expanding screen reader support and keyboard navigation
- Testing Coverage: Implementation of additional test cases to ensure robust functionality
- Error Handling: Enhanced error state management with improved user feedback
- API Protection: Implementation of rate limiting and scraping prevention
- And much more! There are always tiny things you can keep improving :D

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/janescorza/mews-take-home-task.git
    cd movie-search-app
    ```

2. Navigate to the frontend directory:
    ```sh
    cd jobs/Frontend/clean-movie-search-app
    ```

3. Install the dependencies:
    ```sh
    npm install
    ```

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!


