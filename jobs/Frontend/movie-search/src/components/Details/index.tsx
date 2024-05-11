"use client";

import { MovieDetails } from "@/types/MovieDetail";
import { extractYear } from "@/utils/dateFormatter";
import { getScoreColor } from "@/utils/getScoreColor";
import { minutesToHoursMinutes } from "@/utils/minutesToHours";
import Image from "next/image";
import React from "react";
import styled from "styled-components";

const DetailsWrapper = styled.section<{ img?: string }>`
  padding: 3rem 6rem;
  display: flex;
  gap: 24px;

  background:linear-gradient(0deg, rgb(var(--background-start-rgb)), rgb(var(--background-end-rg))),${(props) => (props.img ? `url(${props.img})` : "white")};

  background-repeat: no-repeat;
  background-size: cover;
  height: 60vh;

  @media (max-width: 600px) {
      flex-direction: column;
      height: 100vh;
    }
`;

const ScoreWrapper = styled.div`
    margin: var(--space-md) 0;
    display: flex;
    align-items: center;
    gap: var(--space-md);
`

const Score = styled.p<{ color: string }>`
  background-color: ${(props) => props.color};
  width: 28px;
  height: 28px;
  border-radius: var(--border-radius--circle);
  display: flex;
  align-items: center;
  justify-content: center;
`;

const Year = styled.span`
  font-weight: 100;
`;

const TagLine = styled.p`
  font-style: italic;
  color: #5b675e;
  margin-bottom: var(--space-md);
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
