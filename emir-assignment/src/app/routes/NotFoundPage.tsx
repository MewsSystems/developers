import { Link } from "react-router-dom";
import EmptyState from "../../components/EmptyState";
import { SearchX } from "lucide-react";
import { useDocumentTitle } from "../../hooks/useDocumentTitle";

export default function NotFoundPage() {
    useDocumentTitle("Page Not Found");

    return (
        <div className="container py-10">
            <EmptyState
                title="Page not found (404)"
                description="The page you’re looking for doesn’t exist."
                icon={<SearchX />}
            >
                <Link
                    to="/"
                    className="inline-flex items-center rounded-lg bg-white/10 px-4 py-2 text-sm hover:bg-white/15"
                >
                    Go home
                </Link>
            </EmptyState>
        </div>
    );
}
