export const searchMovies = async (searchTerm: string): Promise<any> => {
  let url = 'https://api.themoviedb.org/3/search/movie?query='+searchTerm+'&include_adult=false&language=en-US&page=2&api_key='+process.env.REACT_APP_TMDB_API_KEY;
  const response = await fetch(url, {method: 'GET', headers: {accept: 'application/json'}});

  if (!response.ok) {
    throw new Error("Failed to fetch movies");
  }
  return response.json();
};