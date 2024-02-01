import styled from 'styled-components';

const StyledMovieDetailsContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 24px;

    button {
        align-self: flex-start;
    }
`;

export default StyledMovieDetailsContainer;

export const StyledMovieDetailsContent = styled.div`
    img {
        width: 100%;
        margin-bottom: 10px;
    }

    @media screen and (min-width: ${(props) => props.theme.breakpoints.lg}) {
        display: flex;
        gap: 36px;

        img {
            width: 40%;
        }
    }
`;
