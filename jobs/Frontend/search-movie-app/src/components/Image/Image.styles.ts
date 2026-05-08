import styled from 'styled-components';

export const StyledCardMovieImage = styled.img<{ $rounded?: boolean }>`
  width: 100%;
  height: 100%;
  border-radius: ${props => (props.$rounded ? '8px' : '0px')};
`;
