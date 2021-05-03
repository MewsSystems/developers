import {
    ActionTypes, SetCurrentPageAction,
    SetLoadingAction, SetMoviePageAction, SetSearchMovieTitleAction,
} from './types'
import {MoviesPage} from "../types";


const setLoading = (loading: boolean): SetLoadingAction => ({
    type: ActionTypes.SET_LOADING,
    payload: loading,
})

const setSearchMovieTitle= (title: string): SetSearchMovieTitleAction => ({
    type: ActionTypes.SET_SEARCH_MOVIE_TITLE,
    payload: title,
})

const setCurrentPage= (currentPage:number): SetCurrentPageAction => ({
    type: ActionTypes.SET_CURRENT_PAGE,
    payload: currentPage,
})

const setMoviePage= (moviePage: MoviesPage): SetMoviePageAction => ({
    type: ActionTypes.SET_MOVIE_PAGE,
    payload: moviePage,
})

export const appActionCreators = {
    setLoading,
    setSearchMovieTitle,
    setCurrentPage,
    setMoviePage
}
