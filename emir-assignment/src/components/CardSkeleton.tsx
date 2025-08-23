type CardSkeletonProps = {
    count?: number;
};

export default function CardSkeleton({ count = 12 }: CardSkeletonProps) {
    const items = Array.from({ length: count });
    return (
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
            {items.map((_, i) => (
                <div key={i} className="animate-pulse">
                    <div className="aspect-[2/3] w-full rounded-lg bg-white/10" />
                    <div className="mt-3 h-4 w-3/4 rounded bg-white/10" />
                    <div className="mt-2 h-3 w-1/3 rounded bg-white/10" />
                </div>
            ))}
        </div>
    );
}
