export interface MovieData {
  id: number;
  title: string;
  overview: string;
  voteAverage: number;
  releaseDate: string;
  backdropImage: string;
  posterImage: string;
}

export class Movie {
  private readonly _id: number;
  private readonly _title: string;
  private readonly _overview: string;
  private readonly _voteAverage: number;
  private readonly _releaseDate: string;
  private readonly _backdropImage: string;
  private readonly _posterImage: string;

  constructor({
    id,
    title,
    overview,
    voteAverage,
    releaseDate,
    backdropImage,
    posterImage,
  }: MovieData) {
    this._id = id;
    this._title = title;
    this._overview = overview;
    this._voteAverage = voteAverage;
    this._releaseDate = releaseDate;
    this._backdropImage = backdropImage;
    this._posterImage = posterImage;
  }

  get id() {
    return this._id;
  }

  get title() {
    return this._title;
  }

  get overview() {
    return this._overview;
  }

  get voteAverage() {
    return this._voteAverage;
  }

  get releaseDate() {
    return this._releaseDate;
  }

  get backdropImage() {
    return this._backdropImage;
  }

  get posterImage() {
    return this._posterImage;
  }
}
