import { useState } from 'react'
import { HomePageViewProps, UseHomeHookProps } from '../types'
import { useMovieSearch } from './useMovieSearch'

export const usePage = (props: UseHomeHookProps): HomePageViewProps => {
    const { submitSearchedTitle } = useMovieSearch()

    return { submitSearchedTitle }
}
