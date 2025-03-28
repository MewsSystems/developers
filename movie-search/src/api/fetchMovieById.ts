export const fetchMovieById = async (movieId: number) => {
    const response = await fetch(`https://api.themoviedb.org/3/movie/${movieId}?api_key=${import.meta.env.VITE_TMDB_API_KEY}`)
    if (!response.ok) {
        throw new Error("Failed to fetch movie details")
    }
    return response.json()
}