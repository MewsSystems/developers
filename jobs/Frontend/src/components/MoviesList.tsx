import React, {useEffect, useState} from 'react'
import {searchMovies} from "@/services";
import { Movie} from "@/types";
import {Box, CircularProgress, Input, InputAdornment, List, ListItemButton, Pagination} from "@mui/material";
import {useNavigate, useSearchParams} from "react-router-dom";
import { debounce } from '@mui/material/utils'
import {getMoviePosterPath} from "@/utils/getMoviePosterPath";

export const MoviesList = () => {
    const [movies, setMovies] = useState<Movie[]>([])
    const [pagination, setPagination] = useState({
        totalPages: 1,
        currentPage: 1
    })
    const [query, setQuery] = useState<string>('')
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const navigate = useNavigate();
    const [, setSearchParams] = useSearchParams()
    const {totalPages, currentPage} = pagination

    useEffect(() => {
        setSearchParams(`?${new URLSearchParams({ page: currentPage.toString() })}`)
    }, [currentPage]);

    useEffect(() => {
        setIsLoading(true)
        searchMovies(query, currentPage).then(({results, total_pages}) => {
                setMovies(results)
                setPagination({
                    ...pagination,
                    totalPages: total_pages
                })
                setIsLoading(false)
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
        {isLoading && <CircularProgress size={20}/>}
        <List>
            {movies.map(
                movie =>
                    <ListItemButton key={movie.id} onClick={() => handleClick(movie.id)}>
                        <Box
                            sx={{maxWidth: { xs: 50 }}}
                            component="img"
                            src={getMoviePosterPath(200, movie.poster_path)}
                        />
                        {movie.title}
                    </ListItemButton>
            )}
        </List>
        <Pagination count={totalPages} onChange={handleClickPagination} />
        </div>
}
