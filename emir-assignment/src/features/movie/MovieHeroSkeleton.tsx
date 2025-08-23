export default function MovieHeroSkeleton() {
    return (
        <section
            className="relative overflow-hidden"
            aria-hidden="true" // skeleton is decorative
        >
            {/* Background placeholder + subtle gradient */}
            <div className="absolute inset-0">
                <div className="absolute inset-0 bg-neutral-900" />
                <div className="absolute inset-0 bg-gradient-to-r from-black/80 via-black/50 to-black/20" />
            </div>

            <div className="container relative grid gap-6 p-6 sm:p-8 md:grid-cols-2 md:gap-10 lg:p-12">
                {/* Left (60%): title, meta, overview blocks */}
                <div className="self-center">
                    {/* Title */}
                    <div className="h-7 w-2/3 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                    {/* Year chip-ish */}
                    <div className="mt-3 h-4 w-24 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />

                    {/* Stars + counts row */}
                    <div className="mt-4 flex items-center gap-3">
                        <div className="h-4 w-24 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                        <div className="h-3 w-16 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                        <div className="h-3 w-24 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                    </div>

                    {/* Overview paragraphs */}
                    <div className="mt-5 space-y-2">
                        <div className="h-4 w-full max-w-prose rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                        <div className="h-4 w-[90%] max-w-prose rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                        <div className="h-4 w-2/3 max-w-prose rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                    </div>

                    {/* Meta chips */}
                    <div className="mt-5 flex flex-wrap gap-2">
                        {Array.from({ length: 4 }).map((_, i) => (
                            <div
                                key={i}
                                className="h-6 w-24 rounded-full bg-white/10 animate-pulse motion-reduce:animate-none"
                            />
                        ))}
                    </div>

                    {/* Production names */}
                    <div className="mt-4 h-3 w-1/2 rounded bg-white/10 animate-pulse motion-reduce:animate-none" />
                </div>

                {/* Right (40%): poster card block */}
                <div className="relative mx-auto w-full max-w-sm self-stretch md:max-w-md">
                    <div className="relative overflow-hidden rounded-xl border border-white/10 bg-white/5 shadow-2xl">
                        {/* Give the container a sensible min height so layout is stable */}
                        <div className="h-[22rem] sm:h-[26rem] md:h-[40rem] bg-white/10 animate-pulse motion-reduce:animate-none" />
                        <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/30 to-transparent" />
                    </div>
                </div>
            </div>
        </section>
    );
}
