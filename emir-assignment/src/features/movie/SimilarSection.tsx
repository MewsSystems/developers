import { useEffect, useState } from "react";
import { fetchJson, type TmdbMovie, type TmdbPage } from "../../lib/tmdb";
import ErrorState from "../../components/ErrorState";
import EmptyState from "../../components/EmptyState";
import MovieCard from "../../components/MovieCard";

export default function SimilarSection({
    movieId,
    limit = 12,
    title = "More like this",
}: {
    movieId: number;
    limit?: number;
    title?: string;
}) {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);
    const [items, setItems] = useState<TmdbMovie[]>([]);

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<TmdbPage<TmdbMovie>>(
            `/movie/${movieId}/similar`,
            { page: 1 },
            { signal: controller.signal }
        )
            .then((data) => {
                setItems((data.results ?? []).slice(0, limit));
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
                <div className="-mx-1 overflow-x-hidden">
                    <div className="flex gap-4 px-1">
                        {Array.from({ length: 6 }).map((_, i) => (
                            <div key={i} className="w-40 sm:w-48 shrink-0">
                                <div className="h-56 rounded-lg border border-white/10 bg-white/10 animate-pulse" />
                                <div className="mt-2 h-3 w-28 rounded bg-white/10 animate-pulse" />
                            </div>
                        ))}
                    </div>
                </div>
            ) : error ? (
                <ErrorState
                    title="Failed to load similar titles"
                    message={error.message}
                />
            ) : items.length === 0 ? (
                <EmptyState
                    title="No similar titles"
                    description="We couldn’t find related movies."
                />
            ) : (
                <div className="-mx-1 overflow-x-auto pb-6">
                    <div className="flex snap-x snap-mandatory gap-4 px-1">
                        {items.map((m) => (
                            <div
                                key={m.id}
                                className="snap-start shrink-0 w-40 sm:w-48"
                            >
                                <MovieCard
                                    id={m.id}
                                    title={m.title}
                                    release_date={m.release_date}
                                    vote_average={m.vote_average}
                                    poster_path={m.poster_path}
                                    backdrop_path={m.backdrop_path}
                                    overview={m.overview}
                                />
                            </div>
                        ))}
                    </div>
                </div>
            )}
        </section>
    );
}
