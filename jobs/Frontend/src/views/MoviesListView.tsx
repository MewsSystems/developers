import { MovieList } from '@/components'
import React, { useEffect, useState } from 'react'
import { Movie } from '@/types'
import { useMovieSearch } from '@/context'
import {movieService} from '@/services'
import {
  CircularProgress,
  Grid,
  Input,
  Pagination,
  styled,
  Typography,
} from '@mui/material'
import { debounce } from '@mui/material/utils'

const loaderSize = 20

const StyledInput = styled(Input)`
  width: calc(100% - ${loaderSize}px);
`

const StyledPagination = styled(Pagination)`
  ul {
    justify-content: end;
  }
`

export const MoviesListView = () => {
  const [movies, setMovies] = useState<Movie[]>([])
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const {
    query,
    setQuery,
    totalPages,
    currentPage,
    setCurrentPage,
    setTotalPages,
  } = useMovieSearch()
  const hasResults = query && Boolean(movies.length)
  const hasNoResults = query && !Boolean(movies.length) && !isLoading

  useEffect(() => {
    setIsLoading(true)

    movieService.searchMovies(query, currentPage).then(({ results, total_pages }) => {
      setMovies(results)
      setTotalPages(total_pages)
      setIsLoading(false)
    })

  }, [query, currentPage])

  const handleClickPagination = (
    event: React.MouseEvent<HTMLButtonElement>,
    newPage: number
  ) => {
    setCurrentPage(newPage)
  }

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.target
    setQuery(value)
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12}>
        <StyledInput
          onChange={debounce(handleInputChange, 300)}
          placeholder={'Search for a movie'}
          defaultValue={query}
        />
        {isLoading && <CircularProgress size={loaderSize} />}
      </Grid>
      <Grid item xs={12}>
        {hasResults && <MovieList movies={movies} />}
        {hasNoResults && <Typography variant={'body1'} color={'textSecondary'}>No results</Typography>}
      </Grid>
      {hasResults && (
        <Grid item xs={12}>
          <StyledPagination
            count={totalPages}
            onChange={handleClickPagination}
            page={currentPage}
          />
        </Grid>
      )}
    </Grid>
  )
}
