import styled from "styled-components";

export const StickyContainer = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem;
  background: white;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  z-index: 100;

  @media (max-width: 768px) {
    flex-direction: column;
    padding: 0.5rem;
  }
`;

export const ContentWrapper = styled.div`
  margin-top: 80px;
  
  @media (max-width: 768px) {
    margin-top: 60px;
  }
`;

export const MoviesContainer = styled.div`
  padding: 2rem;
`;

export const MoviesGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 2rem;
  place-items: center;
`;