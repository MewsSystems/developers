export const getPosterSrc = (posterPath: string | null) => {
    return posterPath === null ? '/poster-placeholder.jpg' : `https://image.tmdb.org/t/p/w500${posterPath}`
}