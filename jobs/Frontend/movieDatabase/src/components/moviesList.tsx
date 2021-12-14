import React, {FC, useEffect, useState} from "react";
import './moviesList.less'
import {MovieListItem} from "../types/movies";

interface MoviesListProps {
    movies?: MovieListItem[];
    setSelectedMovieId?(id: number);

}
export const MoviesList: FC<MoviesListProps> = ({movies = [], setSelectedMovieId= (i: number) => {}}) => {

    const getMoviesListItems = () => {
        return movies.map((m:MovieListItem, i: number ) => {
            return <li className={"movies-list-item"} key={m.id}><a onClick={() => setSelectedMovieId(m.id)}> {m.title} - ({new Date(m.release_date).getFullYear()}) - {getMovieStars(m.vote_average)}</a></li>
        })
    }

    const getMovieStars = (rating: number) => {
        if (rating === 0) {
            return <span>&#128683;</span>
        }

        const stars = [];

        const getStar = (i: number) => {
            return <span key={i}>&#11088;</span>
        }

        for (let i = 0; i < rating; i++) {
            stars.push(getStar(i))
        }
        return stars
    }

    return (<div className={"movies-list"}>
            <ul>
                {getMoviesListItems()}
            </ul>
        </div>
    )
}