import styled from 'styled-components';

const StyledCardMovieContainer = styled.div`
  width: 250px;
  height: 380px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  color: ${({ theme }) => theme.colors.onPrimary};
  border-radius: 8px;
  display: flex;
  flex-direction: column;
  transition:
    transform 0.3s ease,
    box-shadow 0.3s ease;
  cursor: pointer;

  &:hover {
    transform: scale(1.03);
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.3);
  }

  &:active {
    transform: scale(0.98);
  }
`;

const StyledCardMovieContent = styled.div`
  overflow: hidden;
  width: 250px;
  height: 360px;
  justify-content: flex-end;
  align-content: flex-start;
  border-radius: 8px 8px 0 0;
  display: flex;
  position: relative;
`;

const StyledTitle = styled.h3`
  margin: 0;
  font-size: 1.2rem;
  background: ${({ theme }) => theme.colors.surface};
  color: ${({ theme }) => theme.colors.onSurface};
  padding: 4px 8px;
  max-width: 100%;
  border-radius: 0 0 8px 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
`;

const StyledInfoBottom = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: absolute;
  font-size: 0.9rem;
  width: 100%;
`;

export { StyledCardMovieContainer, StyledCardMovieContent, StyledInfoBottom, StyledTitle };
