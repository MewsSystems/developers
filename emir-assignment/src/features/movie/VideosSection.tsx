import { useEffect, useState } from "react";
import { fetchJson, type TmdbVideo } from "../../lib/tmdb";
import PageLoader from "../../components/PageLoader";
import ErrorState from "../../components/ErrorState";
import EmptyState from "../../components/EmptyState";
import YouTube from "../../components/YouTube";

export default function VideosSection({ movieId }: { movieId: number }) {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);
    const [videos, setVideos] = useState<TmdbVideo[]>([]);

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);
        setError(null);

        fetchJson<{ id: number; results: TmdbVideo[] }>(
            `/movie/${movieId}/videos`,
            undefined,
            { signal: controller.signal }
        )
            .then((data) => {
                const yt = (data.results ?? [])
                    .filter(
                        (v) =>
                            v.site === "YouTube" &&
                            /Trailer|Teaser|Clip|Featurette/i.test(v.type)
                    )
                    .slice(0, 4);
                setVideos(yt);
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

    if (loading) return <PageLoader label="Loading videos…" />;
    if (error)
        return (
            <ErrorState title="Failed to load videos" message={error.message} />
        );
    if (videos.length === 0)
        return (
            <EmptyState
                title="No videos"
                description="No trailers or teasers available."
            />
        );

    return (
        <div className="grid gap-4 md:grid-cols-2">
            {videos.map((v) => (
                <YouTube key={v.id} id={v.key} title={v.name} />
            ))}
        </div>
    );
}
