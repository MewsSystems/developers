export function MovieListItemSkeleton() {
  return (
    <div className="flex gap-4 bg-white rounded-xl p-3 h-[255px] border border-cyan-100 animate-pulse">
      <div className="w-[154px] h-[231px] bg-stone-100 rounded-md" />
    </div>
  );
}

interface Props {
  itemNumber: number;
}

export function MovieListSkeleton({ itemNumber }: Props) {
  return (
    <div className="space-y-4 mt-8">
      {Array.from({ length: itemNumber }).map((_, index) => (
        <MovieListItemSkeleton key={index} />
      ))}
    </div>
  );
}
