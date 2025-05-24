import { SkeletonWrapper, CardsGrid, CardSkeleton, SkeletonImage, CardContent, SkeletonTitle, CardFooter, SkeletonText } from "./skeleton-cards-list.styled";

interface MovieCardsSkeletonProps {
  numberOfCards?: number;
}

export const MovieCardsSkeleton = ({ numberOfCards = 4 }: MovieCardsSkeletonProps) => {
  return (
    <SkeletonWrapper data-testid="movie-cards-skeleton">
      <CardsGrid>
        {Array.from({ length: numberOfCards }).map((_, index) => (
          <CardSkeleton key={index}>
            <SkeletonImage />
            <CardContent>
              <SkeletonTitle />
            </CardContent>
            <CardFooter>
              <SkeletonText />
            </CardFooter>
          </CardSkeleton>
        ))}
      </CardsGrid>
    </SkeletonWrapper>
  );
};
