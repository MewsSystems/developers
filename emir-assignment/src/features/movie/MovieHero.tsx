import { backdropUrl, posterUrl } from "../../lib/tmdb";
import Stars from "../../components/Stars";
import { formatRuntime, joinNames, yearFrom } from "./utils";
import type { TmdbMovieDetail } from "./types";

export default function MovieHero({ movie }: { movie: TmdbMovieDetail }) {
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
    } = movie;

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
        <section className="relative overflow-hidden">
            {/* Backdrop + gradient */}
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
                <div
                    className="absolute inset-0 bg-gradient-to-r from-black/80 via-black/50 to-black/20"
                    aria-hidden
                />
            </div>

            <div className="container relative grid gap-6 p-6 sm:p-8 md:grid-cols-2 md:gap-10 lg:p-12">
                {/* Left: text */}
                <div className="self-center">
                    <h1 className="text-2xl font-bold md:text-3xl lg:text-4xl">
                        {title}{" "}
                        {year ? (
                            <span className="text-neutral-400 font-medium">
                                ({year})
                            </span>
                        ) : null}
                    </h1>

                    <div className="mt-3 flex items-center gap-3">
                        <Stars value={vote_average} />
                        <span className="text-sm text-neutral-200/90">
                            {(vote_average / 2).toFixed(1)}/5
                        </span>
                        <span className="text-sm text-neutral-400">
                            · {vote_count.toLocaleString()} votes
                        </span>
                    </div>

                    {overview ? (
                        <p className="mt-4 max-w-prose text-neutral-200">
                            {overview}
                        </p>
                    ) : null}

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

                    {production_companies && production_companies.length > 0 ? (
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

                {/* Right: poster */}
                <div className="relative mx-auto w-full max-w-sm self-stretch md:max-w-md">
                    <div className="relative overflow-hidden rounded-xl border border-white/10 bg-white/5 shadow-2xl">
                        {poster ? (
                            <img
                                src={poster}
                                alt={`${title} poster`}
                                // Reserve space: intrinsic dimensions of TMDB "w500" posters are 2:3 (e.g., 500x750)
                                width={500}
                                height={750}
                                // Responsive rendering with no CLS
                                className="block w-full h-auto object-cover"
                                // Improve perceived speed
                                decoding="async"
                                loading="eager"
                            />
                        ) : (
                            // Fallback reserves space similarly (no aspect utilities)
                            <div
                                className="block w-full"
                                style={{ height: (750 / 500) * 320 }}
                            />
                        )}
                        <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/30 to-transparent" />
                    </div>
                </div>
            </div>
        </section>
    );
}
