import styled from "styled-components";
import { Link } from "react-router";
import { fontSizes } from "../../tokens/fontSizes";
import { colors } from "../../tokens/colors";

export const Wrapper = styled.section`
    margin: 0 auto;
    font-family: Roboto, sans-serif;
`

export const Title = styled.h1`
    font-size: ${fontSizes.lg};
    margin: 0 0 10px 0;
    text-align: left;

    @media screen and (min-width: 600px) {
        text-align: center;
        font-size: ${fontSizes.xlg}
    }
`

export const Tagline = styled.p`
    font-size: ${fontSizes.sm};
    color: ${colors.tagline};
    margin: 0 0 25px 0;
    text-align: left;

    @media screen and (min-width: 600px) {
        text-align: center;
    }
`

export const InfoPrimary = styled.section`
    display: block;
    gap: 20px;
    margin-bottom: 30px;
    justify-content: center;

    @media screen and (min-width: 600px) {
        display: flex;
        gap: 20px;
        margin-bottom: 30px;
        justify-content: center;
    }
`

export const PosterWrapper = styled.div`
`

export const PosterImage = styled.img`
    width: 100%;
    object-fit: cover;
`

export const MovieDetail = styled.div`
    display: flex;
    flex-direction: column;
    line-height: 1.5;
    gap: 10px;
    margin: 20px 0;

    @media screen and (min-width: 600px) {
        display: flex;
        flex-direction: column;
        justify-content: center;
        line-height: 1.5;
        gap: 10px;
    }
`

export const MovieDetailItem = styled.p`
    padding: 0;
    margin: 0;
    white-space: nowrap;  
`

export const MovieLink = styled.a`
    color: ${colors.btnPrimary};
    text-decoration: none;
    font-weight: bold;

    &:hover {
        text-decoration: underline;
        text-underline-offset: 4px;
    }
`
export const MovieOverview = styled.section`
`

export const MovieOverviewHeader = styled.h2`
    font-size: ${fontSizes.md};
    margin-bottom: 20px;
    text-align: left;

    @media screen and (min-width: 600px) {
        font-size: ${fontSizes.lg};
        text-align: center;
    }
`

export const MovieOverviewContent = styled.p`
    line-height: 1.6;
    text-align: left;
    margin-bottom: 30px;

    @media screen and (min-width: 600px) {
        text-align: center;
        width: 500px;
        margin: 0 auto 30px auto;
    }
`

export const ButtonLink = styled(Link)`
    text-decoration: none;
    display: inline-block;

    margin: 0;

    @media screen and (min-width: 600px) {
        display: block;
        max-width: fit-content;
        margin: 0 auto;
    }

    &:focus-visible {
        outline: 2px solid white;
        outline-offset: 4px;
    }
`