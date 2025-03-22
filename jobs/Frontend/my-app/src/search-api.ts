//authentication for API
const tmdbApiKey = '03b8572954325680265531140190fd2a';

export const fetchMovies = async (movieKeyword: string, offset: number) => {
  const moviesPerPage = 20;
  const fetchPage = Math.floor(offset / moviesPerPage) + 1;
  const indexInPage = offset % moviesPerPage;

  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${tmdbApiKey}&query=${movieKeyword}&page=${fetchPage}`
  );
  const data = await response.json();
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return data.results.slice(indexInPage, indexInPage + 10);
};
