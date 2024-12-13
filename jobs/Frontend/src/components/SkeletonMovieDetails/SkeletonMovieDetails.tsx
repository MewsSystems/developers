import {
  SkeletonDetailsContainer,
  SkeletonHero,
  SkeletonContent,
  SkeletonLine,
} from "@/components/SkeletonMovieDetails/SkeletonMovieDetailsStyle";

const SkeletonMovieDetails: React.FC = () => {
  return (
    <SkeletonDetailsContainer>
      <SkeletonHero />
      <SkeletonContent>
        <SkeletonLine width="30%" height="20px" />{" "}
        <SkeletonLine width="60%" height="24px" />
        <SkeletonLine width="40%" height="12px" />{" "}
        <SkeletonLine width="90%" height="12px" />{" "}
        <SkeletonLine width="85%" height="12px" />
        <SkeletonLine width="80%" height="12px" />
        <SkeletonLine width="70%" height="12px" />
      </SkeletonContent>
    </SkeletonDetailsContainer>
  );
};

export default SkeletonMovieDetails;
