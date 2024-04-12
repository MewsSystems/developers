import { useState } from 'react'
import { debounce } from 'lodash'
import { useMovieSearchQuery } from '../../../api/movies/hooks'

export const useMovieSearch = () => {
    const [currentSearchTitle, setCurrentSearchTitle] = useState('')
    const { data } = useMovieSearchQuery({ movieTitle: currentSearchTitle })

    const submitSearchedTitle = debounce((movieTitle: string) => {
        setCurrentSearchTitle(movieTitle)
    }, 500)

    return { submitSearchedTitle }
}
