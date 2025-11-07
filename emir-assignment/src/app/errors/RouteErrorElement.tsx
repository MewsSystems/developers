import {
    isRouteErrorResponse,
    useRouteError,
    useNavigate,
} from "react-router-dom";
import ErrorState from "../../components/ErrorState";

export default function RouteErrorElement() {
    const err = useRouteError();
    const navigate = useNavigate();

    if (isRouteErrorResponse(err)) {
        return (
            <div className="container py-10">
                <ErrorState
                    title={err.statusText || "Request failed"}
                    status={err.status}
                    message={
                        typeof err.data === "string" ? err.data : undefined
                    }
                    onRetry={() => navigate(0)}
                />
            </div>
        );
    }

    let message = "Unknown error";
    if (err instanceof Error) message = err.message;

    return (
        <div className="container py-10">
            <ErrorState
                title="Unexpected error"
                message={message}
                onRetry={() => navigate(0)}
            />
        </div>
    );
}
