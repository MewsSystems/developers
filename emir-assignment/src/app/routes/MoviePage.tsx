import { useEffect, useMemo, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { backdropUrl, fetchJson, posterUrl } from "../../lib/tmdb";
import type { HttpError } from "../../lib/errors";
import PageLoader from "../../components/PageLoader";
import ErrorState from "../../components/ErrorState";
import Stars from "../../components/Stars";

/** Minimal type for /movie/{id} detail payload */
type SpokenLanguage = { english_name: string; iso_639_1: string; name: string };
type Genre = { id: number; name: string };
type Company = {
    id: number;
    name: string;
    logo_path: string | null;
    origin_country: string;
};

type TmdbMovieDetail = {
    id: number;
    title: string;
    overview: string;
    poster_path: string | null;
    backdrop_path: string | null;
    release_date?: string;
    vote_average: number;
    vote_count: number;
    runtime?: number;
    status?: string;
    original_language?: string;
    spoken_languages?: SpokenLanguage[];
    genres?: Genre[];
    production_companies?: Company[];
};

/** helpers */
function yearFrom(date?: string) {
    return (date && date.split("-")[0]) || "";
}

function formatRuntime(min?: number) {
    if (!min || min <= 0) return "";
    const h = Math.floor(min / 60);
    const m = min % 60;
    return `${h}h ${m}m`;
}

function joinNames<T extends { name: string }>(arr?: T[], max = 4) {
    if (!arr || arr.length === 0) return "";
    const names = arr.slice(0, max).map((g) => g.name);
    return names.join(", ");
}

export default function MoviePage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const movieId = useMemo(() => Number(id), [id]);

    const [data, setData] = useState<TmdbMovieDetail | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<Error | null>(null);

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
            .then((json) => {
                setData(json);
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

    const {
        title,
        overview,
        poster_path,
        backdrop_path,
        release_date,
        vote_average,
        vote_count,
        runtime,
        status,
        original_language,
        spoken_languages,
        genres,
        production_companies,
    } = data;

    const year = yearFrom(release_date);
    const heroBg = backdropUrl(backdrop_path ?? undefined, "w1280");
    const poster = posterUrl(poster_path ?? undefined, "w500");
    const genresText = joinNames(genres, 4);
    const langsText =
        spoken_languages && spoken_languages.length > 0
            ? spoken_languages
                  .map((l) => l.english_name || l.name)
                  .slice(0, 3)
                  .join(", ")
            : (original_language ?? "").toUpperCase();

    return (
        <article className="space-y-10">
            {/* Cinematic hero */}
            <section className="relative overflow-hidden rounded-2xl border border-white/10">
                {/* Backdrop background + gradient */}
                <div className="absolute inset-0">
                    {heroBg ? (
                        <div
                            className="absolute inset-0 bg-cover bg-center blur-sm scale-105"
                            style={{ backgroundImage: `url(${heroBg})` }}
                            aria-hidden
                        />
                    ) : (
                        <div
                            className="absolute inset-0 bg-neutral-900"
                            aria-hidden
                        />
                    )}
                    {/* Left-to-right darkening for text legibility */}
                    <div
                        className="absolute inset-0 bg-gradient-to-r from-black/80 via-black/50 to-black/20"
                        aria-hidden
                    />
                </div>

                <div className="relative grid gap-6 p-6 sm:p-8 md:grid-cols-2 md:gap-10 lg:p-12">
                    {/* Left: textual content */}
                    <div className="self-center">
                        <h1 className="text-2xl font-bold md:text-3xl lg:text-4xl">
                            {title}{" "}
                            {year ? (
                                <span className="text-neutral-400 font-medium">
                                    ({year})
                                </span>
                            ) : null}
                        </h1>

                        {/* Rating */}
                        <div className="mt-3 flex items-center gap-3">
                            <Stars value={vote_average} />
                            <span className="text-sm text-neutral-200/90">
                                {(vote_average / 2).toFixed(1)}/5
                            </span>
                            <span className="text-sm text-neutral-400">
                                · {vote_count.toLocaleString()} votes
                            </span>
                        </div>

                        {/* Overview snippet */}
                        {overview ? (
                            <p className="mt-4 max-w-prose text-neutral-200">
                                {overview}
                            </p>
                        ) : null}

                        {/* Key meta chips */}
                        <ul className="mt-5 flex flex-wrap gap-2 text-xs">
                            {runtime ? (
                                <li className="rounded-full bg-white/10 px-3 py-1">
                                    {formatRuntime(runtime)}
                                </li>
                            ) : null}
                            {genresText ? (
                                <li className="rounded-full bg-white/10 px-3 py-1">
                                    {genresText}
                                </li>
                            ) : null}
                            {status ? (
                                <li className="rounded-full bg-white/10 px-3 py-1">
                                    {status}
                                </li>
                            ) : null}
                            {langsText ? (
                                <li className="rounded-full bg-white/10 px-3 py-1">
                                    {langsText}
                                </li>
                            ) : null}
                            {release_date ? (
                                <li className="rounded-full bg-white/10 px-3 py-1">
                                    {release_date}
                                </li>
                            ) : null}
                        </ul>

                        {/* Production companies (names only; we’ll show logos maybe) */}
                        {production_companies &&
                        production_companies.length > 0 ? (
                            <p className="mt-4 text-xs text-neutral-400">
                                Production:{" "}
                                {production_companies
                                    .map((c) => c.name)
                                    .slice(0, 4)
                                    .join(", ")}
                                {production_companies.length > 4 ? "…" : ""}
                            </p>
                        ) : null}
                    </div>

                    {/* Right: poster card */}
                    <div className="relative mx-auto w-full max-w-sm self-stretch md:max-w-md">
                        <div className="relative overflow-hidden rounded-xl border border-white/10 bg-white/5 shadow-2xl">
                            {poster ? (
                                <img
                                    src={poster}
                                    alt={`${title} poster`}
                                    className="aspect-[2/3] w-full object-cover"
                                    loading="lazy"
                                />
                            ) : (
                                <div className="aspect-[2/3] grid place-items-center text-neutral-500">
                                    No poster
                                </div>
                            )}
                            {/* subtle bottom gradient to add depth */}
                            <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/30 to-transparent" />
                        </div>
                    </div>
                </div>
            </section>
        </article>
    );
}
