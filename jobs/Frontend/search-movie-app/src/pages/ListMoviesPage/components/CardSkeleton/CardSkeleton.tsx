import {
  SkeletonBottomInfo,
  SkeletonCard,
  SkeletonImage,
  SkeletonText,
  SkeletonTitle,
} from './CardSkeleton.styles';

export const CardSkeleton = () => (
  <SkeletonCard>
    <SkeletonImage />
    <SkeletonTitle />
    <SkeletonBottomInfo>
      <SkeletonText />
      <SkeletonText />
    </SkeletonBottomInfo>
  </SkeletonCard>
);
