import { Heart, Clapperboard } from "lucide-react";
import type { CSSProperties } from "react";
import { Link, NavLink } from "react-router-dom";

const ACCENT = "#00ad99";

const accentVar: CSSProperties & Record<"--accent", string> = {
    "--accent": ACCENT,
};

export default function Footer() {
    return (
        <footer className="mt-20 border-t border-white/10 bg-black/60">
            <div className="container py-10">
                {/* Top row: brand + small nav */}
                <div className="flex flex-col items-center gap-4 sm:flex-row sm:justify-between">
                    <Link
                        to="/"
                        className="inline-flex items-center gap-2 text-sm text-neutral-300 hover:text-white"
                    >
                        <span
                            className="grid h-7 w-7 place-items-center rounded-md bg-[color:var(--accent)]/12 ring-1 ring-[color:var(--accent)]/30"
                            style={accentVar}
                        >
                            <Clapperboard className="h-4 w-4 text-[color:var(--accent)]" />
                        </span>
                        <span className="font-medium">CinEmir</span>
                    </Link>

                    <nav className="flex flex-wrap items-center justify-center gap-4 text-xs text-neutral-400">
                        {[
                            "Home",
                            "Categories",
                            "Movies",
                            "TV Series",
                            "Contact",
                            "About",
                        ].map((label) => (
                            <NavLink
                                key={label}
                                to="/"
                                className="hover:text-neutral-200 transition-colors"
                            >
                                {label}
                            </NavLink>
                        ))}
                    </nav>
                </div>

                {/* Divider */}
                <div className="my-6 h-px w-full bg-white/10" />

                {/* Bottom row: copyright + made with love */}
                <div className="flex flex-col items-center justify-between gap-2 text-center text-xs text-neutral-400 sm:flex-row">
                    <p>© {new Date().getFullYear()} CinEmir Movies</p>
                    <p className="flex items-center gap-1 text-neutral-500">
                        Made with{" "}
                        <Heart className="text-[#00ad99] w-4 h-4" aria-hidden />{" "}
                        in Germany by
                        <span className="font-medium text-neutral-300">
                            &nbsp;Emir Bayraktar
                        </span>
                    </p>
                </div>
            </div>
        </footer>
    );
}
