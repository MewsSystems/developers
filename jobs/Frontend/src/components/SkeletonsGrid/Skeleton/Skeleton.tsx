import {
  SkeletonContainer,
  SkeletonPoster,
  SkeletonSubInfo,
  SkeletonTitle,
} from "@/components/SkeletonsGrid/Skeleton/SkeletonStyle";

const Skeleton: React.FC = () => {
  return (
    <SkeletonContainer>
      <SkeletonPoster />
      <SkeletonTitle />
      <SkeletonSubInfo />
    </SkeletonContainer>
  );
};

export default Skeleton;
