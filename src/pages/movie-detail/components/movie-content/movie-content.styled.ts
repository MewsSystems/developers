import styled from 'styled-components';

export const Poster = styled.img`
  width: 100%;
  max-width: 300px;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
`;

export const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

export const Title = styled.h1`
  font-size: 2rem;
  font-weight: bold;
  color: #fff;
  margin: 0;
`;

export const Overview = styled.p`
  color: #fff;
  font-size: 1rem;
  line-height: 1.5;
  margin: 0;
`;

export const MovieDetailsContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

export const DetailItem = styled.span`
  width: fit-content;
  background-color: rgba(255, 255, 255, 0.1);
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.875rem;
  color: #fff;
`;

export const ReleaseDate = styled.p`
  width: fit-content;
  color: #fff;
  font-size: 1rem;
  margin: 0;
`;

export const Rating = styled.p`
  color: #fff;
  font-size: 1rem;
  margin: 0;
`; 