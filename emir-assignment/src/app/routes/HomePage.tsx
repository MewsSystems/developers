import { useEffect, useMemo, useState } from "react";
import { useSearchParams } from "react-router-dom";
import SearchBar from "../../components/SearchBar";
import { useDebounce } from "../../hooks/useDebounce";

export default function HomePage() {
    const [searchParams, setSearchParams] = useSearchParams();
    const initialQ = useMemo(() => searchParams.get("q") ?? "", [searchParams]);

    const [query, setQuery] = useState(initialQ);
    const { debounced, isDebouncing } = useDebounce(query, 500);

    // Keep local input in sync if URL changes externally (back/forward, pasted link, etc.)
    useEffect(() => {
        if (initialQ !== query) setQuery(initialQ);
    }, [initialQ]);

    // When typing settles, update ?q= in the URL (and clear pagination if present)
    useEffect(() => {
        const next = new URLSearchParams(searchParams);
        if (debounced) {
            next.set("q", debounced);
            next.delete("page"); // reset pagination when search term changes
        } else {
            next.delete("q");
            next.delete("page");
        }

        // Only push when something actually changed to avoid history spam
        const changed = next.toString() !== searchParams.toString();
        if (changed) setSearchParams(next, { replace: false });
    }, [debounced, searchParams, setSearchParams]);

    return (
        <section className="space-y-6">
            <h1 className="text-2xl font-semibold">Search</h1>

            <SearchBar
                value={query}
                onChange={setQuery}
                autoFocus
                className="max-w-xl"
                placeholder="Search for a movie (e.g., Oppenheimer)…"
            />

            <div className="text-sm text-neutral-400">
                {query && (
                    <span>
                        {isDebouncing ? "Typing…" : "Ready"} · Shareable URL
                        synced to <code>?q=</code>
                    </span>
                )}
                {!query && <span>Try a title to get started.</span>}
            </div>

            {/* The results grid will be added here.
          For now this page focuses on URL-synced search input. */}
        </section>
    );
}
