export type Genre = {
  id?: number;
  name?: string;
}

export type MovieDetail = {
  adult?: boolean;
  genres?: Genre[];
  homepage?: string;
  id?: number;
  "original_language"?: string;
  "original_title"?: string;
  overview?: string | null;
  release_date?: string;
  runtime?: number | null;
  poster_path?: string | null;
};