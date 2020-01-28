import React, { useMemo, useState } from 'react'
import styled from 'styled-components'
import Head from '../components/Head/Head'
import { Col, Container, Row } from '../utils/layout/grid.layout'
import { InputAutosuggest } from '../components/Input/InputAutosuggest/InputAutosuggest'
import { Button } from '../components/Button/Button'
import { getMoviesByQuery } from '../utils/api/movie.requests'
import { Suggestion, useAutosuggest } from '../utils/hooks/useAutosuggest'
import { mapMovieSearchQueryResultToSuggestions } from '../utils/mappers/movie.mappers'
import { useRouter } from 'next/router'
import { AppRoutes } from '../utils/constants/routes.constants'
import { useDispatch, useSelector } from 'react-redux'
import { submitSearchQuery } from '../store/movies/movies.actions'
import { ApplicationState } from '../store/store'
import { MovieElement } from '../components/Movie/MovieElement/MovieElement'
import { MovieList } from '../components/Movie/MovieList/MovieList'
import { MovieModel } from '../utils/types/model'
import { usePagination } from '../utils/hooks/usePagination'
import { MovieSearchQueryResultDto } from '../utils/types/dto'

const Home = () => {
    const [searchValue, setValue] = useState<string>('')

    const router = useRouter()
    const dispatch = useDispatch()

    const searchResults = useSelector(
        (state: ApplicationState) => state.movies.results
    )
    const isLoading = useSelector(
        (state: ApplicationState) => state.app.isLoading
    )

    const {
        paginatedData,
        isDataToPaginate,
        isFirstPage,
        isLastPage,
        onPaginateNext,
        onPaginatePrev,
    } = usePagination<MovieModel>({
        data: searchResults,
        recordsPerPage: 5,
    })

    const {
        handleSelect,
        suggestions,
        handleSearchQuery,
        clearSuggestions,
    } = useAutosuggest<MovieSearchQueryResultDto>({
        onFetch: getMoviesByQuery,
        onSelect: handleMovieSelect,
        onPipeData: mapMovieSearchQueryResultToSuggestions,
        initialSuggestions: [],
    })

    const handleSearchSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        clearSuggestions()
        dispatch(submitSearchQuery(searchValue))
    }

    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        handleSearchQuery(e)
        setValue(e.currentTarget.value)
    }

    function handleMovieSelect({ value, label }: Suggestion) {
        setValue(label)
        handleViewMovie(value)()
    }

    const handleViewMovie = (movieID: string) => {
        return () => {
            router.push({ pathname: AppRoutes.Movie, query: { id: movieID } })
        }
    }

    const renderMovies = (movies: MovieModel[]) =>
        useMemo(() => {
            return isLoading ? (
                <li>Loading mewies...</li>
            ) : (
                movies.map(movie => (
                    <MovieElement
                        title={movie.title}
                        posterPath={movie.posterPath}
                        releaseDate={movie.releaseDate}
                        onClick={handleViewMovie(movie.id)}
                    />
                ))
            )
        }, [searchResults, paginatedData])

    const renderSearchingByValueBar = () =>
        useMemo(() => {
            return (
                isDataToPaginate &&
                searchValue && (
                    <p>
                        Displaying results for: <strong>{searchValue}</strong>
                    </p>
                )
            )
        }, [searchResults, isDataToPaginate])

    return (
        <>
            <Head
                url={''}
                ogImage={''}
                title="Mewies App"
                description={'Find your mewie!'}
            />
            <MovieSearchHeader>
                <Container>
                    <Row>
                        <Col size={12}>
                            <img
                                src="/static/mewies_logo.png"
                                alt="Mewies - logo of movie search application"
                            />
                            <MovieSearchForm onSubmit={handleSearchSubmit}>
                                <InputAutosuggest
                                    placeholder={'search for a movie...'}
                                    name={'search-input'}
                                    suggestions={suggestions}
                                    onSelect={handleSelect}
                                    onChange={handleSearch}
                                    value={searchValue}
                                />
                                <Button type={'submit'}>Search</Button>
                            </MovieSearchForm>
                            {renderSearchingByValueBar()}
                        </Col>
                    </Row>
                </Container>
            </MovieSearchHeader>
            <section>
                <Container>
                    <Row>
                        <Col size={12}>
                            <MovieList>{renderMovies(paginatedData)}</MovieList>
                            {isDataToPaginate && (
                                <PaginationNavigation>
                                    <div>
                                        <Button
                                            disabled={isFirstPage}
                                            onClick={onPaginatePrev}
                                        >
                                            Previous
                                        </Button>
                                        <Button
                                            disabled={isLastPage}
                                            onClick={onPaginateNext}
                                        >
                                            Next
                                        </Button>
                                    </div>
                                </PaginationNavigation>
                            )}
                        </Col>
                    </Row>
                </Container>
            </section>
        </>
    )
}

export const MovieSearchHeader = styled.header`
    margin: 20px 0 20px 0;
`

export const MovieSearchForm = styled.form`
    display: flex;
    justify-content: space-between;
    min-width: 520px;
    max-width: 720px;
    margin: 20px 0 20px 0;
    > div:first-of-type {
        width: 80%;
    }
    button {
        width: 20%;
        margin-left: 8px;
    }
`

export const PaginationNavigation = styled.div`
    display: flex;
    justify-content: flex-end;
    > div:first-of-type {
        display: flex;
        justify-content: space-between;
        min-width: 260px;
    }
`

export default Home
