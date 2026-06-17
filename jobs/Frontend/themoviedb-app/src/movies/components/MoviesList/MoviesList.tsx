import { FC } from 'react';
import { Movie } from '../../types/MovieSearchTypes';
import MovieItem from '../MovieItem/MovieItem';
import StyledMoviesList from './MoviesList.styles';

interface Props {
    movies: Movie[];
}

const MoviesList: FC<Props> = ({ movies }) => {
    return (
        <StyledMoviesList>
            {movies.map((movie) => (
                <MovieItem key={movie.id} {...movie} />
            ))}
        </StyledMoviesList>
    );
};

export default MoviesList;
