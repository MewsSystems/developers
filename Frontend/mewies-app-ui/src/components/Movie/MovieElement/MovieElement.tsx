import React from 'react'
import { formatReleaseDate } from '../../../utils/helpers/component.helpers'
import { MovieElementWrapper } from '../Movie.styles'

interface MovieElementProps {
    title: string
    posterPath: string
    releaseDate: string
    onClick?(): void
}

export const MovieElement: React.FC<MovieElementProps> = ({
    posterPath,
    title,
    releaseDate,
    onClick,
}) => (
    <MovieElementWrapper>
        <figure>
            <img
                onClick={onClick}
                height={240}
                src={'https://image.tmdb.org/t/p/original' + posterPath}
                alt={title}
            />
            <figcaption>
                {title} <br />
                {releaseDate && <span>{formatReleaseDate(releaseDate)}</span>}
            </figcaption>
        </figure>
    </MovieElementWrapper>
)
