import { baseGetApi } from "@/shared/api/baseApi";
import type { Collection } from "@/entities/movie/types";

export async function getMovieCollection({ collection_id }: { collection_id: number }, { language }: { language: string }) {
    return baseGetApi<Collection>({ version: "3", path: `collection/${collection_id}`, params: { language } })
}
