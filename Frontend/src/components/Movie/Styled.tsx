import styled from 'styled-components';
import { Link } from 'react-router-dom';

export const LoadingWrapper = styled.div`
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 3rem;
`;

export const BackButton = styled(Link)`
  border: 1px solid #fff;
  color: #fff;
  padding: 0 1rem;
  text-decoration: none;
  font-size: 1.25rem;
  margin: 1rem;
`;

export const MovieTitle = styled.h1`
  margin: 0 0 1rem;
`;

export const MovieOriginalTitle = styled.p`
  margin: -1rem 0 1rem;
`;

export const MoviePageWrapper = styled.div`
  display: flex;
  margin: 1rem;

  @media only screen and (max-width: 768px) {
    display: block;
  }
`;

export const MoviePoster = styled.div`
  flex: 0 0 25%;
  margin: 0 1rem 0 0;
`;

export const MovieTagline = styled.p`
  font-size: 2rem;
  margin: 0.5rem 0 1rem;
`;

export const MovieGenres = styled.div`
  display: flex;
  flex-wrap: wrap;
`;

export const MovieGenre = styled.span`
  border: 1px solid #ccc;
  color: #ccc;
  margin: 0 0.5rem 0.5rem 0;
  padding: 0.5rem;
`;

export const MovieVote = styled.div`
  font-size: 1.5rem;
`;

const getColor = (vote: number) => {
  if (vote <= 5) {
    return '#ff4d4d';
  } else if (vote <= 7) {
    return '#ffff00';
  }

  return '#4cff4c';
};

export const MovieVoteNum = styled.span`
  color: ${(props: { vote: number }) => getColor(props.vote)};
  font-weight: bold;
`;
