import "./cardContainer.style.css"
import { Card } from "./Card"
import { useState } from "react"
import { useEffect } from "react"
import { getFormattedMovies } from "../constants/getFormattedMovies"

export const CardContainer = ({page = 1}) => {
    const [items, setItems] = useState([])

    useEffect(()=> {
        getFormattedMovies(page).then((data) => {
            setItems(data)
        })
    },[])

    return (
        <div className="card-container">
            {items
                .map((item) => (
                <Card key={item.id} posterPath={item.posterPath} title={item.title}/>
                ))
            }
        </div>
    )
}