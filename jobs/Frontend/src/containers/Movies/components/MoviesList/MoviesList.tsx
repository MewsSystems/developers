import { useSelector } from "react-redux";
import { Link } from "react-router-dom";

import { RootState } from "../../../../store";
import { Loader } from "../../../../components/Loader";
import { handleImageLoadingError } from "../../../../utils";
import { getPosterUrlFromPath } from "../../utils";
import { MovieListStyle } from "./styled";

const { LoaderWrapper, Container, Item, ItemImage, MovieTitle, MovieYear } =
  MovieListStyle;

const selector = (s: RootState) => ({
  movies: s.movies,
  fetchState: s.fetchState,
  loading: s.fetchState === "loading",
});

export const MoviesList = () => {
  const { movies, loading, fetchState } = useSelector(selector);

  if (loading) {
    return (
      <LoaderWrapper>
        <Loader />
      </LoaderWrapper>
    );
  }

  if (fetchState === "idle") {
    return null;
  }

  if (movies.length === 0) {
    return <p>There are no movies matching your search!</p>;
  }

  return (
    <>
      <h3>Results:</h3>
      <Container>
        {movies.map((movie) => (
          <Item as="li" key={movie.id}>
            <Link to={`/movies/${movie.id}`}>
              <ItemImage
                src={getPosterUrlFromPath(movie.poster_path)}
                onError={handleImageLoadingError}
              />
              <div>
                <MovieTitle>
                  {movie.title}{" "}
                  <MovieYear>
                    ({new Date(movie.release_date).getFullYear()})
                  </MovieYear>
                </MovieTitle>
              </div>
            </Link>
          </Item>
        ))}
      </Container>

      {fetchState === "loading-more" && (
        <LoaderWrapper>
          <Loader />
        </LoaderWrapper>
      )}
    </>
  );
};
