import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../../../enums/reactQueryPrimaryKey';
import { getMovieCredits } from '../../../api/tmdbApi';
import { MovieCastRow } from './MovieCastList.styled';
import { VisuallyHidden } from '../../shared/A11y';
import { NetworkErrorMessage } from '../../shared/Error';
import { MovieCastMemberCard } from './MovieCastMemberCard';
import { MovieCastCard } from './MovieCastMemberCard.styled';

export function MovieCastList({movieId}: { movieId: number }) {
    const enabled = !!movieId && !isNaN(movieId);
    const {data, isError, isPending} = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbMovieCredits, movieId],
        queryFn: () => getMovieCredits(Number(movieId)),
        enabled
    });

    if (!enabled) {
        return (<span>Movie not found</span>);
    }

    if (isPending) {
        return <MovieCastListSkeletonLoader/>;
    }

    if (isError) {
        return <NetworkErrorMessage/>;
    }

    const {
        cast
    } = data;

    if (cast.length === 0) {
        return null;
    }

    const castCards = cast.map((castMember) =>
        <MovieCastMemberCard key={castMember.id} {...castMember}/>);

    return (
        <MovieCastRow aria-label="Movies cast">
            {castCards}
        </MovieCastRow>
    );
}

function MovieCastListSkeletonLoader() {
    const skeletonCards = Array.from({length: 6}, (_, i) => {
        return <MovieCastCard key={`${i}-placeholder`}/>;
    });

    return (
        <>
            <VisuallyHidden>Loading cast members</VisuallyHidden>
            <MovieCastRow aria-hidden>
                {skeletonCards}
            </MovieCastRow>
        </>
    );
}

