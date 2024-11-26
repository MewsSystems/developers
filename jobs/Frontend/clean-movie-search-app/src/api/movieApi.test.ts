import { searchMovies, getMovieDetails } from '../api/movieApi';

describe('moviesApi', () => {
  const mockMovieResponse = {
    page: 1,
    results: [
      {
        id: 1,
        title: 'Movie 1',
        overview: 'Overview of Movie 1',
        poster_path: '/poster1.jpg',
        release_date: '2024-11-01',
        vote_average: 8.5,
      },
      {
        id: 2,
        title: 'Movie 2',
        overview: 'Overview of Movie 2',
        poster_path: null,
        release_date: '2024-11-15',
        vote_average: 7.8,
      },
    ],
    total_pages: 2,
    total_results: 2,
  };

  const mockMovieDetails = {
    id: 1,
    title: 'Movie 1',
    overview: 'Detailed overview of Movie 1',
    poster_path: '/poster1.jpg',
    release_date: '2024-11-01',
    vote_average: 8.5,
  };

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
      'https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&query=test&page=2',
      expect.any(Object)
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
    const fetchMock = jest.spyOn(global, 'fetch').mockImplementation(() =>
      Promise.resolve({
        ok: false,
        status: 500,
      } as Response)
    );

    await expect(searchMovies('test')).rejects.toThrowError('HTTP Error: 500');

    expect(fetchMock).toHaveBeenCalledTimes(4); // 1 initial call + 3 retries
  });

  it('should fetch movie details with the correct movie ID', async () => {
    const fetchSpy = jest.spyOn(global, 'fetch').mockResolvedValueOnce({
      ok: true,
      json: async () => mockMovieDetails,
    } as Response);

    await getMovieDetails(1);

    expect(fetchSpy).toHaveBeenCalledWith(
      'https://api.themoviedb.org/3/movie/1?api_key=03b8572954325680265531140190fd2a',
      expect.any(Object)
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
      status: 404,
    } as Response);

    await expect(getMovieDetails(1)).rejects.toThrowError('HTTP Error: 404');
  });
});
