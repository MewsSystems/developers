import React from 'react'
import { MovieElementWrapper } from './MovieElement.styles'

interface MovieElementProps {
    title: string
    posterPath: string
    releaseDate: string
}

export const MovieElement: React.FC<MovieElementProps> = ({
    posterPath,
    title,
    releaseDate,
}) => (
    <MovieElementWrapper>
        <figure>
            <img
                height={240}
                src={'https://image.tmdb.org/t/p/original' + posterPath}
                alt={title}
            />
            <figcaption>
                {title} <span>{releaseDate.split('-')[0]}</span>
            </figcaption>
        </figure>
    </MovieElementWrapper>
)
