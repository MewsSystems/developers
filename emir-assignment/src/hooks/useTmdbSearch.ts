import { useEffect, useRef, useState } from "react";
import { fetchJson, type TmdbMovie, type TmdbPage } from "../lib/tmdb";

type SearchResponse = TmdbPage<TmdbMovie>;

export function useTmdbSearch(query: string, page: number) {
    const [items, setItems] = useState<TmdbMovie[]>([]);
    const [totalPages, setTotalPages] = useState<number | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);

    const lastLoadedQuery = useRef<string>("");
    const lastLoadedPage = useRef<number>(0);
    const retryNonce = useRef(0);

    // expose a retry callback to re-run the fetch for the same page
    const retry = () => {
        lastLoadedPage.current = 0; // force effect to fetch current page
        retryNonce.current += 1;
    };

    useEffect(() => {
        if (!query) {
            setItems([]);
            setTotalPages(null);
            setError(null);
            return;
        }

        if (query !== lastLoadedQuery.current) {
            setItems([]);
            lastLoadedPage.current = 0;
            lastLoadedQuery.current = query;
        }
        if (page === lastLoadedPage.current) return;

        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<SearchResponse>(
            "/search/movie",
            { query, page },
            { signal: controller.signal }
        )
            .then((data: TmdbPage<TmdbMovie>) => {
                const results = (data.results ?? []).slice(0, 18); // exactly 18
                setTotalPages(data.total_pages ?? 1);
                setItems((prev) =>
                    page === 1 ? results : [...prev, ...results]
                );
                lastLoadedPage.current = page;
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
        // include retryNonce.current so calling retry() refetches current page
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [query, page, retryNonce.current]);

    return { items, totalPages, loading, error, retry, lastLoadedPage };
}
