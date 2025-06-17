import styled from 'styled-components';

const getVoteType = (props: { $type?: string }) => {
  if (props.$type === 'low') return '#D32F2F';
  if (props.$type === 'medium') return '#F57C00';
  return '#388E3C';
};

export const StyledCardMovieContainer = styled.div`
  position: relative;
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

export const StyledCardMovieContent = styled.div`
  background-size: cover;
  background-position: center;
  overflow: hidden;
  width: 250px;
  height: 360px;
  justify-content: flex-end;
  align-content: flex-start;
  border-radius: 8px 8px 0 0;
`;
export const StyledCardMovieImage = styled.img`
  width: 250px;
  height: 360px;
`;

export const StyledTitle = styled.h3`
  margin: 0;
  font-size: 1.2rem;
  background: ${({ theme }) => theme.colors.surface};
  color: ${({ theme }) => theme.colors.onSurface};
  padding: 4px 8px;
  max-width: 100%;
  border-radius: 0 0 8px 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
`;

export const StyledInfoBottom = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: relative;
  font-size: 0.9rem;
  padding: 10px;
`;

export const StyledVote = styled.div`
  display: flex;
  background: ${({ theme }) => theme.colors.surface};
  padding: 2px 6px;
  border-radius: 6px;
  backdrop-filter: blur(10px);
  align-items: center;
  color: ${({ theme }) => theme.colors.onSurface};
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  font-weight: 500;
`;

export const StyledVoteNumber = styled.a<{ $type: string }>`
  color: ${props => getVoteType(props)};
  margin-right: 5px;
`;

export const StyledReleaseDate = styled.div`
  display: flex;
  background: ${({ theme }) => theme.colors.primary};
  padding: 2px 6px;
  border-radius: 6px;
  backdrop-filter: blur(10px);
  color: ${({ theme }) => theme.colors.onPrimary};
  align-items: center;
  font-weight: 500;
`;

export const StyledAdultBadge = styled.div`
  display: flex;
  color: rgb(161, 72, 72);
  font-size: 2.5rem;
`;
