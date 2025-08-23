import { useEffect, useState } from "react";
import { fetchJson, type TmdbMovie, type TmdbPage } from "../lib/tmdb";
import HorizontalRail from "./HorizontalRail";

type Props = {
    title: string;
    endpoint: string; // e.g., '/trending/movie/day'
};

export default function RailSection({ title, endpoint }: Props) {
    const [items, setItems] = useState<TmdbMovie[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<TmdbPage<TmdbMovie>>(
            endpoint,
            { page: 1 },
            { signal: controller.signal }
        )
            .then((data) => setItems(data.results || []))
            .catch((err: unknown) => {
                if (controller.signal.aborted) return;
                setError(
                    err instanceof Error ? err : new Error("Unknown error")
                );
            })
            .finally(() => {
                if (!controller.signal.aborted) setLoading(false);
            });

        return () => controller.abort();
    }, [endpoint]);

    return (
        <div className="space-y-3">
            <HorizontalRail
                title={title}
                items={items}
                loading={loading}
                error={error}
                onRetry={() => {
                    // force refetch by resetting state; useEffect will run again due to endpoint same?
                    // Trigger by toggling key: simplest approach is to just rerun effect by changing local state:
                    setItems([]);
                    setError(null);
                    setLoading(true);
                    const controller = new AbortController();
                    fetchJson<TmdbPage<TmdbMovie>>(
                        endpoint,
                        { page: 1 },
                        { signal: controller.signal }
                    )
                        .then((data) => setItems(data.results || []))
                        .catch((err: unknown) => {
                            if (controller.signal.aborted) return;
                            setError(
                                err instanceof Error
                                    ? err
                                    : new Error("Unknown error")
                            );
                        })
                        .finally(() => {
                            if (!controller.signal.aborted) setLoading(false);
                        });
                }}
            />
        </div>
    );
}
