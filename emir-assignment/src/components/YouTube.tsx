export default function YouTube({ id, title }: { id: string; title: string }) {
    const src = `https://www.youtube.com/embed/${encodeURIComponent(id)}`;
    return (
        <div className="relative w-full overflow-hidden rounded-lg border border-white/10 bg-black">
            <div className="aspect-video">
                <iframe
                    src={src}
                    title={title}
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                    allowFullScreen
                    className="h-full w-full"
                    loading="lazy"
                />
            </div>
        </div>
    );
}
