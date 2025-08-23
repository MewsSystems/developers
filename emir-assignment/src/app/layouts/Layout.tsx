import { Outlet } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";
import ScrollToTop from "../router/ScrollToTop";

export default function Layout() {
    return (
        <div className="min-h-dvh flex flex-col">
            <Header />

            <ScrollToTop />

            <main className="flex-1">
                <Outlet />
            </main>

            <Footer />
        </div>
    );
}
