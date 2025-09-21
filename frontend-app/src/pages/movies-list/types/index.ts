import type { MovieListItem } from "@/entities/movie/types"

export type MovieCardItem = {
    movie: MovieListItem;
    poster_img: string | undefined
    release_date_locale: string;
}