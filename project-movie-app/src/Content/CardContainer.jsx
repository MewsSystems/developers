import "./cardContainer.style.css"
import { Card } from "./Card"
import { BASE_API_URL, API_KEY } from "../constants/API.constants"
import { useState } from "react"
import { useEffect } from "react"

export const CardContainer = () => {
    const [items, setItems] = useState([])

    useEffect(()=> {
        const fetchPaginatedMovies = async () => {
            try {
                const reqMovies = await fetch(`${BASE_API_URL}/movie/popular?api_key=${API_KEY}&page=1`)
                const movies = await reqMovies.json()

                setItems(movies.results)
            }
            catch (error) {
                console.error("Error fetching popular movies:", error)
            }
        }
        fetchPaginatedMovies()
    },[])

    return (
        <div className="card-container">
            {items
                .map((item) => (
                <Card posterPath={item.poster_path} title={item.original_title}/>
                ))
            }
        </div>
    )
}