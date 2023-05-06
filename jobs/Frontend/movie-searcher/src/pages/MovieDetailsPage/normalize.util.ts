import { formatCurrency, formatNumber } from "../../helpers/numberFormatters.util";
import type { RawMovieDetailsType } from "../../store/movieDetails/types";

export const normalizeMovieDetails = (movie: RawMovieDetailsType | null) => ({
  id: movie?.id || "",
  title: movie?.title || "",
  overview: movie?.overview || "",
  imgUrl: movie?.poster_path || "",
  releaseDate: movie?.release_date ? new Date(movie.release_date).getFullYear().toString() : "",
  budget: movie?.budget ? formatCurrency(movie.budget) : "",
  originalTitle: movie?.original_title || "",
  rating: movie?.vote_average || 0,
  voteCount: movie?.vote_count ? formatNumber(movie?.vote_count) : "",
});
