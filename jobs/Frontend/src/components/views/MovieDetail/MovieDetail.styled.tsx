import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer.ts';
import { BorderRadius } from '../../../enums/style/borderRadius.ts';
import { Breakpoint } from '../../../enums/style/breakpoint.ts';

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
    margin: 0 0 ${Spacer.Md} 0;
    border-radius: ${BorderRadius.Md};

    @media (min-width: ${Breakpoint.Sm}) {
        margin-right: ${Spacer.Md};
    }
`;

export const MovieDetailIntro = styled.section`
    display: flex;
    flex-direction: column;
    align-items: center;

    @media (min-width: ${Breakpoint.Sm}) {
        align-items: flex-start;
        flex-direction: row;
    }
`;

export const MovieDetailIntroBody = styled.div`
    display: flex;
    flex-direction: column;
`;