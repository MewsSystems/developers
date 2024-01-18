import { Get3SearchMovieApiResponse } from '@/store';
import { MovieCard } from './MovieCard';
import { MovieTitle } from './MovieTitle';
import { MoviePoster } from './MoviePoster';
import { MovieRating } from './MovieRating';

export type MoviePropsType = Pick<Exclude<Get3SearchMovieApiResponse['results'], undefined>[number], 'title' | 'poster_path' | 'id' | 'vote_average'>

const MovieListItem = ({title, poster_path, id, vote_average}: MoviePropsType) => (
  <MovieCard width="200px" position="relative" aspectRatio="9/13.5" flexDirection="column" as="a" href={`/movie/${id}`} title={`Movie: ${title}`}>
    <MoviePoster posterUrl={poster_path} />
    <MovieRating position="absolute" top="0" right="0" width="2.2em" aspectRatio="1/1" p="4px">
      {Math.round((vote_average ?? 0) * 10) / 10}
    </MovieRating>
    <MovieTitle>{title}</MovieTitle>
  </MovieCard>
)

export default MovieListItem
