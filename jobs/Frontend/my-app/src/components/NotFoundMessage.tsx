import styled from 'styled-components';

const StyledNotFoundMessage = styled.div`
  font-size: clamp(2rem, 0.6667rem + 6.6667vw, 6rem);
`;

export const NotFoundMessage = () => {
  return <StyledNotFoundMessage>404 Not Found</StyledNotFoundMessage>;
};
