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
            className={`relative flex items-center rounded-xl border border-white/10 bg-white/5 px-3 py-2 ${className}`}
            role="search"
            aria-label="Movie search"
        >
            <Search className="mr-2 shrink-0 opacity-70" aria-hidden />
            <input
                ref={inputRef}
                type="text"
                value={value}
                onChange={(e) => onChange(e.target.value)}
                onKeyDown={(e) => {
                    if (e.key === "Escape") onChange("");
                }}
                placeholder={placeholder}
                className="w-full bg-transparent outline-none placeholder:text-neutral-500"
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
                    className="ml-2 rounded p-1 hover:bg-white/10"
                    aria-label="Clear search"
                    title="Clear"
                >
                    <X />
                </button>
            ) : null}
        </div>
    );
}
