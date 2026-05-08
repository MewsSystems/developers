import { useEffect, useState } from "react";
import { fetchJson, imgUrl } from "../../lib/tmdb";
import ErrorState from "../../components/ErrorState";
import EmptyState from "../../components/EmptyState";
import { CastSkeletonRow } from "./CastSkeletonRow";
import { UserRound } from "lucide-react";

type CastMember = {
    id: number;
    name: string;
    character: string;
    profile_path: string | null;
    order?: number;
};

export default function CastSection({
    movieId,
    limit = 16,
    title = "Top Billed Cast",
}: {
    movieId: number;
    limit?: number;
    title?: string;
}) {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);
    const [cast, setCast] = useState<CastMember[]>([]);

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<{ id: number; cast: CastMember[] }>(
            `/movie/${movieId}/credits`,
            undefined,
            { signal: controller.signal }
        )
            .then((data) => {
                const sorted = [...(data.cast ?? [])].sort((a, b) => {
                    const ao = a.order ?? 9999;
                    const bo = b.order ?? 9999;
                    return ao - bo;
                });
                setCast(sorted.slice(0, limit));
            })
            .catch((err: unknown) => {
                if (controller.signal.aborted) return;
                setError(
                    err instanceof Error ? err : new Error("Unknown error")
                );
            })
            .finally(() => {
                if (!controller.signal.aborted) setLoading(false);
            });

        return () => controller.abort();
    }, [movieId, limit]);

    return (
        <section className="space-y-3">
            <h2 className="text-lg font-semibold">{title}</h2>

            {loading ? (
                <CastSkeletonRow count={8} />
            ) : error ? (
                <ErrorState
                    title="Failed to load cast"
                    message={error.message}
                />
            ) : cast.length === 0 ? (
                <EmptyState
                    title="No cast found"
                    description="No credited cast available."
                />
            ) : (
                <div className="-mx-1 overflow-x-auto pb-4">
                    <div className="flex snap-x snap-mandatory gap-3 px-1">
                        {cast.map((c) => (
                            <div
                                key={c.id}
                                className="snap-start shrink-0 w-40 sm:w-44 rounded-xl border border-white/10 bg-white/5 p-3"
                            >
                                <div className="mb-3 overflow-hidden rounded-lg bg-white/5">
                                    {c.profile_path ? (
                                        <img
                                            src={imgUrl(c.profile_path, "w185")}
                                            alt={c.name}
                                            className="w-full h-[200px] object-cover"
                                            loading="lazy"
                                        />
                                    ) : (
                                        <div className="h-[200px] grid place-items-center py-12 text-white">
                                            <UserRound
                                                size={40}
                                                strokeWidth={1.5}
                                            />
                                            <span className="mt-2 text-xs text-white">
                                                No photo
                                            </span>
                                        </div>
                                    )}
                                </div>
                                <div className="space-y-1">
                                    <p className="line-clamp-2 text-sm font-semibold text-white">
                                        {c.name}
                                    </p>
                                    <p className="line-clamp-2 text-xs text-neutral-400">
                                        {c.character || "—"}
                                    </p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            )}
        </section>
    );
}
