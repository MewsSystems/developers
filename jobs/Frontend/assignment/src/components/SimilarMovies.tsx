import { useEffect, useState } from "react";
import { Movie } from "tmdb-ts";
import styled from "styled-components";
import { MovieCard, Typography, WithMovieIdProps } from ".";
import { tmdbClient } from "@/tmdbClient";

interface SimilarMoviesProps {
  getGenreNameById(id: number): string;
}

const CardsWrapper = styled.div`
  display: flex;
  gap: 24px;

  padding: 0 0 16px 0;
  overflow-x: auto;
`;

export function SimilarMovies({ movieId, getGenreNameById }: WithMovieIdProps<SimilarMoviesProps>) {
  const [data, setData] = useState<Movie[]>();

  useEffect(() => {
    tmdbClient.movies.similar(movieId).then(res => setData(res.results));
  }, [movieId]);

  return (
    <CardsWrapper>
      {data?.length ? (
        data.map(movie => (
          <MovieCard
            key={movie.id}
            movie={movie}
            genres={movie.genre_ids.map(id => getGenreNameById(id))}
          />
        ))
      ) : (
        <Typography variant="titleMedium" color="secondary">
          No similar movies found
        </Typography>
      )}
    </CardsWrapper>
  );
}
