export default function MediaGallery({ images }: { images: string[] }) {
    if (images.length === 0) return null;
    return (
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 md:grid-cols-4">
            {images.map((src, i) => (
                <div
                    key={i}
                    className="overflow-hidden rounded-lg border border-white/10 bg-white/5"
                >
                    <img
                        src={src}
                        alt=""
                        className="h-full w-full object-cover"
                        loading="lazy"
                    />
                </div>
            ))}
        </div>
    );
}
