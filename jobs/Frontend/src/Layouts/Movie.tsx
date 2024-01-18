import Grid from '@/components/Grid';
import { Get3MovieByMovieIdApiResponse, useGet3MovieByMovieIdQuery } from '@/store';
import MovieDetail from '@/components/MovieDetail/MovieDetail';


const MovieLayout = ({id}: {id: number}) => {
  const movie = useGet3MovieByMovieIdQuery({ movieId: id});

  return (
    <Grid
      gridTemplateRows="auto auto auto"
      minHeight="100%"
    >
      <MovieDetail {...movie.data}/>
    </Grid>
  )
}

export default MovieLayout;
