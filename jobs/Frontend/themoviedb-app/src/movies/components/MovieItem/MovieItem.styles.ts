import { Photo } from '@styled-icons/heroicons-outline';
import styled from 'styled-components';
import Button from '../../../shared/components/Button/Button';

const StyledMovieItem = styled.li`
    padding: 16px;
    background-color: white;
    border-radius: 6px;
    box-shadow: 2px 4px 10px rgba(0, 0, 0, 0.1);
    display: flex;
    flex-direction: column;
    gap: 10px;
    justify-content: space-between;
    transition: all 200ms;

    &:hover {
        box-shadow: 2px 4px 10px rgba(0, 0, 0, 0.25);
        transform: scale(1.02);
    }

    @media screen and (min-width: ${(props) => props.theme.breakpoints.md}) {
        grid-template-columns: 1fr 1fr;
    }
`;

export default StyledMovieItem;

export const StyledPoster = styled.img`
    width: 100%;
`;

export const StyledFallbackPoster = styled.div`
    background-color: lightgray;
    width: 100%;
    min-height: 340px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    opacity: 0.3;
`;

export const StyledPhotoIcon = styled(Photo)`
    width: 24px;
    height: 24px;
`;

export const StyledMovieItemButton = styled(Button)`
    align-self: flex-end;
`;
