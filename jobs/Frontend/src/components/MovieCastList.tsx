import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../enums/reactQueryPrimaryKey.ts';
import { getMovieCredits } from '../api/tmdbApi.ts';
import { ProfileImageSize } from '../enums/images/profileImageSize.ts';
import {
    CastMemberName,
    CastMemberPlaceholderIcon,
    MovieCastCard,
    MovieCastCardBody,
    MovieCastRow
} from './MovieCastList.styled.tsx';
import { IconBaseProps } from 'react-icons';
import { FallbackImg } from './shared/FallbackImg.tsx';
import { MovieModel } from '../interfaces/movieModel.ts';
import { VisuallyHidden } from './shared/A11y.tsx';

export function MovieCastList({ movieId }: { movieId: number }) {
    const enabled = !!movieId && !isNaN(movieId);
    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbMovieCredits, movieId],
        queryFn: () => getMovieCredits(Number(movieId)),
        enabled
    });

    if (!enabled) {
        return (<span>Movie not found</span>);
    }

    if (isPending) {
        return <MovieCastListSkeletonLoader />;
    }

    if (isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
    }

    const {
        cast
    } = data;

    // TODO: remove
    console.log({cast});

    if (!cast || cast.length === 0) {
        return null;
    }

    const castCards = cast?.map((castMember) =>
        <MovieCastMemberCard key={castMember.id} {...castMember}/>);

    return (
        <MovieCastRow>
            {castCards}
        </MovieCastRow>
    );
}

function MovieCastListSkeletonLoader() {
    const skeletonCards = Array.from({ length: 6 }, (_, i) => {
        return <MovieCastCard key={`${i}-placeholder`} />;
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

function MovieCastMemberCard(castMember: MovieModel.CastMember) {
    const {
        name,
        character,
        getProfileImgUrl
    } = castMember;

    return (
        <MovieCastCard>
            <FallbackImg
                src={getProfileImgUrl(ProfileImageSize.Width185)}
                alt=""
                cssAspectRatio='2 / 3'
                placeholderIcon={(props: IconBaseProps) => <CastMemberPlaceholderIcon {...props}/>}
                placeholderLabel='Missing cast member picture'
            />
            <MovieCastCardBody>
                <CastMemberName>{name}</CastMemberName>
                <span>{character}</span>
            </MovieCastCardBody>
        </MovieCastCard>
    );
}