// Import the axios library for making HTTP requests
import axios from "axios";

// Set API_KEY using the environment variable
const API_KEY = import.meta.env.VITE_API_KEY;

// Set the base URL for the API requests
const BASE_URL = "https://api.themoviedb.org/3";

// This function searches for movies based on a query and page number
export const searchMovies = async (query: string, page: number = 1) => {
  // Send a GET request to the /search/movie endpoint of the API with the specified parameters
  const response = await axios.get(`${BASE_URL}/search/movie`, {
    params: {
      api_key: API_KEY,
      query,
      page,
    },
  });

  // If the requested page number exceeds the total number of pages, return an empty array
  if (response.data.total_pages < page) {
    return [];
  }

  // Return an array of movie results
  return response.data.results;
};

// This function loads more movies based on a query and page number
export const loadMoreMovies = async (query: string, page: number) => {
  // Build the URL for the API request
  const url = `${BASE_URL}/search/movie?api_key=${API_KEY}&query=${query}&page=${page}`;
  console.log("url", url);

  // Send a GET request to the specified URL
  const response = await axios.get(url);
  console.log("response", response);

  // Return an array of movie results
  return response.data.results;
};

// This function retrieves the details for a specific movie based on its ID
export const getMovie = async (id: string) => {
  console.log(id);

  // Send a GET request to the /movie/:id endpoint of the API with the specified parameters
  const response = await axios.get(`${BASE_URL}/movie/${id}`, {
    params: {
      api_key: API_KEY,
    },
  });

  // Return an object containing the details of the movie
  return response.data;
};
