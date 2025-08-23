import MovieCard from "./MovieCard";
import CardSkeleton from "./CardSkeleton";
import EmptyState from "./EmptyState";
import ErrorState from "./ErrorState";
import type { TmdbMovie } from "../lib/tmdb";

export default function HorizontalRail({
    title,
    items,
    loading,
    error,
    onRetry,
}: {
    title: string;
    items: TmdbMovie[];
    loading: boolean;
    error: Error | null;
    onRetry?: () => void;
}) {
    return (
        <section className="space-y-3">
            <h2 className="text-lg font-semibold">{title}</h2>

            {error ? (
                <ErrorState
                    title="Failed to load"
                    message={error.message}
                    onRetry={onRetry}
                />
            ) : loading && items.length === 0 ? (
                <div className="-mx-1">
                    {/* Skeleton in a grid for first load to keep layout calm */}
                    <CardSkeleton count={6} />
                </div>
            ) : items.length === 0 ? (
                <EmptyState
                    title="Nothing here"
                    description="No titles available."
                />
            ) : (
                <div className="-mx-1 overflow-x-auto pb-2">
                    <div className="flex snap-x snap-mandatory gap-3 px-1">
                        {items.map((m) => (
                            <div
                                key={m.id}
                                className="snap-start shrink-0 w-40 sm:w-44"
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
