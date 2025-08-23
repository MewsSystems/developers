import { Star, StarHalf } from "lucide-react";

export default function Stars({
    value,
    className = "",
}: {
    value: number;
    className?: string;
}) {
    // TMDB vote_average is 0–10; convert to 0–5
    const fiveScale = Math.max(0, Math.min(5, value / 2));
    const full = Math.floor(fiveScale);
    const hasHalf = fiveScale - full >= 0.5;
    const empty = 5 - full - (hasHalf ? 1 : 0);

    return (
        <div
            className={`flex items-center gap-0.5 text-yellow-400 ${className}`}
        >
            {Array.from({ length: full }).map((_, i) => (
                <Star key={`full-${i}`} size={16} fill="currentColor" />
            ))}
            {hasHalf && <StarHalf size={16} fill="currentColor" />}
            {Array.from({ length: empty }).map((_, i) => (
                <Star key={`empty-${i}`} size={16} className="opacity-30" />
            ))}
        </div>
    );
}
