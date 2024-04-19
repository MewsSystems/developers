import styled from 'styled-components';
import { Spacer } from '../../enums/style/spacer.ts';
import { BorderRadius } from '../../enums/style/borderRadius.ts';

export const ClosingBackdropImage = styled.img`
    mask-image: linear-gradient(transparent, rgb(0 0 0) 45%);
    aspect-ratio: 16 / 9;
    width: 100%;
    border-bottom-left-radius: 1rem;
    border-bottom-right-radius: 1rem;
`;

export const MoviePosterImage = styled.img`
    aspect-ratio: 2 / 3;
    width: 15rem;
    margin: 0 ${Spacer.Md} ${Spacer.Md} 0;
    border-radius: ${BorderRadius.Md};
`;

export const MovieDetailIntro = styled.div`
    display: flex;
    flex-direction: row;
`;

export const MovieDetailIntroBody = styled.div`
    display: flex;
    flex-direction: column;
`;