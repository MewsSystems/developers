import styled, { css } from 'styled-components';
import { Spacer } from '../../enums/style/spacer';
import { BorderRadius } from '../../enums/style/borderRadius';
import { Gradient } from '../../enums/style/gradient';

const contentWidth = '60rem';

const primarySectionStyles = css`
    margin: 0 auto;
    max-width: ${contentWidth};
    width: 100%;
    padding: 0 ${Spacer.Lg};
`;

export const Header = styled.header`
    ${primarySectionStyles};
    background: ${Gradient.AccentsLinearTitled};

    @media (min-width: ${contentWidth}) {
        border-bottom-left-radius: ${BorderRadius.Md};
        border-bottom-right-radius: ${BorderRadius.Md};
    }
`;

export const Main = styled.main`
    ${primarySectionStyles};
    flex-grow: 1;
    padding-top: ${Spacer.Md};
`;

export const Footer = styled.footer`
    ${primarySectionStyles};
    padding: ${Spacer.Lg} ${Spacer.Lg} ${Spacer.Md} ${Spacer.Lg};
`;

export const TmdbAttributionText = styled.span`
    opacity: .9;
`;

export const TmdbLogoImg = styled.img`
    height: 1rem;
    margin-top: ${Spacer.Sm};
`;