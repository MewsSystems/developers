//authentication for API
const tmdbApiKey = '03b8572954325680265531140190fd2a';

export const fetchMovies = async (movieKeyword: string, page: number) => {
  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${tmdbApiKey}&query=${movieKeyword}`
  );
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json();
};
