export const mockMoviesListResponseByTMDB = {
    results: [
      {
        id: 123,
        original_title: 'Test Movie 1',
        overview: 'Test overview 1',
        poster_path: '/test-poster-1.jpg',
        release_date: '2024-01-01',
        vote_average: 8.5,
        vote_count: 1000,
        popularity: 100,
        backdrop_path: '/test-backdrop-1.jpg',
        original_language: 'en',
        video: false,
        runtime: 120
      },
      {
        id: 456,
        original_title: 'Test Movie 2',
        overview: 'Test overview 2',
        poster_path: '/test-poster-2.jpg',
        release_date: '2024-01-02',
        vote_average: 7.5,
        vote_count: 500,
        popularity: 50,
        backdrop_path: '/test-backdrop-2.jpg',
        original_language: 'es',
        video: false,
        runtime: 90
      }
    ],
    page: 1,
    total_pages: 10,
    total_results: 100
  };