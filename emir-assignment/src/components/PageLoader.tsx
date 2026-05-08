import Spinner from "./Spinner";

export default function PageLoader({ label = "Loading…" }: { label?: string }) {
    return (
        <div className="min-h-[60dvh] grid place-items-center">
            <div className="flex items-center gap-3 text-neutral-300">
                <Spinner size={24} aria-label={label} />
                <span className="text-sm">{label}</span>
            </div>
        </div>
    );
}
