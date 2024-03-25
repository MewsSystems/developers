import React, {useEffect, useState} from 'react'
import {searchMovies} from "@/services";
import { Movie} from "@/types";
import {Box, CircularProgress, Input, List, ListItemButton, Pagination} from "@mui/material";
import {useNavigate} from "react-router-dom";
import { debounce } from '@mui/material/utils'
import {getMoviePosterPath} from "@/utils/getMoviePosterPath";
import {useMovieSearch} from "@/context/MovieSearchProvider";

export const MoviesList = () => {
    const [movies, setMovies] = useState<Movie[]>([])
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const navigate = useNavigate();
    const {query, setQuery, totalPages, currentPage, setCurrentPage, setTotalPages} = useMovieSearch()

    useEffect(() => {
        setIsLoading(true)
        searchMovies(query, currentPage).then(({results, total_pages}) => {
                setMovies(results)
                setTotalPages(total_pages)
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
        setCurrentPage(newPage)
    }

    return <div>
        <Input onChange={debounce(handleChange, 300)} placeholder={'Search for a movie'} defaultValue={query} />
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
        <Pagination count={totalPages} onChange={handleClickPagination} page={currentPage} />
        </div>
}
