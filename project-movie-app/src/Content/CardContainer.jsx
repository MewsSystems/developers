import "./cardContainer.style.css"
import { Card } from "./Card"
import { useState } from "react"
import { useEffect } from "react"
import { getFormattedMovies } from "../constants/getFormattedMovies"

export const CardContainer = ({page, items}) => {

    return (
        <div className="card-container">
            {items
                .map((item, index) => (
                    <Card key={`${item.id}-${page}-${index}`} posterPath={item.posterPath} title={item.title} id={item.id}/>
                ))
            }
        </div>
    )
}