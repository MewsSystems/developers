import { HttpRepository } from '@/modules/movies/domain/HttpRepository';
import { Movie, MovieData } from '@/modules/movies/domain/Movie';
import { CastPerson, MovieDetail } from '@/modules/movies/domain/MovieDetail';
import { MovieRepository } from '@/modules/movies/domain/MovieRepository';
import { MovieSearchResult } from '@/modules/movies/domain/MovieSearchResult';
import { MovieDTO } from '@/modules/movies/infrastructure/http/dto/MovieDTO';
import {
  CastDTO,
  MovieDetailDTO,
} from '@/modules/movies/infrastructure/http/dto/MovieDetailDTO';
import { MovieResultDTO } from '@/modules/movies/infrastructure/http/dto/MovieResultDTO';

const API_BASE_URL = 'https://api.themoviedb.org/3';
const API_IMAGE_PATH = 'https://image.tmdb.org/t/p';
const DIRECTOR_JOB = 'Director';

const API_KEY = import.meta.env.VITE_TMDB_API_KEY;

type BackdropSizes = 'w300' | 'w780' | 'w1280' | 'original';
type PosterSizes =
  | 'w92'
  | 'w154'
  | 'w185'
  | 'w342'
  | 'w500'
  | '780'
  | 'original';
type ProfileSizes = 'w45' | 'w185' | 'original';

export class TMDBMovieRepository implements MovieRepository {
  private readonly http: HttpRepository;

  constructor(httpRepository: HttpRepository) {
    this.http = httpRepository;
  }

  async search(query: string, page: number): Promise<MovieSearchResult> {
    const moviesResult = await this.http.get<MovieResultDTO>(
      `${API_BASE_URL}/search/movie`,
      { query, page, include_adult: false, api_key: API_KEY },
    );

    return {
      page: moviesResult.page,
      results: moviesResult.results.map((movie) => this.parseMovieDTO(movie)),
      totalPages: moviesResult.total_pages,
      totalResults: moviesResult.total_results,
    };
  }

  private parseMovieDTO(movie: MovieDTO): Movie {
    return new Movie(this.parseMovieFields(movie));
  }

  private parseMovieFields(movieDTO: MovieDTO | MovieDetailDTO): MovieData {
    return {
      id: movieDTO.id,
      title: movieDTO.title,
      overview: movieDTO.overview,
      voteAverage: movieDTO.vote_average,
      releaseDate: movieDTO.release_date,
      backdropImage: this.buildBackdropImageURL(movieDTO.backdrop_path, 'w780'),
      posterImage: this.buildPosterImageURL(movieDTO.poster_path, 'w500'),
    };
  }

  private buildImageURL(
    imagePath: string,
    imageSize: BackdropSizes | PosterSizes | ProfileSizes,
  ): string | undefined {
    return imagePath
      ? `${API_IMAGE_PATH}/${imageSize}/${imagePath}`
      : undefined;
  }

  private buildBackdropImageURL(imagePath: string, imageSize: BackdropSizes) {
    return this.buildImageURL(imagePath, imageSize);
  }

  private buildPosterImageURL(imagePath: string, imageSize: PosterSizes) {
    return this.buildImageURL(imagePath, imageSize);
  }

  async getDetail(id: number): Promise<MovieDetail> {
    const movie = await this.http.get<MovieDetailDTO>(
      `${API_BASE_URL}/movie/${id}`,
      {
        api_key: API_KEY,
      },
    );

    return this.parseMovieDetailDTO(movie);
  }

  private parseMovieDetailDTO(movieDetail: MovieDetailDTO): MovieDetail {
    return new MovieDetail({
      ...this.parseMovieFields(movieDetail),
      genres: movieDetail.genres.map((genre) => genre.name),
      runtime: movieDetail.runtime,
      cast: this.buildMovieDetailCast(movieDetail.credits.cast),
      directors: movieDetail.credits.crew
        .filter((crew) => crew.job === DIRECTOR_JOB)
        .map((directors) => directors.name),
      productionCompanies: movieDetail.production_companies.map(
        (company) => company.name,
      ),
    });
  }

  private buildMovieDetailCast(cast: Array<CastDTO>): Array<CastPerson> {
    const firstSix = cast
      .slice()
      .sort((a, b) => a.order - b.order)
      .slice(0, 7);

    return firstSix.map((person) => ({
      characterName: person.name,
      originalName: person.original_name,
      profileImage: this.buildProfileImageURL(person.profile_path, 'w185'),
    }));
  }

  private buildProfileImageURL(imagePath: string, imageSize: ProfileSizes) {
    return this.buildImageURL(imagePath, imageSize);
  }
}
