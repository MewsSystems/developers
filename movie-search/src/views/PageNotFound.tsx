import styled from "styled-components";
import {Link} from "react-router-dom";
import {colors, fontSizes, radii, spacing} from "../styles/designTokens.ts";

export const PageNotFound = () => {
    return (
        <main role="main" aria-labelledby="not-found-title">
            <NotFoundWrapper>
                <Heading id="not-found-title" role="heading">404</Heading>
                <SubHeading>Page Not Found</SubHeading>
                <StyledLink to="/" role="link" aria-label="Go back to the homepage">Go Back Home</StyledLink>
            </NotFoundWrapper>
        </main>
    );
};

const NotFoundWrapper = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;

    height: 100vh;
    background-color: ${colors.background};
    text-align: center;
    color: ${colors.text};
`;

const Heading = styled.h1`
    font-size: 6rem;
    margin: 0;
    color: ${colors.error};
`;

const SubHeading = styled.h2`
    font-size: ${fontSizes.xl};
    margin: ${spacing.xs} 0;
`;

const StyledLink = styled(Link)`
    display: inline-block;
    margin-top: ${spacing.lg};
    padding: ${spacing.sm} ${spacing.lg};

    font-size: ${fontSizes.lg};
    color: ${colors.background};
    background-color: ${colors.primary};
    text-decoration: none;
    border-radius: ${radii.sm};
    transition: background-color 0.3s ease;

    &:hover {
        background-color: #000000d4;
    }

    &:focus {
        outline: none;
        box-shadow: 0 0 0 3px ${colors.focus};
    }
`;
