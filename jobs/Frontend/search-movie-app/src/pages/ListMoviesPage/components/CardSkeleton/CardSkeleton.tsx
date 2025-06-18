import { CARD_LIST_MOVIES_SKELETON_TEST_ID } from '../../../../constants';
import {
  SkeletonBottomInfo,
  SkeletonCard,
  SkeletonImage,
  SkeletonText,
  SkeletonTitle,
} from './CardSkeleton.styles';

export const CardSkeleton = () => (
  <SkeletonCard data-testid={CARD_LIST_MOVIES_SKELETON_TEST_ID}>
    <SkeletonImage />
    <SkeletonTitle />
    <SkeletonBottomInfo>
      <SkeletonText />
      <SkeletonText />
    </SkeletonBottomInfo>
  </SkeletonCard>
);
