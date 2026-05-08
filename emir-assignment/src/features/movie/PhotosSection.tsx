import { useEffect, useState } from "react";
import { fetchJson, imgUrl, type TmdbImages } from "../../lib/tmdb";
import PageLoader from "../../components/PageLoader";
import ErrorState from "../../components/ErrorState";
import EmptyState from "../../components/EmptyState";
import MediaGallery from "../../components/MediaGallery";

export default function PhotosSection({ movieId }: { movieId: number }) {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);
    const [backdrops, setBackdrops] = useState<string[]>([]);
    const [posters, setPosters] = useState<string[]>([]);

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<TmdbImages>(`/movie/${movieId}/images`, undefined, {
            signal: controller.signal,
            raw: true,
        })
            .then((data) => {
                const b = (data.backdrops ?? [])
                    .slice(0, 12)
                    .map((i) => imgUrl(i.file_path, "w780"))
                    .filter(Boolean);
                const p = (data.posters ?? [])
                    .slice(0, 12)
                    .map((i) => imgUrl(i.file_path, "w500"))
                    .filter(Boolean);
                setBackdrops(b);
                setPosters(p);
            })
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
    }, [movieId]);

    if (loading) return <PageLoader label="Loading photos…" />;
    if (error)
        return (
            <ErrorState title="Failed to load photos" message={error.message} />
        );
    if (backdrops.length === 0 && posters.length === 0) {
        return (
            <EmptyState
                title="No photos"
                description="No backdrops or posters available."
            />
        );
    }

    return (
        <div className="space-y-6">
            {backdrops.length > 0 && (
                <div className="space-y-3">
                    <h3 className="text-sm font-semibold text-neutral-300">
                        Backdrops
                    </h3>
                    <MediaGallery images={backdrops} />
                </div>
            )}
            {posters.length > 0 && (
                <div className="space-y-3">
                    <h3 className="text-sm font-semibold text-neutral-300">
                        Posters
                    </h3>
                    <MediaGallery images={posters} />
                </div>
            )}
        </div>
    );
}
