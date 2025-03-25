import "./card.style.css"
import { createSlug } from "../utils/createSlug"
import { Link } from "react-router"

export const Card = ({posterPath, title, id}) => {

    const createRoute = (id, title) => {
        const titleSlug = `${id}-${createSlug(title)}`
        return (`/${titleSlug}`)
    }

    return (
        <Link to={createRoute(id, title)} className="card">
            <img src={`https://image.tmdb.org/t/p/w500/${posterPath}.jpg`} alt={title} />
        </Link>
    )
}