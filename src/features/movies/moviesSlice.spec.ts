import moviesReducer, {MoviesState, clearMovies} from './moviesSlice';

describe('movies reducer', () => {
  const initialState: MoviesState = {
    movies: [
      {
        adult: false,
        backdrop_path: '/iQFcwSGbZXMkeyKrxbPnwnRo5fl.jpg',
        genre_ids: [28, 12, 878],
        id: 634649,
        original_language: 'en',
        original_title: 'Spider-Man: No Way Home',
        overview:
          'Peter Parker is unmasked and no longer able to separate his normal life from the high-stakes of being a super-hero. When he asks for help from Doctor Strange the stakes become even more dangerous, forcing him to discover what it truly means to be Spider-Man.',
        popularity: 11147.672,
        poster_path: '/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg',
        release_date: '2021-12-15',
        title: 'Spider-Man: No Way Home',
        video: false,
        vote_average: 8.4,
        vote_count: 7683,
      },
    ],
    status: 'idle',
    total_pages: 1,
    total_results: 10,
    error: null,
  };
  it('should handle initial state', () => {
    expect(moviesReducer(undefined, {type: 'unknown'})).toEqual({
      movies: [],
      status: 'idle',
      error: null,
      total_pages: 0,
      total_results: 0,
    });
  });

  it('should handle clear movies', () => {
    const actual = moviesReducer(initialState, clearMovies());
    expect(actual).toEqual({
      movies: [],
      status: 'idle',
      error: null,
      total_pages: 0,
      total_results: 0,
    });
  });
});
