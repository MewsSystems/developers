# Movie Search App

This is my Movie search application built with React and Redux for Mews. It allows users to search for movies, view movie details, and scroll through search results. The app uses the [The Movie Database (TMDb) API](https://www.themoviedb.org/documentation/api) as mentioned in the instructions to fetch movie data.

## Features

- Search for movies by title
- View movie details
- Infinite scroll to load more search results
- Responsive design for various screen sizes
- Handles the back functionality to resume your search where you left
- Controls the 404 page error
- Lets the user to select their favorite films

## Technologies Used

- React
- Redux
- React Router
- Material-UI
- Jest

## Installation and execution

To run this project locally, follow these steps:

1. Access to the project directory /mews-hometest-marcl
`cd /mews-hometest-marcl`

2. Install the dependencies

```bash
npm install
```

3. Create a .env file in the root directory and add your TMDb API key:

`REACT_APP_API_KEY=your_api_key_here`

4. Start the development server:

```bash
npm start
```

5. To run the tests:

```bash
npm test
```

## Improvements
This project can be improved with some of the following ideas:
- Adding further testing
- Adding the filtering for categories
- Creating a CD/CI for new features
- Implement a login / logout method
- Implement a method to clear LocalStorage (maybe when a logout occurs)
- Implement the functionality of setting the favorite in the MovieList component and not only on the details
... amongst others


## Usage
- Enter a movie title in the search bar and press Enter or click the search button.
- Scroll through the search results to load more movies.
- Click on a movie to view its details.
- Click on the heart if you wish to add the film into your favorites
- Use the back button to return to the search results.

Hope you like it!