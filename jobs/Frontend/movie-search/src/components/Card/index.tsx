import React from 'react'
import Image from 'next/image';
import { Movie } from '@/types/Movie';
import styled from 'styled-components';
import { dateFormatter } from '@/utils/dateFormater';

const CardContainer = styled.li`
    display: flex;;
    align-items: center;
    gap: 12px;

    padding: 10px;
`

type CardProps = { movie: Movie, handleClick: (id: number) => void }

export const Card = ({ movie, handleClick }: CardProps) => {
    return (
        <CardContainer key={movie.id} onClick={() => handleClick(movie.id)}>
            <Image
                width={90}
                height={120}
                src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                alt={movie.original_title}
            />
            <div>
                <h2>{movie.original_title}</h2>
                <p>{dateFormatter(movie.release_date)}</p>
                <p>{movie.overview}</p>
            </div>
        </CardContainer>
    )
}
