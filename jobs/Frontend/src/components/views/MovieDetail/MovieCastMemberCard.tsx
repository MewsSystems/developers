import { MovieModel } from '../../../interfaces/movieModel';
import { useId } from 'react';
import { CastMemberName, CastMemberPlaceholderIcon, MovieCastCard, MovieCastCardBody } from './MovieCastMemberCard.styled';
import { FallbackImg } from '../../shared/FallbackImg';
import { ProfileImageSize } from '../../../enums/images/profileImageSize';
import { IconBaseProps } from 'react-icons';

export function MovieCastMemberCard(castMember: MovieModel.CastMember) {
    const {
        name,
        character,
        getProfileImgUrl
    } = castMember;

    const nameId = useId();

    return (
        <MovieCastCard>
            <FallbackImg
                src={getProfileImgUrl(ProfileImageSize.Width185)}
                alt=""
                cssAspectRatio="2 / 3"
                imgProps={{'aria-labelledby': nameId}}
                placeholderIcon={(props: IconBaseProps) => <CastMemberPlaceholderIcon {...props}/>}
                placeholderLabel="Missing cast member picture"
            />
            <MovieCastCardBody>
                <CastMemberName id={nameId}>{name}</CastMemberName>
                <span>{character}</span>
            </MovieCastCardBody>
        </MovieCastCard>
    );
}