import { useEffect, useMemo, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";
import SearchBar from "../../components/SearchBar";
import { useDebounce } from "../../hooks/useDebounce";
import { fetchJson, type TmdbMovie, type TmdbPage } from "../../lib/tmdb";
import CardSkeleton from "../../components/CardSkeleton";
import EmptyState from "../../components/EmptyState";
import ErrorState from "../../components/ErrorState";
import MovieCard from "../../components/MovieCard";
import type { HttpError } from "../../lib/errors";

type SearchResponse = TmdbPage<TmdbMovie>;

export default function HomePage() {
    const [searchParams, setSearchParams] = useSearchParams();

    // URL-sourced state
    const initialQ = useMemo(() => searchParams.get("q") ?? "", [searchParams]);
    const initialPage = useMemo(() => {
        const p = Number(searchParams.get("page") || "1");
        return Number.isFinite(p) && p > 0 ? p : 1;
    }, [searchParams]);

    const [query, setQuery] = useState(initialQ);
    const { debounced, isDebouncing } = useDebounce(query, 500);

    // Data state
    const [items, setItems] = useState<TmdbMovie[]>([]);
    const [page, setPage] = useState(initialPage);
    const [totalPages, setTotalPages] = useState<number | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    const lastLoadedQuery = useRef<string>(""); // to reset on query change
    const lastLoadedPage = useRef<number>(0);

    // Keep input in sync if URL changes externally
    useEffect(() => {
        if (initialQ !== query) setQuery(initialQ);
        if (initialPage !== page) setPage(initialPage);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [initialQ, initialPage]);

    // When typing settles, update ?q= and reset ?page=
    useEffect(() => {
        const next = new URLSearchParams(searchParams);
        if (debounced) {
            next.set("q", debounced);
            next.set("page", "1");
        } else {
            next.delete("q");
            next.delete("page");
        }
        if (next.toString() !== searchParams.toString()) {
            setSearchParams(next, { replace: false });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [debounced]);

    // Fetch data whenever debounced query or page changes
    useEffect(() => {
        if (!debounced) {
            // No query: reset list and stop (we’ll show a hint)
            setItems([]);
            setTotalPages(null);
            setError(null);
            return;
        }

        // Reset list when the query changed
        if (debounced !== lastLoadedQuery.current) {
            setItems([]);
            lastLoadedPage.current = 0;
            lastLoadedQuery.current = debounced;
        }

        // Avoid duplicate fetches for the same page
        if (page === lastLoadedPage.current) return;

        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<SearchResponse>(
            "/search/movie",
            { query: debounced, page },
            { signal: controller.signal }
        )
            .then((data) => {
                setTotalPages(data.total_pages ?? 1);
                setItems((prev) =>
                    page === 1 ? data.results : [...prev, ...data.results]
                );
                lastLoadedPage.current = page;
            })
            .catch((err: unknown) => {
                if (controller.signal.aborted) return;
                if (err instanceof Error) {
                    setError(err);
                } else {
                    setError(new Error("Unknown error"));
                }
            })
            .finally(() => {
                if (!controller.signal.aborted) setLoading(false);
            });

        return () => controller.abort();
    }, [debounced, page]);

    const canLoadMore = !!debounced && totalPages !== null && page < totalPages;

    function handleLoadMore() {
        const nextPage = page + 1;
        const next = new URLSearchParams(searchParams);
        next.set("page", String(nextPage));
        setSearchParams(next);
        setPage(nextPage); // Optimistic local update; effect will fetch
    }

    return (
        <section className="space-y-6">
            <h1 className="text-2xl font-semibold">Search</h1>

            <SearchBar
                value={query}
                onChange={setQuery}
                autoFocus
                className="max-w-xl"
                placeholder="Search for a movie (e.g., Interstellar)…"
            />

            {!debounced && (
                <p className="text-sm text-neutral-400">
                    Start typing a title above. Your search is shareable via{" "}
                    <code>?q=</code>.
                </p>
            )}

            {error ? (
                <ErrorState
                    title="Search failed"
                    status={(error as HttpError).status}
                    message={error.message}
                    onRetry={() => {
                        lastLoadedPage.current = 0;
                        setPage((p) => p);
                    }}
                />
            ) : debounced ? (
                <>
                    {/* First page loading skeleton */}
                    {loading && items.length === 0 ? (
                        <CardSkeleton count={12} />
                    ) : items.length === 0 ? (
                        <EmptyState
                            title="No results"
                            description={`We couldn’t find anything for “${debounced}”. Try another title.`}
                        />
                    ) : (
                        <>
                            <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
                                {items.map((m) => (
                                    <MovieCard
                                        key={m.id}
                                        id={m.id}
                                        title={m.title}
                                        release_date={m.release_date}
                                        vote_average={m.vote_average}
                                        poster_path={m.poster_path}
                                    />
                                ))}
                            </div>

                            <div className="mt-6 flex items-center justify-center">
                                {canLoadMore ? (
                                    <button
                                        onClick={handleLoadMore}
                                        className="inline-flex items-center rounded-lg bg-white/10 px-4 py-2 text-sm hover:bg-white/15 disabled:opacity-50"
                                        disabled={loading}
                                    >
                                        {loading ? "Loading…" : "Load more"}
                                    </button>
                                ) : (
                                    <p className="text-sm text-neutral-500">
                                        End of results
                                    </p>
                                )}
                            </div>
                        </>
                    )}
                </>
            ) : null}

            <div className="text-xs text-neutral-500">
                {debounced && (
                    <span>
                        {isDebouncing ? "Typing…" : "Showing results"} for{" "}
                        <em>“{debounced}”</em>
                        {totalPages ? ` · Page ${page} of ${totalPages}` : null}
                    </span>
                )}
            </div>
        </section>
    );
}
