import { MagnifyingGlass } from '@styled-icons/heroicons-outline';
import styled from 'styled-components';

const StyledMovieSearchInput = styled.div`
    position: relative;
    width: 100%;
`;

export default StyledMovieSearchInput;

export const StyledSearchIcon = styled(MagnifyingGlass)`
    width: 20px;
    height: 20px;
    position: absolute;
    top: 50%;
    right: 6px;
    transform: translateY(-50%);
`;
