import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../enums/reactQueryPrimaryKey.ts';
import { getMovieDetail } from '../api/tmdbApi.ts';
import { Link, useParams } from 'react-router-dom';
import { ClosingBackdropImage } from './MovieDetail.styled.tsx';

export function MovieDetail() {
    const { movieId } = useParams();
    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbMovieDetail, movieId],
        queryFn: () => getMovieDetail(Number(movieId))
    });

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
        backdropUrl,
        genres,
        tagline
    } = data;

    console.log(data);

    // TODO
    /*
    * show
    * - vote avg & vote count; use icons
    * - poster, to the left of the image
    * - budget, revenue
    * - link to TMDB & IMBD
    * ----
    * - add option to move to the next search result?
    * - add support for country flags for country of origin?
    * */

    return (
        <>
            <Link to={-1}>Back</Link>
            <h2>{title} <i>({releaseDate.getFullYear()})</i></h2>
            <blockquote>{tagline}</blockquote>
            <span>{ genres.map(g => g.name).join(', ') }</span>
            <p>{overview}</p>
            <ClosingBackdropImage alt='' src={backdropUrl} />
        </>
    );
}