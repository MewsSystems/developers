import styled from "styled-components";

export const MovieCardContainer = styled.div`
  display: block;
  width: 100%;
  height: 100%;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.3s ease-in-out;
  &:hover {
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
  }
`;

export const Poster = styled.img`
  width: 100%;
  height: auto;
  display: block;
`;

export const Title = styled.h2`
  font-size: 1.2em;
  font-weight: bold;
  margin: 10px 0;
  padding: 0 10px;
  color: #333;
`;

export const ReleaseDate = styled.p`
  font-size: 0.9em;
  color: #666;
  margin: 0 0 10px;
  padding: 0 10px;
`;

export const GridContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 20px;
  padding: 20px;
  align-items: stretch;

  @media (max-width: 768px) {
    grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  }
`;
