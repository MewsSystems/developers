import { ImageOff } from "lucide-react";

type EmptyStateProps = {
    title?: string;
    description?: string;
    icon?: React.ReactNode;
    children?: React.ReactNode;
};

export default function EmptyState({
    title = "Nothing to show",
    description = "No data available for this section.",
    icon,
    children,
}: EmptyStateProps) {
    return (
        <div className="container py-40 px-8  rounded-xl border border-white/10 bg-white/5  text-center">
            <div className="mx-auto mb-3 grid h-12 w-12 place-items-center rounded-full bg-white/5">
                {icon ?? <ImageOff />}
            </div>
            <h2 className="text-lg font-semibold">{title}</h2>
            {description && (
                <p className="mt-2 text-sm text-neutral-400">{description}</p>
            )}
            {children && <div className="mt-4">{children}</div>}
        </div>
    );
}
