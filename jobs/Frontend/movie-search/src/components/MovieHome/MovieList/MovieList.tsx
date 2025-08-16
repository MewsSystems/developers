import MovieCard from "../MovieCard/MovieCard";
import MovieCardContainer from "../MovieCard/MovieCardContainer";
import NoMoviesComponent from "../NoMovies/NoMovies";

interface MovieListProps {
  searchterm: string;
  movies: {
    id: number;
    title: string;
    releaseDate: string;
    posterPath: string | null;
  }[];
}

export default function MovieList({ searchterm, movies }: MovieListProps) {
  if (!searchterm) {
    return (
      <NoMoviesComponent
        message="Search For Movies"
        additionalText="Use the search bar to find movies"
      />
    );
  }
  if (movies.length === 0) {
    return (
      <NoMoviesComponent
        message="No movies found"
        additionalText="Try searching for another movie"
      />
    );
  }
  return (
    <MovieCardContainer>
      {movies.map((movie, index) => (
        <MovieCard key={movie.id} index={index} movie={movie} />
      ))}
    </MovieCardContainer>
  );
}
