function MovieListItemSkeleton() {
  return <div className="bg-white rounded-xl h-[255px] border border-cyan-100 animate-pulse"></div>;
}

interface Props {
  itemNumber: number;
}

export function MovieListSkeleton({ itemNumber }: Props) {
  return Array.from({ length: itemNumber }).map((_, index) => (
    <MovieListItemSkeleton key={index} />
  ));
}
