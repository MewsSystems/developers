import { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { fetchJson, posterUrl } from "../../lib/tmdb";
import type { HttpError } from "../../lib/errors";
import ErrorState from "../../components/ErrorState";
import PillTabs from "../../components/PillTabs";
import MovieHero from "../../features/movie/MovieHero";
import OverviewSection from "../../features/movie/OverviewSection";
import VideosSection from "../../features/movie/VideosSection";
import PhotosSection from "../../features/movie/PhotosSection";
import { formatRuntime, joinNames } from "../../features/movie/utils";
import type { TmdbMovieDetail } from "../../features/movie/types";
import CastSection from "../../features/movie/CastSection";
import SimilarSection from "../../features/movie/SimilarSection";
import MovieHeroSkeleton from "../../features/movie/MovieHeroSkeleton";

function prettyDetailError(err: Error | null): {
    title: string;
    message?: string;
} {
    if (!err) return { title: "Couldn’t load movie" };
    const http = err as HttpError;
    const s = http.status;
    if (s === 404) {
        return {
            title: "Movie not found",
            message: "This title may have been removed or is unavailable.",
        };
    }
    if (s === 401 || s === 403) {
        return {
            title: "TMDB authentication failed",
            message: "Verify your API key and permissions.",
        };
    }
    if (s === 429) {
        return {
            title: "Rate limited by TMDB",
            message: "Too many requests. Please try again shortly.",
        };
    }
    if (s && s >= 500) {
        return {
            title: "TMDB is having issues",
            message: `Server error (${s}). Please retry.`,
        };
    }
    return { title: "Couldn’t load movie", message: err.message };
}

export default function MoviePage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const movieId = useMemo(() => Number(id), [id]);

    const [data, setData] = useState<TmdbMovieDetail | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<Error | null>(null);

    // Tab state synced with URL (?tab=overview|videos|photos)
    const tabParam = searchParams.get("tab");
    const [tab, setTab] = useState<"overview" | "videos" | "photos">(
        tabParam === "videos" || tabParam === "photos" ? tabParam : "overview"
    );

    useEffect(() => {
        const current = searchParams.get("tab");
        if (current !== tab) {
            const next = new URLSearchParams(searchParams);
            next.set("tab", tab);
            setSearchParams(next, { replace: true });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [tab]);

    // Fetch details
    useEffect(() => {
        if (!movieId || Number.isNaN(movieId)) {
            setError(new Error("Invalid movie id"));
            setLoading(false);
            return;
        }

        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<TmdbMovieDetail>(`/movie/${movieId}`, undefined, {
            signal: controller.signal,
        })
            .then((json) => setData(json))
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
    }, [movieId]);

    if (loading || (!data && !error)) {
        // skeleton while loading
        return (
            <article className="space-y-10 mb-20">
                <MovieHeroSkeleton />
                {/* Skeletons for tabs/sections below */}
                <div className="container px-6 sm:px-8 lg:px-12">
                    <div className="h-9 w-64 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                    <div className="mt-6 grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4">
                        {Array.from({ length: 8 }).map((_, i) => (
                            <div
                                key={i}
                                className="h-40 rounded-lg bg-white/10 animate-pulse motion-reduce:animate-none"
                            />
                        ))}
                    </div>
                </div>
            </article>
        );
    }

    if (error || !data) {
        const pretty = prettyDetailError(error);
        return (
            <div className="py-8">
                <ErrorState
                    title={pretty.title}
                    status={(error as HttpError | null)?.status}
                    message={pretty.message}
                    onRetry={() => navigate(0)} // full refetch of this route
                />
            </div>
        );
    }

    // Prepare overview details
    const genresText = joinNames(data.genres, 4);
    const langsText =
        data.spoken_languages && data.spoken_languages.length > 0
            ? data.spoken_languages
                  .map((l) => l.english_name || l.name)
                  .slice(0, 3)
                  .join(", ")
            : (data.original_language ?? "").toUpperCase();

    return (
        <article className="space-y-10">
            <MovieHero movie={data} />

            <section className="space-y-6 container p-8">
                <PillTabs
                    tabs={[
                        { key: "overview", label: "Overview" },
                        { key: "videos", label: "Videos" },
                        { key: "photos", label: "Photos" },
                    ]}
                    value={tab}
                    onChange={(k) =>
                        setTab(k as "overview" | "videos" | "photos")
                    }
                />

                {tab === "overview" && (
                    <OverviewSection
                        posterUrl={posterUrl(
                            data.poster_path ?? undefined,
                            "w500"
                        )}
                        overview={data.overview}
                        details={{
                            genres: genresText || undefined,
                            languages: langsText || undefined,
                            status: data.status,
                            runtime: formatRuntime(data.runtime),
                            releaseDate: data.release_date,
                            budget: data.budget,
                            revenue: data.revenue,
                            companies: (data.production_companies ?? []).map(
                                (c) => c.name
                            ),
                        }}
                    />
                )}

                {tab === "videos" && <VideosSection movieId={data.id} />}
                {tab === "photos" && <PhotosSection movieId={data.id} />}
            </section>
            <div className="container p-8">
                <CastSection movieId={data.id} />
            </div>
            <div className="container p-8">
                <SimilarSection movieId={data.id} />
            </div>
        </article>
    );
}
