import { styled } from "styled-components";

export const DetailSection = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;
export const MovieDetailWrapper = styled.div`
  display: flex;
  flex-direction: row;
  gap: 2rem;
  align-items: center;
  margin-bottom: 1rem;

  @media (max-width: 767px) {
    flex-wrap: wrap;
  }
`;
export const MoviePoster = styled.img`
  max-width: 300px;
  border-radius: 4px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);

  @media (max-width: 767px) {
    max-width: 100%;
  }
`;

export const BackButton = styled.button`
  margin-top: 1rem;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border: 1px solid #888;
  border-radius: 4px;
  cursor: pointer;
  transition: 0.2s;
  background-color: #282828;
  color: #fff;

  &:hover {
    background-color: #3d3d3d;
  }
`;
