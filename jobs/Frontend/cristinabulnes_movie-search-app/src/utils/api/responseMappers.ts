import { Genre, Movie } from "../../types";

export interface SearchMovieApiResponse {
	id: string;
	title: string;
	poster_path: string | null;
	release_date: string;
	vote_average: number;
	overview: string;
}

export interface MovieDetailsApiResponse extends SearchMovieApiResponse {
	genres: Genre[];
}

// Map raw API response to the `Movie` type for search results
export const mapFetchMoviesResponse = (
	rawMovie: SearchMovieApiResponse
): Movie => ({
	id: rawMovie.id,
	title: rawMovie.title || "Unknown Title",
	posterPath: rawMovie.poster_path || null,
	releaseDate: rawMovie.release_date || "N/A",
	voteAverage: Number(rawMovie.vote_average.toFixed(2)) ?? 0,
	overview: rawMovie.overview || "No overview available.",
});

// Map raw API response to the `Movie` type for movie details
export const mapFetchMovieDetailsResponse = (
	rawMovie: MovieDetailsApiResponse
): Movie => ({
	id: rawMovie.id,
	title: rawMovie.title,
	posterPath: rawMovie.poster_path,
	releaseDate: rawMovie.release_date,
	voteAverage: rawMovie.vote_average,
	genres: rawMovie.genres,
	overview: rawMovie.overview,
});
