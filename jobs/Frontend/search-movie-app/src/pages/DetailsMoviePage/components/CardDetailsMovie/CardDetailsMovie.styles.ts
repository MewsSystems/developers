import styled from 'styled-components';

const StyledCardMovieDetailsWrapper = styled.div`
  display: flex;
  padding: 20px;
  margin: 20px;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  gap: 40px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  border-radius: 8px;
  background: ${({ theme }) => theme.colors.surface};

  @media (max-width: 991px) {
    flex-direction: column;
  }
`;

const StyledCardMovieDetailsimage = styled.img`
  height: 100%;
`;
const StyledCardMovieDetailsContent = styled.div`
  display: flex;
  flex-direction: column;
  flex: 2;
`;

const StyledCardMovieDetailsOverview = styled.p``;
const StyledCardMovieDetailsGenreWrapper = styled.div`
  display: flex;
  gap: 15px;
  padding: 10px 0px;
`;
const StyledCardMovieDetailsGenreItem = styled.div`
  background: ${({ theme }) => theme.colors.primary};
  color: ${({ theme }) => theme.colors.onPrimary};
  border-radius: 8px;
  padding: 4px 15px;
  height: 30px;
  min-width: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
`;
const StyledCardMovieDetailsTitle = styled.p`
  font-size: 3rem;
  font-weight: 500;
  margin: 0;
  color: ${({ theme }) => theme.colors.primary};
  @media (max-width: 991px) {
    font-size: 1.5rem;
  }
`;
const StyledCardMovieDetailsTagline = styled.p`
  font-size: 1.5rem;
  margin: 0;
  color: ${({ theme }) => theme.colors.onSurface};
  @media (max-width: 991px) {
    font-size: 1.2rem;
  }
`;

const StyledCardMovieImageWrapper = styled.div`
  @media (max-width: 991px) {
    width: 100%;
  }
`;

export {
  StyledCardMovieDetailsWrapper,
  StyledCardMovieDetailsGenreItem,
  StyledCardMovieDetailsOverview,
  StyledCardMovieDetailsTagline,
  StyledCardMovieDetailsTitle,
  StyledCardMovieDetailsimage,
  StyledCardMovieDetailsContent,
  StyledCardMovieDetailsGenreWrapper,
  StyledCardMovieImageWrapper,
};
