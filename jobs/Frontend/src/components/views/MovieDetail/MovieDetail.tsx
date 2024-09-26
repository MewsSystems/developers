import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../../../enums/reactQueryPrimaryKey';
import { getMovieDetail } from '../../../api/tmdbApi';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import {
    ClosingBackdropImage,
    MovieDetailIntro,
    MovieDetailIntroBody,
    MoviePosterImage
} from './MovieDetail.styled';
import { BackdropImageSize } from '../../../enums/images/backdropImageSize';
import { Button } from '../../shared/Button';
import { Spacer } from '../../../enums/style/spacer';
import { PosterImageSize } from '../../../enums/images/posterImageSize';
import { MovieCastList } from './MovieCastList';
import { BsClock } from 'react-icons/bs';
import { displayRatings, displayRuntime } from '../../../utils/movieUtils';
import { ReactNode } from 'react';
import { Loader } from '../../shared/Loader';
import { NetworkErrorMessage } from '../../shared/Error';

export function MovieDetail() {
    const {movieId} = useParams();
    const enabled = !!movieId;
    const {data, isError, isPending} = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbMovieDetail, movieId],
        queryFn: () => getMovieDetail(Number(movieId)),
        enabled
    });

    if (!enabled) {
        return (<h2>Movie not found</h2>);
    }

    if (isPending) {
        return (<Loader><h2>Loading movie detail</h2></Loader>);
    }

    if (isError) {
        return <NetworkErrorMessage/>;
    }

    const {
        title,
        overview,
        releaseDate,
        getBackdropUrl,
        getPosterUrl,
        genres,
        tagline,
        runtime,
        imbdId,
        id,
        voteAverage,
        voteCount
    } = data;

    const backdropUrl = getBackdropUrl(BackdropImageSize.Width1280);
    const posterUrl = getPosterUrl(PosterImageSize.Width500);

    const genresSummary = (
        genres.length > 0 &&
        <><span>{genres.map(g => g.name).join(', ')}</span><br/></>
    );

    return (
        <>
            <MovieDetailIntro>
                {posterUrl && <MoviePosterImage alt="" src={posterUrl}/>}
                <MovieDetailIntroBody>
                    <h2>
                        {title}
                        {releaseDate && <>{' '}<i>({releaseDate.getFullYear()})</i></>}
                    </h2>

                    {tagline && <blockquote>{tagline}</blockquote>}
                    {genresSummary}
                    {runtime > 0 && <><span><BsClock/> {displayRuntime(runtime)}</span><br/></>}
                    <i>{displayRatings(voteAverage, voteCount)}</i>

                    {overview && <p>{overview}</p>}

                    <p>
                        <Button as="a" href={`https://www.themoviedb.org/movie/${id}`} title="See on TMDB">TMBD</Button>
                        {imbdId && <Button
                            as="a"
                            href={`https://www.imdb.com/title/${imbdId}`}
                            title="See on IMDB">
                            IMBD
                        </Button>
                        }
                    </p>
                </MovieDetailIntroBody>
            </MovieDetailIntro>

            <MovieCastList movieId={Number(movieId)}/>

            {backdropUrl && <ClosingBackdropImage alt="" src={backdropUrl}/>}

            <BackButton>
                Back to search
            </BackButton>
        </>
    );
}

function BackButton({children}: { children: ReactNode }) {
    const navigate = useNavigate();
    const location = useLocation();

    const goBack = () => {
        // prevent leaving site; initial location is 'default', subsequent have an unique id
        if (location.key !== 'default') {
            navigate(-1);
        } else {
            navigate('/');
        }
    };

    return (
        <Button
            onClick={goBack}
            style={{marginTop: Spacer.Lg}}
        >
            {children}
        </Button>
    );
}
