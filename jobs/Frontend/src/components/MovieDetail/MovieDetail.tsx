import Flex from '@/components/Flex';
import { Get3MovieByMovieIdApiResponse } from '@/store';
import { MovieBackdrop } from '@/components/MovieDetail/MovieBackdrop';
import { MovieHeading } from '@/components/MovieDetail/MovieHeading';

const MovieDetail = (movie: Get3MovieByMovieIdApiResponse) => {

  return (
    <Flex flexDirection="column" gridGap="8px">
      <MovieBackdrop backdropPath={movie.backdrop_path ?? ''}>
        <MovieHeading>
          {movie.title}
        </MovieHeading>
      </MovieBackdrop>
      <Flex gridGap="16px">
          <span>
            {movie.release_date}
          </span>
        <span>
            {movie.genres?.map((genre) => genre.name).join(', ')}
          </span>
      </Flex>
      <Flex as="p">
        {movie.overview}
      </Flex>
    </Flex>
  )
}

export default MovieDetail;
