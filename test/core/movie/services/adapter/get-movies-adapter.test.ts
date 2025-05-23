import { getMoviesAdapter } from "@core/movie/services/adapter/get-movies-adapter";

describe('getMoviesAdapter', () => {
  const mockMoviesResponse = [
    {
      id: 123,
      original_title: 'Test Movie 1',
      overview: 'Test overview 1',
      popularity: 100,
      poster_path: '/test-poster-1.jpg',
      release_date: '2024-01-01',
      video: false,
      vote_average: 8.5,
      vote_count: 1000,
      backdrop_path: '/test-backdrop-1.jpg',
      runtime: 120,
      genre_ids: [1, 2, 3],
      original_language: 'en'
    },
    {
      id: 456,
      original_title: 'Test Movie 2',
      overview: 'Test overview 2',
      popularity: 200,
      poster_path: '/test-poster-2.jpg',
      release_date: '2024-01-02',
      video: true,
      vote_average: 7.5,
      vote_count: 500,
      backdrop_path: '/test-backdrop-2.jpg',
      runtime: 90,
      genre_ids: [4, 5],
      original_language: 'es'
    }
  ];

  it('should transform array of API responses to Movie array', () => {
    const result = getMoviesAdapter(mockMoviesResponse);

    expect(result).toEqual([
      {
        id: 123,
        title: 'Test Movie 1',
        overview: 'Test overview 1',
        popularity: 100,
        posterPath: '/test-poster-1.jpg',
        releaseDate: '2024-01-01',
        video: false,
        voteAverage: 8.5,
        voteCount: 1000,
        backdropPath: '/test-backdrop-1.jpg',
        runtime: 120,
        genreIds: [1, 2, 3],
        language: 'en'
      },
      {
        id: 456,
        title: 'Test Movie 2',
        overview: 'Test overview 2',
        popularity: 200,
        posterPath: '/test-poster-2.jpg',
        releaseDate: '2024-01-02',
        video: true,
        voteAverage: 7.5,
        voteCount: 500,
        backdropPath: '/test-backdrop-2.jpg',
        runtime: 90,
        genreIds: [4, 5],
        language: 'es'
      }
    ]);
  });

  it('should handle empty array', () => {
    const result = getMoviesAdapter([]);
    expect(result).toEqual([]);
  });

  it('should handle missing optional fields', () => {
    const partialResponse = [{
      id: 123,
      original_title: 'Test Movie',
      overview: 'Test overview',
      popularity: 100,
      poster_path: '/test-poster.jpg',
      release_date: '2024-01-01',
      video: false,
      vote_average: 8.5,
      vote_count: 1000,
      backdrop_path: null,
      original_language: 'en'
    }];

    const result = getMoviesAdapter(partialResponse);

    expect(result).toEqual([{
      id: 123,
      title: 'Test Movie',
      overview: 'Test overview',
      popularity: 100,
      posterPath: '/test-poster.jpg',
      releaseDate: '2024-01-01',
      video: false,
      voteAverage: 8.5,
      voteCount: 1000,
      backdropPath: null,
      language: 'en',
      runtime: undefined,
      genreIds: undefined
    }]);
  });
}); 