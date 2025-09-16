import { baseGetApi } from "@/shared/api/baseApi"
import type { PageList } from "@/shared/api/types"
import type { MovieListItem } from "@/entities/movie/types"

export async function getMovies({ query, page, language }: { query: string, page: string, language: string }) {
    const params =
    {
        query,
        page,
        include_adult: "false",
        language

    }
    return baseGetApi<PageList<MovieListItem>>({ version: "3", path: "search/movie", params })
}
