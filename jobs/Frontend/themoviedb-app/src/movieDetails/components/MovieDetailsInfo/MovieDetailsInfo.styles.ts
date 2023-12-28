import styled from 'styled-components';

const StyledMovieDetailsInfo = styled.div`
    display: flex;
    flex-direction: column;
    gap: 16px;
`;

export default StyledMovieDetailsInfo;

export const StyledMovieDetailsOverview = styled.div`
    display: flex;
    flex-direction: column;
    gap: 6px;
`;

export const StyledMovieDetailsGenre = styled.div`
    display: flex;
    flex-wrap: wrap;
    gap: 10px;

    span {
        font-size: 12px;
        font-weight: bold;
        background-color: ${(props) => props.theme.colors.primary};
        color: ${(props) => props.theme.colors.btnTextLight};
        padding: 6px 16px;
        border-radius: 24px;
        box-shadow: 2px 4px 10px rgba(0, 0, 0, 0.1);
    }
`;

export const StyledMovieDetailsInformation = styled.div`
    ul {
        list-style: none;
        display: flex;
        flex-direction: column;
        li {
            padding-left: 16px;
            padding-top: 6px;
        }
    }
`;
