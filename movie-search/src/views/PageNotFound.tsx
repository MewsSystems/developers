import styled from "styled-components";
import {Link} from "react-router-dom";

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
    background-color: ${({ theme }) => theme.colors.background};
    text-align: center;
    color: ${({ theme }) => theme.colors.text};
`;

const Heading = styled.h1`
    font-size: 6rem;
    margin: 0;
    color: ${({ theme }) => theme.colors.error};
`;

const SubHeading = styled.h2`
    font-size: ${({ theme }) => theme.fontSizes.xl};
    margin: ${({ theme }) => theme.spacing.xs} 0;
`;

const StyledLink = styled(Link)`
    display: inline-block;
    margin-top: ${({ theme }) => theme.spacing.lg};
    padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.lg};

    font-size: ${({ theme }) => theme.fontSizes.lg};
    color: ${({ theme }) => theme.colors.background};
    background-color: ${({ theme }) => theme.colors.primary};
    text-decoration: none;
    border-radius: ${({ theme }) => theme.radii.sm};
    transition: background-color 0.3s ease;

    &:hover {
        background-color: #000000d4;
    }

    &:focus {
        outline: none;
        box-shadow: 0 0 0 3px ${({ theme }) => theme.colors.focus};
    }
`;
