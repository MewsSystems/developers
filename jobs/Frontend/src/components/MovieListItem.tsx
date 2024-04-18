import { MovieModel } from '../interfaces/movieModel.ts';
import {
    MovieCard,
    MovieCardBody,
    MovieCardImg,
    MovieCardImgPlaceholder,
    MovieCardImgPlaceholderIcon,
    MovieCardTitle,
    MoviesListLi
} from './MovieListItem.styled';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';
import { useId } from 'react';

export function MovieListItem({id, getPosterUrl, title, voteAverage, voteCount}: MovieModel.MovieItem) {
    const posterUrl = getPosterUrl(PosterImageSize.Medium);
    const cardTitleId = useId();
    const displayRatingPercent = `${Math.round(voteAverage * 10)}%`;
    const displayRatingCount = new Intl.NumberFormat('en-US').format(voteCount);

    return (
        <MoviesListLi>
            <MovieCard to={`/movie/${id}`}>
                {
                    posterUrl
                        ? <MovieCardImg alt="" aria-labelledby={cardTitleId} src={posterUrl}/>
                        : <MovieCardImgPlaceholder aria-label='Missing movie picture' role='img'>
                            <MovieCardImgPlaceholderIcon aria-hidden={true}/>
                        </MovieCardImgPlaceholder>
                }
                <MovieCardBody>
                    <MovieCardTitle id={cardTitleId}>{title}</MovieCardTitle>
                    <i>{displayRatingPercent} from {displayRatingCount} ratings</i>
                </MovieCardBody>
            </MovieCard>
        </MoviesListLi>
    );
}