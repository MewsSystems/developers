import { useEffect } from "react";
import { useLocation } from "react-router-dom";

/** Scroll to top only when the path changes (ignore ?query and #hash). */
export default function ScrollToTop() {
    const { pathname, hash } = useLocation();

    useEffect(() => {
        // If navigating to an in-page anchor, let the browser handle it.
        if (hash) return;

        window.scrollTo({ top: 0, left: 0, behavior: "auto" });
    }, [pathname, hash]); // <-- only path, not search/hash

    return null;
}
