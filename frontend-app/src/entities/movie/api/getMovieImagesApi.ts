import { baseGetApi } from "@/shared/api/baseApi";
import type { Images } from "@/entities/movie/types";

export async function getMovieImages({ movie_id }: { movie_id: string }) {
    return baseGetApi<Images>({ version: "3", path: `movie/${movie_id}/images`, params: {} })
}
