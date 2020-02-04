const API_KEY = "03b8572954325680265531140190fd2a";
const BASE_URL = "https://api.themoviedb.org/3";

export function createUrl(
  path: string,
  queryParams?: { [param: string]: string }
) {
  let result = `${BASE_URL}${encodeURI(path)}?api_key=${API_KEY}`;
  for (const param in queryParams) {
    result += `&${encodeURIComponent(param)}=${encodeURIComponent(
      queryParams[param]
    )}`;
  }

  return result;
}

export async function searchForMovie(query: string, pageNum?: number) {
  try {
    const queryParams: { [key: string]: string } = { query };
    if (pageNum) queryParams.page = String(pageNum);

    const json = await fetch(
      createUrl("/search/movie", queryParams)
    ).then(res => res.json());

    return json as {
      page: number;
      results: {
        id: number;
        poster_path: string | null;
        overview: string;
        release_date: string;
        title: string;
      }[];
      total_pages: number;
      total_results: number;
    };
  } catch (e) {
    console.error(e);
  }
}

export interface MovieInfo {
  genres: { id: number; name: string }[];
  homepage: string | null;
  id: number;
  imdb_id: string | null;
  original_language: string;
  original_title: string;
  overview: string | null;
  poster_path: string | null;
  release_date: string;
  runtime: number | null;
  spoken_languages: { iso_639_1: string; name: string }[];
  status:
    | "Rumored"
    | "Planned"
    | "In Production"
    | "Post Production"
    | "Released"
    | "Canceled";
  tagline: string;
  title: string;
}
export async function getMovieInfo(id: number) {
  try {
    const json = await fetch(createUrl(`/movie/${id}`)).then(res => res.json());

    return json as MovieInfo;
  } catch (e) {
    console.error(e);
  }
}

export function getImageUrl(imagePath: string) {
  const baseUrl = "https://image.tmdb.org/t/p/w154";
  return baseUrl + imagePath;
}
