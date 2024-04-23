export interface Genre {
  readonly id: number;
  readonly name: string;
}

export interface Movie {
  readonly id: number;
  readonly title: string;
  readonly poster_path: string;
  readonly original_title: string;
  readonly genres: Genre[];
  readonly origin_country: string[];
  readonly original_language: string;
  readonly overview: string;
}
