import { Link } from "react-router-dom";
import Stars from "./Stars";
import { posterUrl } from "../lib/tmdb";
import { ImageOff } from "lucide-react";

type Props = {
    id: number;
    title: string;
    release_date?: string;
    vote_average: number;
    poster_path: string | null;
};

function yearFrom(date?: string) {
    if (!date) return "";
    const y = date.split("-")[0];
    return y || "";
}

export default function MovieCard({
    id,
    title,
    release_date,
    vote_average,
    poster_path,
}: Props) {
    const href = `/movie/${id}`;
    const img = posterUrl(poster_path, "w342");
    const year = yearFrom(release_date);

    return (
        <Link
            to={href}
            className="group block"
            aria-label={`${title}${year ? ` (${year})` : ""}`}
        >
            <div className="relative overflow-hidden rounded-lg bg-white/5">
                {img ? (
                    <img
                        src={img}
                        alt={title}
                        loading="lazy"
                        className="aspect-[2/3] w-full object-cover transition-transform duration-300 group-hover:scale-[1.03]"
                    />
                ) : (
                    <div className="aspect-[2/3] grid place-items-center text-neutral-500">
                        <ImageOff size={28} />
                    </div>
                )}
            </div>

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
