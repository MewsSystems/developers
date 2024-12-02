import { FC } from 'react';
import { MovieDetailsResponse } from '../../types/MovieDetailsTypes';
import StyledMovieDetailsInfo, {
    StyledMovieDetailsGenre,
    StyledMovieDetailsInformation,
    StyledMovieDetailsOverview,
} from './MovieDetailsInfo.styles';

const MovieDetailsInfo: FC<MovieDetailsResponse> = ({
    title,
    overview,
    genres,
    homepage,
    vote_average,
    vote_count,
    release_date,
}) => {
    return (
        <StyledMovieDetailsInfo>
            <StyledMovieDetailsGenre>
                {genres.map((genre) => (
                    <span key={genre.id}>{genre.name}</span>
                ))}
            </StyledMovieDetailsGenre>
            <h2>{title}</h2>
            <StyledMovieDetailsOverview>
                <h3>Overview</h3>
                <p>{overview}</p>
            </StyledMovieDetailsOverview>

            <StyledMovieDetailsInformation>
                <h3>Details:</h3>
                <ul>
                    <li>
                        <b>Homepage: </b>
                        <a href={homepage} target="_blank" rel="noreferrer">
                            {homepage}
                        </a>
                    </li>
                    <li>
                        <b>Rating: </b>
                        {vote_average} out of 10
                    </li>
                    <li>
                        <b>Vote counts: </b>
                        {vote_count}
                    </li>
                    <li>
                        <b>Release date: </b>
                        {release_date}
                    </li>
                </ul>
            </StyledMovieDetailsInformation>
        </StyledMovieDetailsInfo>
    );
};

export default MovieDetailsInfo;
