"use client";

import { MovieDetails } from "@/types/MovieDetail";
import { extractYear } from "@/utils/dateFormatter";
import { minutesToHoursMinutes } from "@/utils/minutesToHours";
import Image from "next/image";
import React from "react";
import styled from "styled-components";

const DetailsWrapper = styled.section<{ img?: string }>`
  padding: 3rem 6rem;
  display: flex;
  gap: 24px;
  background-image: linear-gradient(black, black),
    ${(props) => (props.img ? `url(${props.img})` : "white")};
  background-repeat: no-repeat;
  background-size: cover;
  background-blend-mode: saturation;
  backdrop-filter: blur(10px);
  height: 60vh;
`;

const ScoreWrapper = styled.div`
    margin: 12px 0;

    display: flex;
    align-items: center;
    gap: 12px;
`

const Score = styled.p<{ color: string }>`
  background-color: ${(props) => props.color};
  width: 28px;
  height: 28px;
  border-radius: 50%;

  display: flex;
  align-items: center;
  justify-content: center;
`;

const Year = styled.span`
  font-weight: 100;
`;

const TagLine = styled.p`
  font-style: italic;
  color: #bab3b3;
  margin-bottom: 12px;
`;

export default function Details({ movie }: { movie: MovieDetails }) {
    const [hours, remainingMinutes] = minutesToHoursMinutes(movie.runtime);

    return (
        <DetailsWrapper
            img={`https://image.tmdb.org/t/p/w500${movie.backdrop_path}`}
        >
            <Image
                quality={75}
                width={190}
                height={290}
                src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                alt={movie.original_title ?? "Movie Image"}
            />
            <div>
                <h2>
                    <strong>{movie.title}</strong>{" "}
                    <Year>({extractYear(movie.release_date)})</Year>
                </h2>
                <p>
                    {movie.genres.map((genre, index) => {
                        if (index === movie.genres.length - 1) {
                            return genre.name;
                        } else {
                            return `${genre.name},`;
                        }
                    })}{" "}
                    - {hours ? `${hours}hr ${remainingMinutes}m` : null}
                </p>
                <ScoreWrapper>
                    <Score color={getScoreColor(Math.floor(movie.vote_average))}>
                        {Math.floor(movie.vote_average)}
                    </Score>{" "}
                    User score
                </ScoreWrapper>
                <TagLine>{movie.tagline}</TagLine>
                <h2>Overview</h2>
                <p>{movie.overview}</p>
            </div>
        </DetailsWrapper>
    );
}
