import { useEffect, useState } from "react";
import { Movie } from "tmdb-ts";
import styled from "styled-components";
import { MovieCard, Typography, WithMovieIdProps } from ".";
import { IMG_BASE_PATH, tmdbClient } from "@/pages/Search";

const CardsWrapper = styled.div`
  display: flex;
  gap: 24px;

  padding: 0 0 16px 0;
  overflow-x: auto;
`;

export function SimilarMovies({ movieId }: WithMovieIdProps) {
  const [data, setData] = useState<Movie[]>();

  useEffect(() => {
    tmdbClient.movies.similar(movieId).then(res => setData(res.results));
  }, [movieId]);

  // TODO: refactor MovieCard to accept only movie, add genre
  return (
    <CardsWrapper>
      {data?.length ? (
        data.map(movie => (
          <MovieCard
            imgPath={movie.poster_path ? IMG_BASE_PATH + movie.poster_path : null}
            key={movie.id}
            id={movie.id}
            title={movie.title}
            description={movie.overview}
            releaseDate={movie.release_date}
            rating={movie.vote_average / 2}
            genres={movie.genre_ids.map(() => "Unknown")}
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
