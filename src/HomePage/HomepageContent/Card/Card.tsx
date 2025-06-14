import { createSlug } from "../../../utils/createSlug"
import { CardLink, PosterImage } from "./style"
import { MovieCard } from "./types"

export const Card: React.FC<MovieCard> = ({posterPath, title, id}) => {

    const createRoute = (id: number, title: string): string => {
        const titleSlug = `${id}-${createSlug(title)}`
        return (`/${titleSlug}`)
    }

    return (
        <CardLink to={createRoute(id, title)}>
            <PosterImage src={`https://image.tmdb.org/t/p/w500/${posterPath}.jpg`} alt={title} />
        </CardLink>
    )
}