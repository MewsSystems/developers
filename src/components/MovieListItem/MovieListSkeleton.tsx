import { movieListItemContainerClasses } from './MovieListItem';
import { Card } from '@/components/Card';

function MovieListItemSkeleton() {
  return (
    <Card
      className={`${movieListItemContainerClasses} animate-pulse`}
      data-testid="movie-list-item-skeleton"
    ></Card>
  );
}

interface Props {
  itemNumber: number;
}

export function MovieListSkeleton({ itemNumber }: Props) {
  return Array.from({ length: itemNumber }).map((_, index) => (
    <MovieListItemSkeleton key={index} />
  ));
}
