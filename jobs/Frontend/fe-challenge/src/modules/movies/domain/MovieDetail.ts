import { Movie, MovieData } from '@/modules/movies/domain/Movie';

export interface CastPerson {
  characterName: string;
  originalName: string;
  profileImage?: string;
}

interface MovieDetailData extends MovieData {
  genres: Array<string>;
  cast: Array<CastPerson>;
  directors: Array<string>;
  runtime: number;
  country: string;
  tagline: string;
  imdbId?: string;
  productionCompanies: Array<string>;
}

export class MovieDetail extends Movie {
  private readonly _genres: Array<string>;
  private readonly _cast: Array<CastPerson>;
  private readonly _directors: Array<string>;
  private readonly _runtime: number;
  private readonly _country: string;
  private readonly _tagline: string;
  private readonly _imdbId?: string;
  private readonly _productionCompanies: Array<string>;

  constructor({
    genres,
    cast,
    directors,
    runtime,
    country,
    tagline,
    imdbId,
    productionCompanies,
    ...rest
  }: MovieDetailData) {
    super(rest);
    this._genres = genres;
    this._cast = cast;
    this._directors = directors;
    this._runtime = runtime;
    this._imdbId = imdbId;
    this._country = country;
    this._tagline = tagline;
    this._productionCompanies = productionCompanies;
  }

  get genres() {
    return this._genres;
  }

  get cast() {
    return this._cast;
  }

  get directors() {
    return this._directors;
  }

  get directorsFormatted() {
    return this._directors.join(', ');
  }

  get runtime() {
    return this._runtime;
  }

  get imdbId() {
    return this._imdbId;
  }

  get imdbURL() {
    return this._imdbId
      ? `https://www.imdb.com/title/${this._imdbId}`
      : undefined;
  }

  get country() {
    return this._country;
  }

  get tagline() {
    return this._tagline;
  }

  get productionCompanies() {
    return this._productionCompanies;
  }
}
