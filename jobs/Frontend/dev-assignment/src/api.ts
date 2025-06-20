const API_KEY = '03b8572954325680265531140190fd2a';
const BASE = 'https://api.themoviedb.org/3';

export const fetchMovies = (query: string, page = 1) =>
  fetch(`${BASE}/search/movie?api_key=${API_KEY}&query=${query}&page=${page}`)
    .then(res => res.json());

export const fetchMovie = (id: string) =>
  fetch(`${BASE}/movie/${id}?api_key=${API_KEY}`)
    .then(res => res.json());