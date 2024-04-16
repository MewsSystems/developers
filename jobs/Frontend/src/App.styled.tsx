import styled, { css } from 'styled-components';
import { Spacer } from './enums/style/spacer.ts';

const primarySectionStyles = css`
    margin: 0 auto;
    max-width: 60rem;
    width: 100%;
    padding: 0 ${Spacer.Lg};
`;

export const Header = styled.header`
    ${primarySectionStyles};
`;

export const Main = styled.main`
    ${primarySectionStyles};
    flex-grow: 1;
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