import { FC } from 'react';
import { Movie } from '../models/MovieSearchModels';
import MovieItem from './MovieItem';

interface Props {
    movies: Movie[];
}

const MoviesList: FC<Props> = ({ movies }) => {
    return (
        <ul>
            {movies.map((movie) => (
                <MovieItem key={movie.id} {...movie} />
            ))}
        </ul>
    );
};

export default MoviesList;
