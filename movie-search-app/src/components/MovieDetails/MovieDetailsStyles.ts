import { Link } from "react-router-dom";
import styled, { keyframes } from "styled-components";

export const Container = styled.div`
  max-width: 800px;
  height: auto;
  min-height: 1000px;
  margin: 2rem auto;
  padding: 1.5rem;
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 0 12px rgba(0, 0, 0, 0.1);
  font-family: "Helvetica Neue", sans-serif;
  border: 2px solid black;
`;

export const BackLink = styled(Link)`
  display: inline-block;
  margin-bottom: 1rem;
  text-decoration: none;
  color: #3498db;
  font-weight: 500;

  &:hover {
    text-decoration: underline;
  }
`;

export const Title = styled.h1`
  font-size: 2rem;
  margin-bottom: 1rem;
  text-align: center;
`;

export const PosterWrapper = styled.div`
  display: inline-block;
  padding: 4px; /* thickness of the "border" */
  border-radius: 12px; /* match your poster corners */
  background: linear-gradient(
    135deg,
    #8e44ad,
    #3498db
  ); /* purple to blue gradient */

  /* Optional: For smooth corners inside */
  overflow: hidden;
`;

export const Poster = styled.img`
  display: block;
  width: 100%;
  max-width: 400px;
  border-radius: 8px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
`;

export const Info = styled.p`
  font-size: 1rem;
  line-height: 1.6;
  margin-bottom: 0.75rem;
`;

// Pulse animation
const pulse = keyframes`
  0% { background-color: #e0e0e0; }
  50% { background-color: #f0f0f0; }
  100% { background-color: #e0e0e0; }
`;

// Skeleton styles
export const SkeletonContainer = styled(Container)`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

export const SkeletonBlock = styled.div<{ width?: string; height?: string }>`
  width: ${({ width }) => width || "100%"};
  height: ${({ height }) => height || "1rem"};
  border-radius: 8px;
  animation: ${pulse} 1.5s infinite ease-in-out;
  margin-bottom: 1rem;
`;

export const SkeletonPoster = styled.div`
  width: 100%;
  max-width: 400px;
  height: 600px;
  border-radius: 8px;
  animation: ${pulse} 1.5s infinite ease-in-out;
  margin-bottom: 1rem;
`;
