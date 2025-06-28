import "./App.css";
import { LayoutWrapper } from "./components/Layout/LayoutWrapper";
import { Input } from "./components/Form/Input";
import { MainContent } from "./components/Layout/MainContent";
import { Header } from "./components/Header/Header";
import { Footer } from "./components/Footer/Footer";
import { Grid } from "./components/Grid/Grid";
import { GridCard } from "./components/Grid/GridCard";
import { useEffect, useState } from "react";
import { getPopularMovies } from "./api/requests";
import { type Movie } from "./api/types";

interface MovieCardProps {
  movieData: Movie;
}

const MovieCard = (props: MovieCardProps) => {
  return (
    <GridCard item={props.movieData} onClick={() => console.log("clicked")}>
      {props.movieData.original_title}
    </GridCard>
  );
};

function App() {
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
    <LayoutWrapper>
      <Header>
        <Input name="searchField" />
      </Header>
      <MainContent>
        <Grid
          items={movieData}
          keyExtractor={(movie) => movie.id}
          renderItem={(movie) => <MovieCard movieData={movie} />}
        />
      </MainContent>
      <Footer>Created by Petar Zayakov</Footer>
    </LayoutWrapper>
  );
}

export default App;
