import {
  BodySkeleton,
  CardSkeletonContainer,
  FooterSkeleton,
  TitleSkeleton,
} from './card-skeleton.styled';
import { ImageSkeleton } from './card-skeleton.styled';

export const CardSkeleton = () => {
  return (
    <CardSkeletonContainer>
      <ImageSkeleton />
      <BodySkeleton>
        <TitleSkeleton />
      </BodySkeleton>
      <FooterSkeleton />
    </CardSkeletonContainer>
  );
};
