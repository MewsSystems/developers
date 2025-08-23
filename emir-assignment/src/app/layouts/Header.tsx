import { Link, NavLink } from "react-router-dom";
import { Clapperboard, Sparkles, UserRound } from "lucide-react";

const ACCENT = "#00ad99";

function Logo() {
    return (
        <Link
            to="/"
            aria-label="CinEmir home"
            className="group inline-flex items-center gap-2 rounded px-1 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-[var(--accent)]"
            style={{ ["--accent" as any]: ACCENT }}
        >
            {/* Icon mark */}
            <span className="relative grid h-9 w-9 place-items-center rounded-lg bg-[color:var(--accent)]/12 ring-1 ring-[color:var(--accent)]/30">
                <Clapperboard
                    className="h-5 w-5 text-[color:var(--accent)] transition-transform duration-200 group-hover:scale-110"
                    aria-hidden
                />
                {/* tiny sparkle accent */}
                <Sparkles
                    className="absolute -right-1 -top-1 h-3 w-3 text-[color:var(--accent)]/90"
                    aria-hidden
                />
            </span>
            <span className="text-lg font-semibold tracking-tight">
                CinEmir
            </span>
        </Link>
    );
}

function FakeAvatar() {
    return (
        <button
            type="button"
            title="Account"
            aria-label="Account"
            className="relative inline-grid h-9 w-9 place-items-center rounded-full bg-white/5 ring-1 ring-white/10 hover:bg-white/10 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-[var(--accent)]"
            style={{ ["--accent" as any]: ACCENT }}
        >
            <UserRound className="h-5 w-5 text-neutral-300" aria-hidden />
            {/* tiny online dot for flair */}
            <span className="absolute bottom-0.5 right-0.5 h-2 w-2 rounded-full bg-[color:var(--accent)] ring-2 ring-black" />
        </button>
    );
}

export default function Header() {
    return (
        <header className="sticky top-0 z-50 border-b border-white/10 bg-black/60 backdrop-blur-md">
            <div className="container flex h-14 items-center justify-between gap-3">
                {/* Left: Logo */}
                <Logo />

                {/* Center: Menu */}
                <nav
                    className="hidden md:flex items-center gap-3 text-sm text-neutral-300"
                    aria-label="Primary"
                >
                    {[
                        { to: "/", label: "Home" },
                        { to: "/", label: "Categories" },
                        { to: "/", label: "Movies" },
                        { to: "/", label: "TV Series" },
                        { to: "/", label: "Contact" },
                    ].map((item) => (
                        <NavLink
                            key={item.label}
                            to={item.to}
                            className={({ isActive }) =>
                                [
                                    "rounded-md px-3 py-1.5 transition-colors",
                                    "hover:text-white hover:bg-white/5",
                                    "focus-visible:outline-none focus-visible:ring-2",
                                    `focus-visible:ring-[${ACCENT}]`,
                                    isActive ? "text-white bg-white/5" : "",
                                ].join(" ")
                            }
                        >
                            {item.label}
                        </NavLink>
                    ))}
                </nav>

                {/* Right: Fake avatar */}
                <div className="flex items-center gap-2">
                    <FakeAvatar />
                </div>
            </div>
        </header>
    );
}
