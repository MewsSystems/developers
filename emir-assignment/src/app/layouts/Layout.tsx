import { Outlet, ScrollRestoration } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";

export default function Layout() {
    return (
        <div className="min-h-dvh flex flex-col">
            <Header />

            <main className="flex-1">
                <Outlet />
            </main>

            <Footer />

            {/* Scroll to top on route change; restores on back/forward */}
            <ScrollRestoration getKey={(location) => location.pathname} />
        </div>
    );
}
