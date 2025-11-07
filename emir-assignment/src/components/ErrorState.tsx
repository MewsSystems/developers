import { CircleAlert } from "lucide-react";

type ErrorStateProps = {
    title?: string;
    message?: string;
    status?: number;
    onRetry?: () => void;
};

export default function ErrorState({
    title = "Something went wrong",
    message,
    status,
    onRetry,
}: ErrorStateProps) {
    return (
        <div className="container p-8 rounded-xl border border-white/10 bg-white/5 text-center">
            <div className="mx-auto mb-3 grid h-12 w-12 place-items-center rounded-full bg-red-500/10">
                <CircleAlert className="text-red-400" />
            </div>
            <h2 className="text-lg font-semibold">
                {title}
                {typeof status === "number" ? ` (${status})` : ""}
            </h2>
            {message && (
                <p className="mt-2 text-sm text-neutral-400">{message}</p>
            )}
            {onRetry && (
                <button
                    onClick={onRetry}
                    className="mt-4 inline-flex items-center gap-2 rounded-lg bg-white/10 px-4 py-2 text-sm hover:bg-white/15"
                >
                    Try again
                </button>
            )}
        </div>
    );
}
