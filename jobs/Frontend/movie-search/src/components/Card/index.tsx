import React from 'react'
import Image from 'next/image';
import { Movie } from '@/types/Movie';
import styled from 'styled-components';
import { dateFormatter } from '@/utils/dateFormatter';

const CardContainer = styled.li`
    display: flex;;
    align-items: center;
    gap: 12px;

    padding: 10px;
    cursor: pointer;
    &:hover {
        background-color: #ffffff70;
        color: black;
        border: 1px solid #ffffff;
        border-radius: 14px;
        scale: 1.1;
    }
`;

const Date = styled.p`
    font-style: italic;
    color: gray;
`

const Overview = styled.p`
    margin-top: 12px;
`



type CardProps = { movie: Movie, handleClick: (id: number) => void }

export const Card = ({ movie, handleClick }: CardProps) => {
    return (
        <CardContainer data-testid="card_movie" key={movie.id} onClick={() => handleClick(movie.id)}>
            <>
                <Image
                    width={180}
                    height={100}
                    style={{ width: "120px", height: "auto" }}
                    quality={75}
                    loading='lazy'
                    src={movie.poster_path ? `https://image.tmdb.org/t/p/w500${movie.poster_path}` : `https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/9556d16312333.5691dd2255721.jpg`}
                    alt={movie.original_title}
                />
            </>

            <div>
                <h2>{movie.original_title}</h2>
                <Date>{dateFormatter(movie.release_date)}</Date>
                <Overview>{movie.overview}</Overview>
            </div>
        </CardContainer>
    )
}
