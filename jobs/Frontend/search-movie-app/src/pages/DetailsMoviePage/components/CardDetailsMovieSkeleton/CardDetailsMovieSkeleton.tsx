import { CARD_DETAILS_MOVIE_SKELETON_TEST_ID } from '../../../../constants';
import {
  SkeletonCardDetailsMovie,
  SkeletonContentDetailsMovie,
  SkeletonBadgeItemDetailsMovie,
  SkeletonBadgeWrapperDetailsMovie,
  SkeletonImageDetailsMovie,
  SkeletonTaglineDetailsMovie,
  SkeletonTextDetailsMovie,
  SkeletonTitleDetailsMovie,
} from './CardDetailsMovieSkeleton.styles';

export const CardDetailsMovieSkeleton = () => {
  return (
    <SkeletonCardDetailsMovie data-testid={CARD_DETAILS_MOVIE_SKELETON_TEST_ID}>
      <div className="flex">
        <SkeletonImageDetailsMovie />
      </div>
      <SkeletonContentDetailsMovie>
        <SkeletonTitleDetailsMovie />
        <SkeletonTaglineDetailsMovie />
        <SkeletonBadgeWrapperDetailsMovie>
          <SkeletonBadgeItemDetailsMovie />
          <SkeletonBadgeItemDetailsMovie />
          <SkeletonBadgeItemDetailsMovie />
        </SkeletonBadgeWrapperDetailsMovie>
        <SkeletonTextDetailsMovie />
        <SkeletonBadgeWrapperDetailsMovie>
          <SkeletonBadgeItemDetailsMovie />
          <SkeletonBadgeItemDetailsMovie />
        </SkeletonBadgeWrapperDetailsMovie>
      </SkeletonContentDetailsMovie>
    </SkeletonCardDetailsMovie>
  );
};
