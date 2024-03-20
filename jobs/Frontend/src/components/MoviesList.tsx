import React, {useEffect, useState} from 'react'
import {searchMovies} from "@/services";
import { Movie} from "@/types";
import {Input, List, ListItemButton, Pagination} from "@mui/material";
import {useNavigate, useSearchParams} from "react-router-dom";
import { debounce } from '@mui/material/utils'

export const MoviesList = () => {
    const [movies, setMovies] = useState<Movie[]>([])
    const [pagination, setPagination] = useState({
        totalPages: 1,
        currentPage: 1
    })
    const [query, setQuery] = useState<string>('')
    const navigate = useNavigate();
    const [, setSearchParams] = useSearchParams()
    const {totalPages, currentPage} = pagination

    useEffect(() => {
        setSearchParams(`?${new URLSearchParams({ page: currentPage.toString() })}`)
    }, [currentPage]);

    useEffect(() => {
        searchMovies(query, currentPage).then(({results, total_pages}) => {
                setMovies(results)
                setPagination({
                    ...pagination,
                    totalPages: total_pages
                })
            }
        )
    }, [query, currentPage]);

    const handleChange = (event) => {
        const {value} = event.target
        setQuery(value)
    }

    const handleClick = (id: number) => {
        navigate(`/movie/${id}`);
    }

    const handleClickPagination = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
        setPagination({
            ...pagination,
            currentPage: newPage,
        })
    }

    return <div>
        <Input onChange={debounce(handleChange, 300)} placeholder={'Search for a movie'} />
        <List>
            {movies.map(
                movie =>
                    <ListItemButton key={movie.id} onClick={() => handleClick(movie.id)}>
                        {movie.title}
                    </ListItemButton>
            )}
        </List>
        <Pagination count={totalPages} onChange={handleClickPagination} />
        </div>
}
