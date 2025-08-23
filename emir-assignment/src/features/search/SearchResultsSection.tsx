import MovieCard from "../../components/MovieCard";
import CardSkeleton from "../../components/CardSkeleton";
import EmptyState from "../../components/EmptyState";
import ErrorState from "../../components/ErrorState";
import { Loader2 } from "lucide-react";
import type { HttpError } from "../../lib/errors";
import { prettySearchError } from "./prettySearchError";
import type { TmdbMovie } from "../../lib/tmdb";

export default function SearchResultsSection({
    debounced,
    items,
    page,
    totalPages,
    loading,
    error,
    isDebouncing,
    canLoadMore,
    onLoadMore,
    onRetry,
}: {
    debounced: string;
    items: TmdbMovie[];
    page: number;
    totalPages: number | null;
    loading: boolean;
    error: Error | null;
    isDebouncing: boolean;
    canLoadMore: boolean;
    onLoadMore: () => void;
    onRetry: () => void;
}) {
    if (error) {
        const pretty = prettySearchError(error);
        return (
            <ErrorState
                title={pretty.title}
                status={(error as HttpError).status}
                message={pretty.message}
                onRetry={onRetry}
            />
        );
    }

    if (loading && items.length === 0) {
        return <CardSkeleton count={12} />;
    }

    if (items.length === 0) {
        return (
            <EmptyState
                title="No results"
                description={`We couldn’t find anything for “${debounced}”. Try another title.`}
            />
        );
    }

    return (
        <>
            <div className="grid grid-cols-2 gap-y-8 gap-x-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
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

            <div className="mt-14 flex items-center justify-center">
                {canLoadMore ? (
                    <button
                        onClick={onLoadMore}
                        disabled={loading}
                        className={`
              inline-flex items-center justify-center gap-2
              rounded-lg px-5 py-2 text-lg font-medium
              transition-colors duration-200
              ${
                  loading
                      ? "bg-[#00ad99]/40 cursor-not-allowed"
                      : "bg-[#00ad99] hover:bg-[#009b89]"
              }
              text-white disabled:opacity-60
            `}
                    >
                        {loading ? (
                            <>
                                <Loader2 className="h-4 w-4 animate-spin" />
                                Loading…
                            </>
                        ) : (
                            "Load more"
                        )}
                    </button>
                ) : (
                    <p className="text-sm text-neutral-500">End of results</p>
                )}
            </div>

            <div className="text-xs text-neutral-500 text-center">
                <span>
                    {isDebouncing ? "Typing…" : "Showing results"} for{" "}
                    <em>“{debounced}”</em>
                    {totalPages ? ` · Page ${page} of ${totalPages}` : null}
                </span>
            </div>
        </>
    );
}
