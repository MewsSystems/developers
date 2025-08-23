import type { PropsWithChildren } from "react";

type SearchHeroProps = PropsWithChildren<{
    bgSrc?: string;
    title?: string;
    subtitle?: string;
    className?: string;
}>;

export default function SearchHero({
    bgSrc = "/assets/hero-placeholder.jpg",
    title = "Find your next movie",
    subtitle = "Search the TMDB catalog",
    className = "",
    children,
}: SearchHeroProps) {
    return (
        <section
            className={[
                "relative overflow-hidden",
                "h-[42vh] min-h-[300px] sm:h-[48vh] md:h-[56vh] lg:h-[700px]",
                className,
            ].join(" ")}
        >
            {/* Background image */}
            <div className="absolute inset-0">
                {bgSrc ? (
                    <div
                        className="absolute inset-0 bg-cover bg-center"
                        style={{ backgroundImage: `url(${bgSrc})` }}
                        aria-hidden
                    />
                ) : (
                    <div
                        className="absolute inset-0 bg-neutral-900"
                        aria-hidden
                    />
                )}

                {/* Subtle vignette + bottom fade to black */}
                <div
                    className="absolute inset-0"
                    aria-hidden
                    style={{
                        background:
                            // radial vignette center + bottom linear fade
                            "radial-gradient(120% 80% at 50% 20%, rgba(0,0,0,0.25) 0%, rgba(0,0,0,0.55) 60%, rgba(0,0,0,0.75) 100%)",
                    }}
                />
                <div
                    className="absolute inset-0 pointer-events-none"
                    aria-hidden
                    style={{
                        background:
                            "linear-gradient(to bottom, rgba(0,0,0,0) 0%, rgba(0,0,0,0.35) 40%, rgba(0,0,0,0.7) 70%, rgba(17,17,17,1) 100%)",
                    }}
                />
            </div>

            {/* Centered content */}
            <div className="relative z-10 h-full">
                <div className="container h-full">
                    <div className="flex h-full flex-col items-center justify-center text-center gap-4 sm:gap-5">
                        <div className="space-y-2">
                            <h1 className="text-2xl font-bold sm:text-3xl lg:text-4xl">
                                {title}
                            </h1>
                            {subtitle ? (
                                <p className="text-sm text-neutral-300">
                                    {subtitle}
                                </p>
                            ) : null}
                        </div>

                        {/* Search container */}
                        <div className="w-full max-w-2xl">{children}</div>

                        {/* Accent underline for flair */}
                        <div className="h-1 w-24 rounded-full bg-[#00ad99]/70"></div>
                    </div>
                </div>
            </div>
        </section>
    );
}
