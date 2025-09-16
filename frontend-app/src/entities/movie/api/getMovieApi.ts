import { baseGetApi } from "@/shared/api/baseApi";
import { append_to_response, type MovieDetailsAppended } from "@/entities/movie/types";

export async function getMovie({ movie_id }: { movie_id: string }, { language, session_id }: { language: string, session_id: string }) {
    return baseGetApi<MovieDetailsAppended>({ version: "3", path: `movie/${movie_id}`, params: { session_id, language, append_to_response } })
}
