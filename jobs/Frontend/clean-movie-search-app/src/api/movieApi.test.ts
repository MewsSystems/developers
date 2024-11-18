import { searchMovies, getMovieDetails } from '../api/movieApi';

describe('movieApi', () => {
  const mockMovieResponse = {
    results: [
      { id: 1, title: 'Movie 1' },
      { id: 2, title: 'Movie 2' },
    ],
    total_pages: 2,
  };

  const mockMovieDetails = { id: 1, title: 'Movie 1 Details' };

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should search for movies with the correct query and page', async () => {
    const fetchSpy = jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: true,
      json: async () => mockMovieResponse,
    } as Response);

    await searchMovies('test', 2);

    expect(fetchSpy).toHaveBeenCalledWith(
      'https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&query=test&page=2'
    );
  });

  it('should handle successful movie search response', async () => {
    jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: true,
      json: async () => mockMovieResponse,
    } as Response);

    const result = await searchMovies('test');

    expect(result).toEqual(mockMovieResponse);
  });

  it('should handle failed movie search response', async () => {
    jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: false,
    } as Response);

    await expect(searchMovies('test')).rejects.toThrowError(
      'Failed to fetch movies'
    );
  });

  it('should fetch movie details with the correct movie ID', async () => {
    const fetchSpy = jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: true,
      json: async () => mockMovieDetails,
    } as Response);

    await getMovieDetails(1);

    expect(fetchSpy).toHaveBeenCalledWith(
      'https://api.themoviedb.org/3/movie/1?api_key=03b8572954325680265531140190fd2a'
    );
  });

  it('should handle successful movie details response', async () => {
    jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: true,
      json: async () => mockMovieDetails,
    } as Response);

    const result = await getMovieDetails(1);

    expect(result).toEqual(mockMovieDetails);
  });

  it('should handle failed movie details response', async () => {
    jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: false,
    } as Response);

    await expect(getMovieDetails(1)).rejects.toThrowError(
      'Failed to fetch movie details'
    );
  });
});