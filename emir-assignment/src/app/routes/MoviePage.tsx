import { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { fetchJson, posterUrl } from "../../lib/tmdb";
import type { HttpError } from "../../lib/errors";
import PageLoader from "../../components/PageLoader";
import ErrorState from "../../components/ErrorState";
import PillTabs from "../../components/PillTabs";
import MovieHero from "../../features/movie/MovieHero";
import OverviewSection from "../../features/movie/OverviewSection";
import VideosSection from "../../features/movie/VideosSection";
import PhotosSection from "../../features/movie/PhotosSection";
import { formatRuntime, joinNames } from "../../features/movie/utils";
import type { TmdbMovieDetail } from "../../features/movie/types";
import CastSection from "../../features/movie/CastSection";

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

    if (loading) return <PageLoader label="Loading movie…" />;

    if (error || !data) {
        const http = error as HttpError | null;
        return (
            <div className="py-8">
                <ErrorState
                    title="Couldn’t load movie"
                    status={http?.status}
                    message={error ? error.message : "Unknown error"}
                    onRetry={() => navigate(0)}
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
        </article>
    );
}
