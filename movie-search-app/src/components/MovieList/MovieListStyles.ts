import styled from "styled-components";

export const MovieListContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(
    auto-fill,
    minmax(200px, 1fr)
  ); /* 3 per row on large screens, responsive on smaller */
  gap: 1.5rem;
  padding: 2rem;
  justify-items: center;
`;

export const MovieCard = styled.div`
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  width: 100%;
  border: 1px solid black;
  max-width: 300px;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  text-align: center;
  cursor: pointer;

  &:hover {
    transform: translateY(-10px);
    box-shadow: 0 12px 24px rgba(0, 0, 0, 0.15);
  }

  h2 {
    font-size: 1rem;
    padding: 0.5rem;
    color: #2c3e50;
  }

  img {
    width: 100%;
    display: block;
    height: auto;
  }
`;
