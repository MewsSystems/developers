import { useEffect, useRef } from "react";
import { Search, X } from "lucide-react";

type Props = {
    value: string;
    onChange: (next: string) => void;
    autoFocus?: boolean;
    placeholder?: string;
    className?: string;
};

export default function SearchBar({
    value,
    onChange,
    autoFocus = false,
    placeholder = "Search for a movie…",
    className = "",
}: Props) {
    const inputRef = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        if (autoFocus) inputRef.current?.focus();
    }, [autoFocus]);

    return (
        <div
            role="search"
            aria-label="Movie search"
            className={[
                // layout + base
                "group relative flex w-full items-center gap-2 rounded-2xl",
                "px-4 py-2.5",
                "bg-white/5 backdrop-blur-sm",
                // border + ring on container (not input)
                "border border-white/10",
                "transition-shadow",
                "focus-within:ring-2 focus-within:ring-[#00ad99] focus-within:border-transparent",
                // subtle glow on focus
                "focus-within:shadow-[0_0_0_3px_rgba(0,173,153,0.15)]",
                // hover affordance
                "hover:border-white/20",
                className,
            ].join(" ")}
        >
            <Search
                className="shrink-0 opacity-70 transition-colors group-focus-within:text-[#00ad99]"
                aria-hidden
            />

            <label htmlFor="movie-search" className="sr-only">
                Search movies
            </label>
            <input
                id="movie-search"
                ref={inputRef}
                type="text"
                value={value}
                onChange={(e) => onChange(e.target.value)}
                onKeyDown={(e) => {
                    if (e.key === "Escape") onChange("");
                }}
                placeholder={placeholder}
                className={[
                    "w-full bg-transparent",
                    "text-[15px] leading-6",
                    "placeholder:text-neutral-500",
                    "outline-none border-0 focus:border-0 focus:outline-none focus:ring-0 :focus-visible:outline-4",
                ].join(" ")}
                aria-label="Search movies"
                inputMode="search"
                autoCorrect="off"
                autoCapitalize="none"
                spellCheck={false}
            />

            {value ? (
                <button
                    type="button"
                    onClick={() => onChange("")}
                    className={[
                        "ml-1 grid place-items-center rounded-lg p-1.5",
                        "text-neutral-400 hover:text-white",
                        "hover:bg-white/10",
                        "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-[#00ad99]",
                        "transition-colors",
                    ].join(" ")}
                    aria-label="Clear search"
                    title="Clear"
                >
                    <X className="h-4 w-4" />
                </button>
            ) : null}
        </div>
    );
}
