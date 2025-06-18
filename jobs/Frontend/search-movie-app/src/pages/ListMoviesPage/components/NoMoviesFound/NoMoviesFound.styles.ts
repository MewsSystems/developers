import styled from 'styled-components';

const StyledNoMoviesFoundtitle = styled.p`
  font-size: 2.5rem;
  color: ${({ theme }) => theme.colors.onSurface};
  margin: 10px;
`;

const StyledNoMoviesFoundSubtitle = styled.p`
  font-size: 1.2rem;
  color: ${({ theme }) => theme.colors.onSurface};
`;

const StyledNoMoviesFoundTextcontainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
`;

const StyledNoMoviesFoundImage = styled.img`
  max-width: 400px;
  height: auto;
`;
export {
  StyledNoMoviesFoundSubtitle,
  StyledNoMoviesFoundTextcontainer,
  StyledNoMoviesFoundtitle,
  StyledNoMoviesFoundImage,
};
