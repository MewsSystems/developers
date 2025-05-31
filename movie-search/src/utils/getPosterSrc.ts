export const getPosterSrc = (posterPath: string | null, format: "webp" | "jpg" = "jpg") => {
    if (!posterPath) {
        return format === "webp" ? "/poster-placeholder.webp" : "/poster-placeholder.jpg";
    }

    return `https://image.tmdb.org/t/p/w500${posterPath}`;
};
