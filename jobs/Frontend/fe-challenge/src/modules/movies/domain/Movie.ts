import { formatDate } from '@/modules/movies/domain/utils/formatDate';

export interface MovieData {
  id: number;
  title: string;
  originalTitle?: string;
  overview: string;
  voteAverage: number;
  voteCount: number;
  releaseDate: string;
  backdropImage?: string;
  posterImage?: string;
}

export class Movie {
  private readonly _id: number;
  private readonly _title: string;
  private readonly _originalTitle?: string;
  private readonly _overview: string;
  private readonly _voteAverage: number;
  private readonly _voteCount: number;
  private readonly _releaseDate: string;
  private readonly _backdropImage?: string;
  private readonly _posterImage?: string;

  constructor({
    id,
    title,
    originalTitle,
    overview,
    voteAverage,
    voteCount,
    releaseDate,
    backdropImage,
    posterImage,
  }: MovieData) {
    this._id = id;
    this._title = title;
    this._originalTitle = originalTitle;
    this._overview = overview;
    this._voteAverage = voteAverage;
    this._voteCount = voteCount;
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

  get originalTitle() {
    return this._originalTitle;
  }

  get overview() {
    return this._overview;
  }

  get voteAverage() {
    return this._voteAverage;
  }

  get voteAveragePercent() {
    return Math.round((this._voteAverage * 100) / 10);
  }

  get voteCount() {
    return this._voteCount;
  }

  get releaseDate() {
    return this._releaseDate;
  }

  get releaseDateFormatted() {
    return this._releaseDate ? formatDate(this._releaseDate) : '';
  }

  get releaseYear() {
    return this._releaseDate
      ? formatDate(this._releaseDate, { year: 'numeric' })
      : '';
  }

  get backdropImage() {
    return this._backdropImage;
  }

  get posterImage() {
    return this._posterImage;
  }
}
