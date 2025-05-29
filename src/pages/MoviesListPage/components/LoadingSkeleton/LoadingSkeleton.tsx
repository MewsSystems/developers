import {useEffect, useState} from 'react';
import {getGridItemsCount} from './utils/getGridItemsCount.ts';
import {
  MovieCard,
  MovieCoverSkeleton,
  MovieDescription,
  MovieTitle,
  SkeletonGrid,
} from './LoadingSkeleton.styled.tsx';

export const LoadingSkeleton = () => {
  const [gridItemsCount, setGridItemsItemCount] = useState(getGridItemsCount());

  useEffect(() => {
    function handleResize() {
      setGridItemsItemCount(getGridItemsCount());
    }

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  return (
    <SkeletonGrid>
      {Array.from({length: gridItemsCount}, (_, index) => (
        <MovieCard key={index}>
          <MovieCoverSkeleton />
          <MovieTitle />
          <MovieDescription />
        </MovieCard>
      ))}
    </SkeletonGrid>
  );
};

LoadingSkeleton.displayName = 'LoadingSkeleton';
