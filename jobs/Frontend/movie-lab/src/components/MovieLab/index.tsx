'use client';

import { useEffect, useState } from "react";
import MovieSearch from "@/src/components/MovieSearch";
import MovieList from "@/src/components/MovieList";
import { AppContainer, Header, MovieName } from "@/src/components/MovieLab/style";
import { MovieType } from "@/src/types/movie";

export default function MovieLab({ data, totalPages }: { data: MovieType[], totalPages: number }) {
  const [randomMovie, setRandomMovie] = useState('');
  const [key, setKey] = useState(0);

  const getRandomMovie = (movies: MovieType[]): string => {
    const randomIndex = Math.floor(Math.random() * movies.length);
    return movies[randomIndex].title;
  };

  useEffect(() => {
    const updateRandomMovie = () => {
      setRandomMovie(getRandomMovie(data));
      setKey(prevKey => prevKey + 1);
    };

    updateRandomMovie();

    const interval = setInterval(updateRandomMovie, 5000);

    return () => clearInterval(interval);
  }, [data]);

  return (
    <AppContainer>
      <Header>Looking for? <MovieName key={key}>{randomMovie}</MovieName></Header>
      <MovieSearch />
      <MovieList data={data} totalPages={totalPages} />
    </AppContainer>
  );
}
