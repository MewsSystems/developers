import { useEffect, useState } from "react";
import { Input } from "../../components/Form/Input";
import { MainContent } from "../../components/Layout/MainContent";
import { Header } from "../../components/Header/Header";
import { Grid } from "../../components/Grid/Grid";
import { getPopularMovies } from "../../api/requests";
import { type Movie } from "../../api/types";
import { MovieCard } from "./MovieCard";
import { Heading } from "../../components/Typography/Heading";

export const HomePage = () => {
  const [movieData, setMovieData] = useState<Movie[]>([]);
  useEffect(() => {
    (async () => {
      try {
        const data = await getPopularMovies();
        setMovieData(data.results);
      } catch (e) {
        console.error(e);
      }
    })();
  }, []);
  return (
    <>
      <Header>
        <Input name="searchField" />
      </Header>
      <MainContent>
        <Heading>List of movies</Heading>
        {movieData ? (
          <Grid
            items={movieData}
            keyExtractor={(movie) => movie.id}
            renderItem={(movie) => <MovieCard movieData={movie} />}
          />
        ) : (
          <p>No data present</p>
        )}
      </MainContent>
    </>
  );
};
