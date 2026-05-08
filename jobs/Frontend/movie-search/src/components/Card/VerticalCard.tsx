import React from 'react'
import Image from 'next/image';
import { Movie } from '@/types/Movie';
import styled from 'styled-components';
import { dateFormatter } from '@/utils/dateFormatter';

const CardContainer = styled.li`
    display: flex;
    flex-direction: column;
    gap: var(--space-md);
    padding: var(--space-md);
    cursor: pointer;
    text-align: left;
    &:hover {
        background-color: rgb(var(--hover-card));
        color: rgb(var(--foreground-rgb));
        border: 1px solid #ffffff;
        border-radius: 2px;
        scale: 1.1;
    }
`;

const StyledImage = styled(Image)`
    border-radius:var(--border-radius);
`;

const Date = styled.p`
    font-size: 12px;
    margin-top: var(--space-sm);
`;

type CardProps = { movie: Movie, handleClick: (id: number) => void }

export const VerticalCard = ({ movie, handleClick }: CardProps) => {
    return (
        <CardContainer key={movie.id} onClick={() => handleClick(movie.id)}>
            <StyledImage
                width={120}
                height={180}
                quality={75}
                loading='lazy'
                src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                alt={movie.original_title}
            />
            <div>
                <strong>{movie.original_title}</strong>
                <Date>{dateFormatter(movie.release_date)}</Date>
            </div>
        </CardContainer>
    )
}
