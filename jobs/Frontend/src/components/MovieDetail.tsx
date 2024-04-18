import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../enums/reactQueryPrimaryKey.ts';
import { getMovieDetail } from '../api/tmdbApi.ts';
import { Link, useParams } from 'react-router-dom';
import { ClosingBackdropImage } from './MovieDetail.styled.tsx';
import { BackdropImageSize } from '../enums/images/backdropImageSize.ts';
import { Button } from './shared/Button.tsx';
import { Spacer } from '../enums/style/spacer.ts';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';

export function MovieDetail() {
    const { movieId } = useParams();
    const enabled = !!movieId;
    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbMovieDetail, movieId],
        queryFn: () => getMovieDetail(Number(movieId)),
        enabled
    });

    if (!enabled) {
        return (<span>Movie not found</span>);
    }

    if (isPending) {
        return (<span>Loading</span>);
    }

    if (isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
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
        id
    } = data;

    // TODO: remove
    console.log(data);

    // TODO
    /*
    * show
    * - vote avg & vote count; use icons
    * - poster, to the left of the image
    * - runtime
    * - link to TMDB & IMBD
    * */

    const backdropUrl = getBackdropUrl(BackdropImageSize.Large);
    const posterUrl = getPosterUrl(PosterImageSize.Medium);

    // TODO: the year can be missing
    // TODO: runtime can be 0
    // TODO: there can be no ratings
    return (
        <>
            {posterUrl && <img alt="" width={200} src={posterUrl} style={{ float: 'left', margin: `0 ${Spacer.Md} ${Spacer.Md} 0` }}/>}
            <h2>{title} <i>({releaseDate.getFullYear()})</i></h2>
            <blockquote>{tagline}</blockquote>
            <span>{genres.map(g => g.name).join(', ')}</span><br/>
            <span>Duration: {runtime}m</span>
            <a href={`https://www.imdb.com/title/${imbdId}`}>IMBD</a>
            <a href={`https://www.themoviedb.org/movie/${id}`}>TMBD</a>
            <p>{overview}</p>
            {backdropUrl && <ClosingBackdropImage alt='' src={backdropUrl}/>}
            <Button
                as={Link}
                to={-1}
                style={{marginTop: Spacer.Lg}}
            >
                Back to search
            </Button>
        </>
    );
}