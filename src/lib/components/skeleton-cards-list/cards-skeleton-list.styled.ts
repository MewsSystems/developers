import styled, { keyframes } from 'styled-components';

const shimmer = keyframes`
  0% { background-position: -200px 0; }
  100% { background-position: 200px 0; }
`;

export const SkeletonWrapper = styled.div`
  width: 100%;
  padding: 2rem;
`;

export const CardsGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 2rem;
`;

export const CardSkeleton = styled.div`
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow: hidden;
`;

export const SkeletonImage = styled.div`
  width: 100%;
  height: 450px;
  background: linear-gradient(90deg, #e0e0e0 25%, #f5f5f5 50%, #e0e0e0 75%);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
`;

export const CardContent = styled.div`
  padding: 1rem;
`;

export const SkeletonTitle = styled.div`
  height: 24px;
  width: 80%;
  margin-bottom: 1rem;
  background: linear-gradient(90deg, #e0e0e0 25%, #f5f5f5 50%, #e0e0e0 75%);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
  border-radius: 4px;
`;

export const SkeletonText = styled.div`
  height: 16px;
  width: 100%;
  margin-bottom: 0.5rem;
  background: linear-gradient(90deg, #e0e0e0 25%, #f5f5f5 50%, #e0e0e0 75%);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
  border-radius: 4px;

  &:last-child {
    width: 60%;
  }
`;

export const CardFooter = styled.div`
  padding: 1rem;
  border-top: 1px solid #eee;
`;
