import { MovieModel } from '../interfaces/movieModel.ts';
import {
    MovieCard,
    MovieCardBody,
    MovieCardImgPlaceholderIcon,
    MovieCardTitle,
    MoviesListLi
} from './MovieListItem.styled';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';
import { useId } from 'react';
import { FallbackImg } from './shared/FallbackImg.tsx';
import { IconBaseProps } from 'react-icons';
import { displayRatings } from '../utils/movieUtils.ts';

export function MovieListItem({id, getPosterUrl, title, voteAverage, voteCount}: MovieModel.MovieItem) {
    const posterUrl = getPosterUrl(PosterImageSize.Width500);
    const cardTitleId = useId();

    return (
        <MoviesListLi>
            <MovieCard to={`/movie/${id}`}>
                <FallbackImg
                    src={posterUrl}
                    alt=""
                    imgProps={{ 'aria-labelledby': cardTitleId }}
                    cssAspectRatio='2 / 3'
                    placeholderIcon={(props: IconBaseProps) => <MovieCardImgPlaceholderIcon {...props}/>}
                    placeholderLabel='Missing movie picture'
                />
                <MovieCardBody>
                    <MovieCardTitle id={cardTitleId}>{title}</MovieCardTitle>
                    <i>{displayRatings(voteAverage, voteCount)}</i>
                </MovieCardBody>
            </MovieCard>
        </MoviesListLi>
    );
}