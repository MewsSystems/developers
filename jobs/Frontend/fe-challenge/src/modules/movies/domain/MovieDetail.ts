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
  productionCompanies: Array<string>;
}

export class MovieDetail extends Movie {
  private readonly _genres: Array<string>;
  private readonly _cast: Array<CastPerson>;
  private readonly _directors: Array<string>;
  private readonly _runtime: number;
  private readonly _productionCompanies: Array<string>;

  constructor({
    genres,
    cast,
    directors,
    runtime,
    productionCompanies,
    ...rest
  }: MovieDetailData) {
    super(rest);
    this._genres = genres;
    this._cast = cast;
    this._directors = directors;
    this._runtime = runtime;
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

  get runtime() {
    return this._runtime;
  }

  get productionCompanies() {
    return this._productionCompanies;
  }
}
