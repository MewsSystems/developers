import { Movie } from '../types/movie';


export const getMovieDetailsAdapter = (movie: any): Movie => {
  return {
    id: movie.id,
    title: movie.title,
    overview: movie.overview,
    posterPath: movie.poster_path,  
    releaseDate: movie.release_date,
    voteAverage: movie.vote_average,
    voteCount: movie.vote_count,
    popularity: movie.popularity,
    backdropPath: movie.backdrop_path,
    language: movie.original_language,
    video: movie.video,
    runtime: movie.runtime,
    genreIds: movie.genre_ids,
  };
};