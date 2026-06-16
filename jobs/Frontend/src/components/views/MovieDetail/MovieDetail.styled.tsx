import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer';
import { BorderRadius } from '../../../enums/style/borderRadius';
import { Breakpoint } from '../../../enums/style/breakpoint';
import { Gradient } from '../../../enums/style/gradient';

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

export const ClosingBackdropImage = styled.img`
    mask-image: ${Gradient.FadeTop};
    aspect-ratio: 16 / 9;
    width: 100%;
    border-bottom-left-radius: ${BorderRadius.Md};
    border-bottom-right-radius: ${BorderRadius.Md};
`;