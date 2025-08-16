import styled from 'styled-components';

const StyledMovieSearchHeader = styled.header`
    padding: 24px 0px;
    display: flex;
    /* align-items: center;
    justify-content: space-between; */
    flex-direction: column;
    gap: 10px;
    position: relative;
    margin-bottom: 24px;
`;

export default StyledMovieSearchHeader;

export const StyledSearchContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 10px;

    & > div:nth-child(2) {
        align-self: flex-end;
    }

    @media screen and (min-width: ${(props) => props.theme.breakpoints.md}) {
        flex-direction: row;
        align-items: center;
        justify-content: space-between;

        & > div:nth-child(1) {
            width: 60%;
        }

        & > div:nth-child(2) {
            align-self: auto;
            margin-left: auto;
        }
    }

    @media screen and (min-width: ${(props) => props.theme.breakpoints.lg}) {
        & > div:nth-child(1) {
            width: 50%;
        }
    }
`;
