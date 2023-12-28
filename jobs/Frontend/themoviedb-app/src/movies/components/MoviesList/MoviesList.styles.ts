import styled from 'styled-components';

const StyledMoviesList = styled.ul`
    list-style: none;
    display: grid;
    grid-template-columns: 1fr;
    gap: 24px;

    @media screen and (min-width: ${(props) => props.theme.breakpoints.md}) {
        grid-template-columns: repeat(2, 1fr);
    }

    @media screen and (min-width: ${(props) => props.theme.breakpoints.lg}) {
        grid-template-columns: repeat(3, 1fr);
    }
`;

export default StyledMoviesList;
