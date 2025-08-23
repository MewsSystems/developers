import { useEffect, useState } from "react";
import { fetchJson, type TmdbMovie, type TmdbPage } from "../lib/tmdb";
import CardSkeleton from "./CardSkeleton";
import EmptyState from "./EmptyState";
import ErrorState from "./ErrorState";
import MovieCard from "./MovieCard";
import type { HttpError } from "../lib/errors";

type Props = {
    title: string;
    endpoint: string; // e.g., '/movie/top_rated'
    limit?: number; // how many to show (default 8)
};

export default function GridSection({ title, endpoint, limit = 12 }: Props) {
    const [items, setItems] = useState<TmdbMovie[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);

    async function load(signal: AbortSignal) {
        setLoading(true);
        setError(null);
        try {
            const data = await fetchJson<TmdbPage<TmdbMovie>>(
                endpoint,
                { page: 1 },
                { signal }
            );
            setItems((data.results ?? []).slice(0, limit));
        } catch (err: unknown) {
            if (!signal.aborted)
                setError(
                    err instanceof Error ? err : new Error("Unknown error")
                );
        } finally {
            if (!signal.aborted) setLoading(false);
        }
    }

    useEffect(() => {
        const controller = new AbortController();
        load(controller.signal);
        return () => controller.abort();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [endpoint, limit]);

    return (
        <section className="space-y-3">
            <h2 className="text-lg font-semibold">{title}</h2>

            {error ? (
                <ErrorState
                    title="Failed to load"
                    status={(error as HttpError).status}
                    message={error.message}
                    onRetry={() => {
                        const controller = new AbortController();
                        load(controller.signal);
                    }}
                />
            ) : loading && items.length === 0 ? (
                <CardSkeleton count={limit} />
            ) : items.length === 0 ? (
                <EmptyState
                    title="Nothing found"
                    description="No titles available right now."
                />
            ) : (
                <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
                    {items.map((m) => (
                        <MovieCard
                            key={m.id}
                            id={m.id}
                            title={m.title}
                            release_date={m.release_date}
                            vote_average={m.vote_average}
                            poster_path={m.poster_path}
                            backdrop_path={m.backdrop_path}
                            overview={m.overview}
                        />
                    ))}
                </div>
            )}
        </section>
    );
}
