import { MovieSummary, MovieSearchResult, Movie } from "@/types";
import { MovieDetailResponse, MovieServiceRespnse } from "../types";
import { IMAGE_DETAIL_SOURCE, IMAGE_THUMBNAIL_SOURCE } from "@/constants";

export const MovieByIdAdapter = (response: MovieDetailResponse): Movie =>
    ({
      id: response.id,
      title: response.title,
      overview: response.overview,
      posterImage: `${IMAGE_DETAIL_SOURCE}${response.poster_path}`,
      homepage: response.homepage,
      genres: response.genres.map((genre: {id: number; name: string }) => genre.name),
      spokenLanguages: response.spoken_languages
        .map((lang: {english_name: string; iso_639_1: string; name: string}) => lang.english_name),
    });
