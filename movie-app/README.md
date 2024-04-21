# Movie Search Application

This repository contains a solution for the Mews frontend developer task. The task involves creating a simple movie search application using React and TypeScript.

## Getting Started

To begin working on the project, follow these steps:

1. **Fork the Repository**: Start by forking this repository to your own GitHub account.

2. **Clone the Repository**: Clone the forked repository to your local machine.

3. **Install Dependencies**: Navigate to the project directory and install the necessary dependencies using npm or yarn:

    ```bash
    cd mews-frontend-task/movie-app
    npm install
    ```

4. **Start the Development Server**: Run the development server to see the project in action:

    ```bash
    npm run dev
    ```

   This command will start the development server at `http://localhost:5173`.
5. **Run test**: Run all available test using command 
    ```bash
   npm run test
    ```

## Folder Structure

The project follows a standard folder structure:

- `src/`: Contains the source code of the application.
    - `api/`: Contains integration logic for fetching [TheMovieDb API](https://developers.themoviedb.org/3/getting-started/introduction) api.
    - `components/`: Contains reusable components used across the application.
    - `hooks/`: Contains reusable hooks used across the application and supporting types definitions.
    - `layout/`: Contains layout component used as main UI wrapper around application.
    - `pages/`: Contains the main views of the application (search and movie detail).
    - `styles/`: Contains main definitions and setup for styling.
    - `tests/`: Contains test environment setup and mocks.
