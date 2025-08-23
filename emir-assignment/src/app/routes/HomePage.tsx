import { useEffect, useMemo, useState } from "react";
import { useSearchParams } from "react-router-dom";
import SearchHeaderSection from "../../features/search/SearchHeaderSection";
import HomeRails from "../../features/search/HomeRails";
import SearchResultsSection from "../../features/search/SearchResultsSection";
import { useDebounce } from "../../hooks/useDebounce";
import { useTmdbSearch } from "../../hooks/useTmdbSearch";

export default function HomePage() {
    const [searchParams, setSearchParams] = useSearchParams();

    // URL-sourced state
    const initialQ = useMemo(() => searchParams.get("q") ?? "", [searchParams]);
    const initialPage = useMemo(() => {
        const p = Number(searchParams.get("page") || "1");
        return Number.isFinite(p) && p > 0 ? p : 1;
    }, [searchParams]);

    const [query, setQuery] = useState(initialQ);
    const [page, setPage] = useState(initialPage);
    const { debounced, isDebouncing } = useDebounce(query, 500);

    // Reflect debounced value into URL
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

    // Sync local state if URL changed externally
    useEffect(() => {
        if (initialQ !== query) setQuery(initialQ);
        if (initialPage !== page) setPage(initialPage);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [initialQ, initialPage]);

    // Data fetching (encapsulated)
    const { items, totalPages, loading, error, retry, lastLoadedPage } =
        useTmdbSearch(debounced, page);

    const canLoadMore = !!debounced && totalPages !== null && page < totalPages;

    function handleLoadMore() {
        const nextPage = page + 1;
        const next = new URLSearchParams(searchParams);
        next.set("page", String(nextPage));
        setSearchParams(next);
        setPage(nextPage);
    }

    return (
        <div>
            <SearchHeaderSection value={query} onChange={setQuery} />

            <section className="relative space-y-8 container px-8 pb-8 z-[20] -mt-[310px]">
                {!debounced ? (
                    <HomeRails />
                ) : (
                    <SearchResultsSection
                        debounced={debounced}
                        items={items}
                        page={page}
                        totalPages={totalPages}
                        loading={loading}
                        error={error}
                        isDebouncing={isDebouncing}
                        canLoadMore={canLoadMore}
                        onLoadMore={handleLoadMore}
                        onRetry={() => {
                            lastLoadedPage.current = 0;
                            retry();
                        }}
                    />
                )}
            </section>
        </div>
    );
}
