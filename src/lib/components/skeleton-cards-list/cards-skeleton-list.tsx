import styled from 'styled-components';
import { CardSkeleton } from '../card/card-skeleton';

const SkeletonContainer = styled.div`
  padding: 2rem;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 2rem;
  place-items: center;
`;

export const MovieCardsSkeleton = () => {
  return (
    <SkeletonContainer>
      {Array.from({ length: 6 }).map((_, index) => (
        <CardSkeleton key={index} />
      ))}
    </SkeletonContainer>
  );
};
