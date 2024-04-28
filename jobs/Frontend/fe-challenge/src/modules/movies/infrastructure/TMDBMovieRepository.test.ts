import { FetchHttpRepository } from '@/modules/movies/infrastructure/FetchHttpRepository';
import { server } from '@/mocks/server';
import { TMDBMovieRepository } from '@/modules/movies/infrastructure/TMDBMovieRepository';
import { movieDetailMock, moviesResultMock } from '@/mocks/data';
import { MovieDTO } from '@/modules/movies/infrastructure/http/dto/MovieDTO';
import { Movie } from '@/modules/movies/domain/Movie';
import { MovieDetail } from '@/modules/movies/domain/MovieDetail';

describe('TMDBMovieRepository', () => {
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  const http = new FetchHttpRepository();

  test('should fetch movies by page number', async () => {
    const apiMovieRepository = new TMDBMovieRepository(http);

    const movies = await apiMovieRepository.search('Batman', 1);

    expect(movies).toEqual({
      page: moviesResultMock[0].page,
      results: moviesResultMock[0].results.map((movieDTO: MovieDTO) => {
        return new Movie({
          id: movieDTO.id,
          title: movieDTO.title,
          originalTitle: movieDTO.original_title,
          overview: movieDTO.overview,
          voteAverage: movieDTO.vote_average,
          voteCount: movieDTO.vote_count,
          releaseDate: movieDTO.release_date,
          backdropImage: `https://image.tmdb.org/t/p/w780/${movieDTO.backdrop_path}`,
          posterImage: `https://image.tmdb.org/t/p/w500/${movieDTO.poster_path}`,
        });
      }),
      totalPages: moviesResultMock[0].total_pages,
      totalResults: moviesResultMock[0].total_results,
    });

    const moviesPage2 = await apiMovieRepository.search('Batman', 2);

    expect(moviesPage2).toEqual({
      page: moviesResultMock[1].page,
      results: moviesResultMock[1].results.map((movieDTO: MovieDTO) => {
        return new Movie({
          id: movieDTO.id,
          title: movieDTO.title,
          originalTitle: movieDTO.original_title,
          overview: movieDTO.overview,
          voteAverage: movieDTO.vote_average,
          voteCount: movieDTO.vote_count,
          releaseDate: movieDTO.release_date,
          backdropImage: `https://image.tmdb.org/t/p/w780/${movieDTO.backdrop_path}`,
          posterImage: `https://image.tmdb.org/t/p/w500/${movieDTO.poster_path}`,
        });
      }),
      totalPages: moviesResultMock[1].total_pages,
      totalResults: moviesResultMock[1].total_results,
    });
  });

  test('should fetch movie detail', async () => {
    const apiMovieRepository = new TMDBMovieRepository(http);

    const movie = await apiMovieRepository.getDetail(1);
    const movieMock = movieDetailMock;

    expect(movie).toEqual(
      new MovieDetail({
        id: movieMock.id,
        title: movieMock.title,
        originalTitle: movieMock.original_title,
        overview: movieMock.overview,
        tagline: movieMock.tagline,
        country: movieMock.origin_country.join(', '),
        voteAverage: movieMock.vote_average,
        voteCount: movieMock.vote_count,
        releaseDate: movieMock.release_date,
        imdbId: movieMock.imdb_id,
        backdropImage: `https://image.tmdb.org/t/p/w780/${movieMock.backdrop_path}`,
        posterImage: `https://image.tmdb.org/t/p/w500/${movieMock.poster_path}`,
        genres: movieMock.genres.map((genre) => genre.name),
        runtime: movieMock.runtime,
        cast: expect.any(Array),
        directors: expect.any(Array),
        productionCompanies: expect.any(Array),
      }),
    );
  });
});
