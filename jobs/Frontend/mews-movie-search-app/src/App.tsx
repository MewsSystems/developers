import React from "react";
import styled from "styled-components";

type MovieListResponse = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

type Movie = {
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

function App() {
  const [movies, setMovies] = React.useState<MovieListResponse | null>(null);

  const getList = async () => {
    const res = await fetch(
      `https://api.themoviedb.org/3/discover/movie?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }&include_adult=false&include_video=false&language=en-US&page=1&sort_by=popularity.desc`
    );
    const data = await res.json();
    setMovies(data);
  };

  React.useEffect(() => {
    getList();
  }, []);

  return (
    <MainLayout>
      <HeaderNav>
        <Title>Mews Movie Search App</Title>
        <SearchBarContainer>
          <Input
            type="text"
            onChange={(e) => console.log(e)}
            placeholder="Search a movie"
          />
        </SearchBarContainer>
      </HeaderNav>
      <MovieContainer>
        {movies?.results &&
          movies?.results.map((movie: Movie) => {
            return (
              <MovieItem key={movie.id}>
                <MoviePoster>
                  <MoviePosterImage
                    src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
                    alt={movie.title}
                  />
                </MoviePoster>
              </MovieItem>
            );
          })}
      </MovieContainer>
    </MainLayout>
  );
}

export default App;

const Input = styled.input`
  padding: 0.5rem;
  margin: 1rem;
  border: none;
  border-radius: 5px;
  width: 200px;
  font-size: 1rem;
`;

const Title = styled.h3`
  color: white;
  padding: 0.5rem 1rem;
`;
const HeaderNav = styled.nav`
  background: #1a1a1a;
  display: flex;
  justify-content: flex-start;
  align-items: center;
`;
const SearchBarContainer = styled.div`
  background: white;
`;

const MainLayout = styled.main`
  width: 100vw;
  height: 100vh;
  display: flex;
  background-color: #77b0aa;
  flex-direction: column;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
`;

const MovieContainer = styled.ul`
  display: flex;
  flex-wrap: wrap;
  overflow-y: auto;
`;

const MovieItem = styled.li`
  display: flex;
  flex-wrap: wrap;
  overflow-y: auto;
`;
const MoviePosterImage = styled.img`
  width: 100%;
  height: 100%;
  border-width: 8px;
  border-radius: 1.5rem;
`;
const MoviePoster = styled.div`
  padding: 1rem;
`;
