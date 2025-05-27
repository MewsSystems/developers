import { http, HttpResponse } from 'msw';

const TMDB_API_URL = 'https://api.themoviedb.org/3';

export const handlers = [
  // Get movie by ID
  http.get(`${TMDB_API_URL}/movie/:id`, () => {
    return HttpResponse.json({
      id: 1,
      title: 'Test Movie',
      overview: 'Test overview',
      poster_path: '/test-poster.jpg',
      backdrop_path: '/test-backdrop.jpg',
      release_date: '2024-01-01',
      vote_average: 8.5,
      vote_count: 1000,
      genre_ids: [1, 2, 3],
      adult: false,
      original_language: 'en',
      original_title: 'Test Movie',
      popularity: 100,
      video: false,
      runtime: 120,
      genres: [
        { id: 1, name: 'Action' },
        { id: 2, name: 'Drama' }
      ]
    });
  }),

  // Search movies
  http.get(`${TMDB_API_URL}/search/movie`, () => {
    return HttpResponse.json({
      page: 1,
      results: [
        {
          id: 1,
          title: 'Test Movie',
          overview: 'Test overview',
          poster_path: '/test-poster.jpg',
          backdrop_path: '/test-backdrop.jpg',
          release_date: '2024-01-01',
          vote_average: 8.5,
          vote_count: 1000,
          genre_ids: [1, 2, 3],
          adult: false,
          original_language: 'en',
          original_title: 'Test Movie',
          popularity: 100,
          video: false
        }
      ],
      total_pages: 1,
      total_results: 1
    });
  }),

  // Get popular movies
  http.get(`${TMDB_API_URL}/movie/popular`, () => {
    return HttpResponse.json({
      page: 1,
      results: [
        {
          id: 1,
          title: 'Test Movie',
          overview: 'Test overview',
          poster_path: '/test-poster.jpg',
          backdrop_path: '/test-backdrop.jpg',
          release_date: '2024-01-01',
          vote_average: 8.5,
          vote_count: 1000,
          genre_ids: [1, 2, 3],
          adult: false,
          original_language: 'en',
          original_title: 'Test Movie',
          popularity: 100,
          video: false
        }
      ],
      total_pages: 1,
      total_results: 1
    });
  })
]; 