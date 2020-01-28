import React from 'react'
import { MovieListWrapper } from '../Movie.styles'

export const MovieList: React.FC = ({ children }) => (
    <MovieListWrapper>{children}</MovieListWrapper>
)
