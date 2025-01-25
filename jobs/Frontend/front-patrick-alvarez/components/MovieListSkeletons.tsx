
import { memo } from 'react';

const SKELETON_COUNT = 9;

const SkeletonItem = memo(() => (
  <div className="animate-pulse">
    <div className="flex flex-col relative w-full h-48 rounded-lg bg-gray-700">
      <div className="absolute bottom-4 left-4">
        <div className="bg-gray-600 rounded-lg p-2">
          <div className="h-4 w-32 bg-gray-500 rounded"></div>
          <div className="h-3 w-16 bg-gray-500 rounded mt-1"></div>
        </div>
      </div>
    </div>
  </div>
));

// Precompute the array since it never changes
const skeletonArray = [...Array(SKELETON_COUNT)].map((_, index) => index);

export const MovieListSkeletons = () => {
  return (
    <section className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3 w-full">
      {skeletonArray.map((index) => (
        <SkeletonItem key={index} />
      ))}
    </section>
  );
};

SkeletonItem.displayName = 'SkeletonItem'