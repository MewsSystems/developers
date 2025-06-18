import {
  SkeletonBottomInfo,
  SkeletonCard,
  SkeletonImage,
  SkeletonText,
  SkeletonTitle,
} from './CardSkeleton.styles';

export const CardSkeleton = () => (
  <SkeletonCard data-testid="card-skeleton">
    <SkeletonImage />
    <SkeletonTitle />
    <SkeletonBottomInfo>
      <SkeletonText />
      <SkeletonText />
    </SkeletonBottomInfo>
  </SkeletonCard>
);
