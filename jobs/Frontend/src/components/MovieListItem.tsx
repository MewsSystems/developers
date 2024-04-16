import { MovieModel } from '../interfaces/movieModel.ts';
import {
    MovieCard,
    MovieCardBody,
    MovieCardImg,
    MovieCardImgPlaceholder, MovieCardImgPlaceholderIcon,
    MovieCardTitle,
    MoviesListLi
} from './MovieListItem.styled';

export function MovieListItem({id, posterUrl, title, voteAverage}: MovieModel.MovieItem) {
    return (
        <MoviesListLi key={id}>
            <MovieCard to={`/movie/${id}`}>
                {
                    posterUrl
                        ? <MovieCardImg alt="" src={posterUrl}/>
                        : <MovieCardImgPlaceholder>
                            <MovieCardImgPlaceholderIcon/>
                        </MovieCardImgPlaceholder>
                }
                <MovieCardBody>
                    <MovieCardTitle>{title}</MovieCardTitle> <i>({voteAverage})</i>
                </MovieCardBody>
            </MovieCard>
        </MoviesListLi>
    );
}