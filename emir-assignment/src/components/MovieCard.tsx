import { Link } from "react-router-dom";
import Stars from "./Stars";
import { posterUrl, backdropUrl } from "../lib/tmdb";
import { ImageOff } from "lucide-react";

type Props = {
    id: number;
    title: string;
    release_date?: string;
    vote_average: number;
    poster_path: string | null;
    backdrop_path?: string | null;
    overview?: string;
};

function yearFrom(date?: string) {
    if (!date) return "-";
    const y = date.split("-")[0];
    return y || "";
}

export default function MovieCard({
    id,
    title,
    release_date,
    vote_average,
    poster_path,
    backdrop_path,
    overview,
}: Props) {
    const href = `/movie/${id}`;
    const img = posterUrl(poster_path, "w342");
    const bg = backdropUrl(backdrop_path ?? undefined, "w780"); // overlay background if available
    const year = yearFrom(release_date);

    return (
        <Link
            to={href}
            className="group block"
            aria-label={`${title}${year ? ` (${year})` : ""}`}
        >
            <div className="relative overflow-hidden rounded-lg bg-white/5">
                {/* Base poster */}
                {img ? (
                    <img
                        src={img}
                        alt={title}
                        loading="lazy"
                        className="aspect-[2/3] w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]"
                    />
                ) : (
                    <div
                        className="aspect-[2/3] grid place-items-center text-white"
                        data-testid="no-poster"
                    >
                        <ImageOff size={28} />
                    </div>
                )}

                {/* Hover overlay */}
                <div
                    className={`
            pointer-events-none absolute inset-0 opacity-0 transition-opacity duration-300
            group-hover:opacity-100
          `}
                >
                    {/* Optional backdrop image behind overlay */}
                    {bg ? (
                        <div
                            className="absolute inset-0 scale-105 bg-cover bg-center blur-sm"
                            style={{ backgroundImage: `url(${bg})` }}
                            aria-hidden
                        />
                    ) : (
                        <div
                            className="absolute inset-0 bg-black/30"
                            aria-hidden
                        />
                    )}

                    {/* Gradient to make text readable */}
                    <div
                        className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/30 to-transparent"
                        aria-hidden
                    />

                    {/* Content */}
                    <div className="absolute inset-x-0 bottom-0 p-3 text-white">
                        <div className="flex items-center justify-between gap-2">
                            <span className="line-clamp-1 text-sm font-semibold">
                                {title}
                            </span>
                            <span className="shrink-0 text-xs text-neutral-200">
                                {year}
                            </span>
                        </div>

                        {overview ? (
                            <p className="mt-2 line-clamp-3 text-[13px] text-neutral-200/90">
                                {overview}
                            </p>
                        ) : null}

                        <div className="mt-2 flex items-center justify-between">
                            <Stars value={vote_average} />
                            <span className="text-xs text-neutral-200/80">
                                {(vote_average / 2).toFixed(1)}/5
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            {/* Static info under the card (still visible when not hovering) */}
            <div className="mt-3">
                <h3 className="line-clamp-1 text-sm font-semibold text-white">
                    {title}
                </h3>
                <div className="mt-1 flex items-center justify-between">
                    <span className="text-xs text-neutral-400">{year}</span>
                    <Stars value={vote_average} />
                </div>
            </div>
        </Link>
    );
}
