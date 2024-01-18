import MovieLayout from '@/Layouts/Movie';
import { useRouter } from 'next/router';
import { useGet3MovieByMovieIdQuery } from '@/store';

const Movie = () => {
  const {query} = useRouter();
  const id = parseInt(query.id as string, 10);

  return (
    <MovieLayout id={id}/>
  )
}

export default Movie
