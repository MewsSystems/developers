import { useEffect, useMemo, useState } from "react";
import SearchHero from "./SearchHero";
import {
    fetchJson,
    type TmdbMovie,
    type TmdbPage,
    backdropUrl,
    posterUrl,
} from "../lib/tmdb";

type Picked = {
    id: number;
    title: string;
    bgSrc: string; // resolved image url (backdrop preferred)
};

function pickCandidate(list: TmdbMovie[], seed: number): Picked | null {
    // Filter by best-available image (prefer backdrop, else poster)
    const withBackdrop = list.filter((m) => !!m.backdrop_path);
    const pool =
        withBackdrop.length > 0
            ? withBackdrop
            : list.filter((m) => !!m.poster_path);

    if (pool.length === 0) return null;

    const idx = Math.floor(seed * pool.length) % pool.length;
    const m = pool[idx];

    // Large images: prefer backdrop w1280
    const bg =
        (m.backdrop_path && backdropUrl(m.backdrop_path, "w1280")) ||
        (m.poster_path && posterUrl(m.poster_path, "w780")) ||
        "";

    if (!bg) return null;

    return { id: m.id, title: m.title, bgSrc: bg };
}

export default function SearchHeroDynamic({
    children,
    title = "Discover movies you’ll love",
    subtitle = "Type a title to search the catalog",
}: {
    children: React.ReactNode;
    title?: string;
    subtitle?: string;
}) {
    const [picked, setPicked] = useState<Picked | null>(null);
    const [loading, setLoading] = useState(true);

    // Tiny seed so refreshes feel different (time-based)
    const seed = useMemo(() => Math.random(), []); // new on each page load

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);

        // Grab page 1 of popular (20 items), enough to almost guarantee images
        fetchJson<TmdbPage<TmdbMovie>>(
            "/movie/popular",
            { page: 1 },
            { signal: controller.signal }
        )
            .then((res) => {
                const cand = pickCandidate(res.results ?? [], seed);
                setPicked(cand ?? null);
            })
            .catch(() => {
                setPicked(null);
            })
            .finally(() => {
                if (!controller.signal.aborted) setLoading(false);
            });

        return () => controller.abort();
    }, [seed]);

    // Graceful skeleton state (no CLS: height is defined by SearchHero)
    if (loading) {
        return (
            <SearchHero
                bgSrc="" // no image, fallback to solid
                title={title}
                subtitle={subtitle}
            >
                <div className="w-full max-w-2xl">
                    {/* skeleton block shaped like the search bar */}
                    <div className="h-12 rounded-xl bg-white/10 animate-pulse motion-reduce:animate-none" />
                </div>
            </SearchHero>
        );
    }

    // If the API didn’t return anything usable, fallback to static asset
    const bgSrc =
        picked?.bgSrc ??
        "https://image.tmdb.org/t/p/original/A4ajXbOus829ZEtrnOcTL4CNT2P.jpg";

    return (
        <SearchHero bgSrc={bgSrc} title={title} subtitle={subtitle}>
            <div className="relative">
                {/* Search bar passed in as children */}
                {children}
            </div>
        </SearchHero>
    );
}
