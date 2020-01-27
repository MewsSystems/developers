import React from 'react'
import { MovieListWrapper } from './MovieList.styles'

export const MovieList: React.FC = ({ children }) => (
    <MovieListWrapper>{children}</MovieListWrapper>
)
