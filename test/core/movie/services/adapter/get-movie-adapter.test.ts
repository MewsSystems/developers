import { getMovieDetailsAdapter } from "@core/movie/services/adapter/get-movie-adapter";

describe('getMovieDetailsAdapter', () => {
  const mockMovieResponse = {
    id: 123,
    title: 'Test Movie',
    overview: 'Test overview',
    poster_path: '/test-poster.jpg',
    release_date: '2024-01-01',
    vote_average: 8.5,
    vote_count: 1000,
    popularity: 100,
    backdrop_path: '/test-backdrop.jpg',
    original_language: 'en',
    video: false,
    runtime: 120,
    genre_ids: [1, 2, 3]
  };

  it('should transform API response to Movie type', () => {
    const result = getMovieDetailsAdapter(mockMovieResponse);

    expect(result).toEqual({
      id: 123,
      title: 'Test Movie',
      overview: 'Test overview',
      posterPath: '/test-poster.jpg',
      releaseDate: '2024-01-01',
      voteAverage: 8.5,
      voteCount: 1000,
      popularity: 100,
      backdropPath: '/test-backdrop.jpg',
      language: 'en',
      video: false,
      runtime: 120,
      genreIds: [1, 2, 3]
    });
  });

  it('should handle missing optional fields', () => {
    const partialResponse = {
      id: 123,
      title: 'Test Movie',
      overview: 'Test overview',
      poster_path: '/test-poster.jpg',
      release_date: '2024-01-01',
      vote_average: 8.5,
      vote_count: 1000,
      popularity: 100,
      backdrop_path: null,
      original_language: 'en',
      video: false
    };

    const result = getMovieDetailsAdapter(partialResponse);

    expect(result).toEqual({
      id: 123,
      title: 'Test Movie',
      overview: 'Test overview',
      posterPath: '/test-poster.jpg',
      releaseDate: '2024-01-01',
      voteAverage: 8.5,
      voteCount: 1000,
      popularity: 100,
      backdropPath: null,
      language: 'en',
      video: false,
      runtime: undefined,
      genreIds: undefined
    });
  });
}); 