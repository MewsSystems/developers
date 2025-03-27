import "./card.style.css"
import { createSlug } from "../utils/createSlug"
import { Link } from "react-router"
import { MovieCard } from "../types/movie"

export const Card: React.FC<MovieCard> = ({posterPath, title, id}) => {

    const createRoute = (id: number, title: string): string => {
        const titleSlug = `${id}-${createSlug(title)}`
        return (`/${titleSlug}`)
    }

    return (
        <Link to={createRoute(id, title)} className="card">
            <img src={`https://image.tmdb.org/t/p/w500/${posterPath}.jpg`} alt={title} />
        </Link>
    )
}