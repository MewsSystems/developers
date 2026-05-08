import { getBackdropPath, getPlaceholderBackdropPath } from "../../api";
import { useMovieDetailsQuery } from "../../hooks/useMovieDetailsQuery";
import Spinner from "../../components/Spinner";
import BackButton from "../../components/BackButton";
import Error from "../../components/Error";
import {
  MovieDataContainer,
  MovieDetailContainer,
  MovieImage,
  MovieImageWrapper,
  Header,
  GenresContainer,
  Pill,
  HeaderInfo,
} from "./MovieDetails.styles";

type Genre = {
  id: number;
  name: string;
};

function MovieDetails() {
  const { movie, isLoading, isError, error, movieRuntime } =
    useMovieDetailsQuery();

  return (
    <>
      {isLoading ? (
        <Spinner />
      ) : isError ? (
        <Error errorMessage={error?.message} />
      ) : (
        <MovieDetailContainer>
          <BackButton />
          <MovieImageWrapper>
            <MovieImage
              src={
                movie.backdrop_path
                  ? getBackdropPath(movie.backdrop_path)
                  : getPlaceholderBackdropPath(movie.title)
              }
              alt={movie.title}
            />
          </MovieImageWrapper>
          <MovieDataContainer>
            <Header>{movie.title}</Header>
            <HeaderInfo>
              <span>{new Date(movie.release_date).getFullYear()}</span>
              <span>{`${movieRuntime?.hours}h ${movieRuntime?.minutes}m`}</span>
            </HeaderInfo>
            <GenresContainer>
              {movie?.genres?.map((genre: Genre) => (
                <Pill key={genre.id}>{genre.name}</Pill>
              ))}
            </GenresContainer>

            <div>{movie.overview}</div>
          </MovieDataContainer>
        </MovieDetailContainer>
      )}
    </>
  );
}

export default MovieDetails;
