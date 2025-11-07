export function CastSkeletonRow({ count = 8 }: { count?: number }) {
    const items = Array.from({ length: count });
    return (
        <div className="-mx-1 overflow-x-hidden">
            <div className="flex gap-3 px-1">
                {items.map((_, i) => (
                    <div
                        key={i}
                        className="w-40 sm:w-44 shrink-0 rounded-xl border border-white/10 bg-white/5 p-3"
                    >
                        <div className="animate-pulse rounded-lg bg-white/10 h-40 mb-3" />
                        <div className="animate-pulse h-4 w-28 rounded bg-white/10 mb-2" />
                        <div className="animate-pulse h-3 w-24 rounded bg-white/10" />
                    </div>
                ))}
            </div>
        </div>
    );
}
