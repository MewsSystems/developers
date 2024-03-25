import {MoviesList} from "@/components";
import React, {useEffect, useState} from "react";
import {Movie} from "@/types";
import {useMovieSearch} from "@/context";
import {searchMovies} from "@/services";
import {CircularProgress, Input, Pagination} from "@mui/material";
import {debounce} from "@mui/material/utils";

export const MoviesListView = () => {
    const [movies, setMovies] = useState<Movie[]>([])
    const [isLoading, setIsLoading] = useState<boolean>(false)
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

    const handleClickPagination = (event: React.MouseEvent<HTMLButtonElement>, newPage: number) => {
        setCurrentPage(newPage)
    }

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const {value} = event.target
        setQuery(value)
    }

    return <div>
        <Input onChange={debounce(handleChange, 300)} placeholder={'Search for a movie'} defaultValue={query} />
        {isLoading && <CircularProgress size={20}/>}
        <MoviesList movies={movies} />
        <Pagination count={totalPages} onChange={handleClickPagination} page={currentPage} />
    </div>
}
