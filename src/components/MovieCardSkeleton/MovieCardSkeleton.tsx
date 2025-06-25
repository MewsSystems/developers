import {
  SkeletonCard,
  SkeletonContent,
  SkeletonInfo,
  SkeletonOverview,
  SkeletonOverviewShort,
  SkeletonPoster,
  SkeletonTitle,
} from "./MovieCardSkeleton.styles"

export const MovieCardSkeleton = () => {
  return (
    <SkeletonCard>
      <SkeletonPoster />
      <SkeletonContent>
        <SkeletonTitle />
        <SkeletonOverview />
        <SkeletonOverview />
        <SkeletonOverviewShort />
        <SkeletonInfo />
      </SkeletonContent>
    </SkeletonCard>
  )
}
