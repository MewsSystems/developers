import "./cardContainer.style.css"
import { Card } from "./Card"
import { useState } from "react"
import { useEffect } from "react"
import { getFormattedMovies } from "../constants/getFormattedMovies"

export const CardContainer = ({page}) => {
    const [items, setItems] = useState([])

    useEffect(()=> {
        getFormattedMovies(page).then((data) => {
            if (page === 1) {
                setItems(data) // first load
            }
            else {
                setItems((prev) => 
                    [...prev, ...data]); // pagination
            }
        })
    },[page])

    return (
        <div className="card-container">
            {items
                .map((item, index) => (
                <Card key={`${item.id}-${page}-${index}`} posterPath={item.posterPath} title={item.title}/>
                ))
            }
        </div>
    )
}