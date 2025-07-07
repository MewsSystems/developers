import styled, { keyframes } from "styled-components";

export const BodyContainer = styled.div`
  max-width: 1200px;
  margin: 0 auto; /* centers the container */
  padding: 2rem 1rem;
`;

export const Heading = styled.h1`
  font-size: 2rem;
  text-align: center;
  color: #2c3e50;
  margin-bottom: 0.5rem;
`;

export const Subheading = styled.p`
  font-size: 1.125rem;
  text-align: center;
  color: #7f8c8d;
  margin-bottom: 1.5rem;
`;

export const SearchInput = styled.input`
  display: block;
  margin: 0 auto 2rem auto;
  padding: 0.5rem 1rem;
  width: 100%;
  max-width: 400px;
  border-radius: 8px;
  border: 1px solid black;
  font-size: 1rem;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
  background-color: white;

  &:focus {
    border-color: #8e44ad;
    box-shadow: 0 0 0 3px rgba(142, 68, 173, 0.2);
    outline: none;
  }
`;

export const MovieListWrapper = styled.div`
  margin-top: 1rem;
`;

// Pulse animation
export const pulse = keyframes`
  0% { background-color: #e0e0e0; }
  50% { background-color: #f0f0f0; }
  100% { background-color: #e0e0e0; }
`;

// Skeleton styles
export const SkeletonGrid = styled.div`
  height: 100vh;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 3rem;
  width: 100%;
`;

export const SkeletonCard = styled.div`
  width: 100%;
  aspect-ratio: 2 / 3;
  border-radius: 12px;
  animation: ${pulse} 1.5s infinite ease-in-out;
`;

export const Logo = styled.img`
  height: 40px;
  object-fit: contain;
`;
