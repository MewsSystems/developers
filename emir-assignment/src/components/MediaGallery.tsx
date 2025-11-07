export default function MediaGallery({
    images,
    aspect,
}: {
    images: string[];
    aspect?: "poster" | "backdrop";
}) {
    const aspectClass =
        aspect === "poster"
            ? "aspect-[2/3]"
            : aspect === "backdrop"
            ? "aspect-video"
            : "";
    return (
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 md:grid-cols-4">
            {images.map((src, i) => (
                <div
                    key={i}
                    className={`overflow-hidden rounded-lg border border-white/10 bg-white/5 ${aspectClass}`}
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
