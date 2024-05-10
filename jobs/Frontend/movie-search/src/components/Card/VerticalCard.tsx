import React from 'react'
import Image from 'next/image';
import { Movie } from '@/types/Movie';
import styled from 'styled-components';
import { dateFormatter } from '@/utils/dateFormatter';

const CardContainer = styled.li`
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 12px;
    padding: 18px;
    cursor: pointer;
    &:hover {
        background-color: #ffffff70;
        color: black;
        border: 1px solid #ffffff;
        border-radius: 14px;
        scale: 1.1;
    }
`;

const StyledImage = styled(Image)`
    border-radius: 12px;
`

type CardProps = { movie: Movie, handleClick: (id: number) => void }

export const VerticalCard = ({ movie, handleClick }: CardProps) => {
    return (
        <CardContainer key={movie.id} onClick={() => handleClick(movie.id)}>
            <StyledImage
                width={120}
                height={180}
                src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                alt={movie.original_title}
            />
            <div>
                <strong>{movie.original_title}</strong>
                <p>{dateFormatter(movie.release_date)}</p>
            </div>
        </CardContainer>
    )
}
