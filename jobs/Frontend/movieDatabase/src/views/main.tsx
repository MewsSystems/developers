import React, {FC, useEffect, useState} from 'react';
import 'purecss/build/pure.css'

import {getMovieById, getMoviesByTitle} from "../utils/moviesService";
import {MoviesList} from "../components/moviesList";
import './main.less'
import {MovieDetails, MovieListItem} from "../types/movies";
import {MovieDetail} from "../components/movieDetail";

export const MainView: FC = () => {
    const [movies, setMovies] = useState<MovieListItem[]>()
    const [selectedMovieId, setSelectedMovieId] = useState<number>()
    const [selectedMovieDetails, setSelectedMovieDetails] = useState<MovieDetails>()
    const [searchTitle, setSearchTitle] = useState<string>("")
    const [sortBy, setSortBy] = useState<string>('Name')
    const [isLoadingList, setIsLoadingList] = useState<boolean>(false)
    const [isLoadingDetail, setIsLoadingDetail] = useState<boolean>(false)

    useEffect(() => {
            const validationTimer = setTimeout(() => {onUserStopTyping(); }, 700)
            return () => {
                clearTimeout(validationTimer);
            };
        },
        [searchTitle]
    );

    useEffect(() => {
        if (movies) {
            const result = [...movies.sort(movieSort)]
            setMovies(result);
            }
        },
        [sortBy]
    );

    useEffect(() => {
        const getMovieDetailData = async () => {
            setIsLoadingDetail(true)
            const result = await getMovieById(selectedMovieId)
            setSelectedMovieDetails(result)
            setIsLoadingDetail(false)
        }

        if (selectedMovieId) {
            getMovieDetailData()
        }

        },
        [selectedMovieId]
    );

    const movieSort = (a: MovieListItem, b: MovieListItem) => {
        switch (sortBy) {
            case 'Name':
                return a.title.toLowerCase().localeCompare(b.title.toLowerCase())
                break;
            case 'Release Date':
                return new Date(b.release_date).getTime() - new Date(a.release_date).getTime()
                break;
            case 'Vote Average':
                return b.vote_average - a.vote_average
            break
        }
    }

    const onUserStopTyping = async () => {
        if (searchTitle) {
            setIsLoadingList(true)
            const result = await getMoviesByTitle(searchTitle);
            result.sort(movieSort)
            setMovies(result);
            setIsLoadingList(false)
        }
    }

    const getLoadingSpinner = () => {
        return <div className={"loadingContainer"}>
            <div id={"loading"}/>
        </div>
    }

    return (
        <div>
            <div className="pure-g movieSearch">
                <div className="pure-u-1-3"/>
                <div className="pure-u-1-3">
                    <h1>&#127909; Search Movie Titles</h1>
                        <form className="pure-form">
                            <fieldset>
                                <input id="searchText"
                                       type="text"
                                       autoFocus={true}
                                       size={50}
                                       value={searchTitle}
                                       onChange={(e: React.ChangeEvent<HTMLInputElement>) => {setSearchTitle(e.target.value)}}
                                       placeholder='Example: "Terminator 2"' /> Sort by &nbsp;
                                <select value={sortBy} onChange={(e: React.ChangeEvent<HTMLSelectElement>) => setSortBy(e.target.value)}>
                                    <option value={'Name'}>Name</option>
                                    <option value={'Release Date'}>Release Date</option>
                                    <option value={'Vote Average'}>Vote Average</option>
                                </select>
                            </fieldset>
                        </form>
                </div>
                <div className="pure-u-1-3"/>
            </div>
            <div className="pure-u-1-3">
                {isLoadingList ? getLoadingSpinner() : null}
                <MoviesList movies={movies} setSelectedMovieId={setSelectedMovieId}/></div>
            <div className="pure-u-1-3"/>
            <div className="pure-u-1-3">
                {isLoadingDetail ? getLoadingSpinner() : null}
                {selectedMovieDetails ? <MovieDetail movie={selectedMovieDetails} /> : null}
            </div>
        </div>
    )
}