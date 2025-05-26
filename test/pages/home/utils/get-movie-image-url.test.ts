import { getMovieImageUrl } from '../../../../src/pages/home/utils/get-movie-image-url';

describe('getMovieImageUrl', () => {
  const baseURL = 'https://image.tmdb.org/t/p/w500';

  it('should return the complete image URL', () => {
    const moviePoster = { posterPath: '/poster.jpg' };
    expect(getMovieImageUrl(baseURL, moviePoster)).toBe('https://image.tmdb.org/t/p/w500/poster.jpg');
  });

  it('should handle empty poster path', () => {
    const moviePoster = { posterPath: '' };
    expect(getMovieImageUrl(baseURL, moviePoster)).toBe('https://image.tmdb.org/t/p/w500');
  });

  it('should handle different base URLs', () => {
    const moviePoster = { posterPath: '/poster.jpg' };
    const differentBaseURL = 'https://example.com/images/';
    expect(getMovieImageUrl(differentBaseURL, moviePoster)).toBe('https://example.com/images//poster.jpg');
  });
}); 