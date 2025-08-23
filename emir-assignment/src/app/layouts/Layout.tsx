import { Outlet, Link, NavLink, ScrollRestoration } from "react-router-dom";

export default function Layout() {
    return (
        <div className="min-h-dvh flex flex-col">
            <header className="border-b border-white/10">
                <div className="container flex items-center justify-between py-4">
                    <Link
                        to="/"
                        className="text-lg font-semibold tracking-tight"
                    >
                        CinEmir
                    </Link>

                    <nav className="hidden sm:flex items-center gap-6 text-sm text-neutral-300">
                        <NavLink
                            to="/"
                            className={({ isActive }) =>
                                `hover:text-white ${
                                    isActive ? "text-white" : ""
                                }`
                            }
                        >
                            Search
                        </NavLink>
                    </nav>
                </div>
            </header>

            <main className="flex-1">
                <div className="">
                    <Outlet />
                </div>
            </main>

            <footer className="border-t border-white/10">
                <div className="container py-6 text-xs text-neutral-400">
                    © {new Date().getFullYear()} CinEmir — Built with React +
                    Tailwind
                </div>
            </footer>

            {/* Scroll to top on route change; restores on back/forward */}
            <ScrollRestoration getKey={(location) => location.pathname} />
        </div>
    );
}
