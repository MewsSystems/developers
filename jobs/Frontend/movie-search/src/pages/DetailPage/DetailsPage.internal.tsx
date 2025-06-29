import styled from "styled-components";

export const MovieDetailsWrapper = styled.div`
  width: 100%;
  height: auto;
  display: flex;
  justify-content: flex-start;
  /* flex-wrap: wrap; */
  gap: 2rem;
`;

export const ImageSection = styled.section`
  width: 20%;
  display: flex;
  align-items: center;
  justify-content: center;
`;

export const MovieDetailsTagline = styled.p`
  font-size: 1.2rem;
  color: #333;
  font-style: italic;
`;

export const MovieDetailsSection = styled.section`
  width: auto;
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
`;

export const MovieDetailsRow = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
`;

export const MovieDetailsTitle = styled.p`
  font-size: 1.2rem;
  font-weight: 800;
  color: #333;
`;

export const MovieGenresList = styled.ul`
  width: 100%;
  padding: 0;
  list-style: none;
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  flex-wrap: wrap;
  gap: 0.4rem;
`;

export const MovieGenresBadge = styled.li`
  padding: 0.2rem;
  border: 1px solid #333;

  color: #333;
  font-size: 0.8rem;
`;

export const MovieOverview = styled.p`
  color: #333;
  font-size: 0.8rem;
`;
