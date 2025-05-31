export const fetchMovies = async ({query, pageParam = 1}: { query: string, pageParam: number }) => {
    if (query === "") return {results: [], total_pages: 1}
    const response = await fetch(`https://api.themoviedb.org/3/search/movie?api_key=${import.meta.env.VITE_TMDB_API_KEY}&query=${query}&page=${pageParam}`)
    return response.json()
}