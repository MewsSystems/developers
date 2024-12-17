import { Link } from "react-router";
import movieDefault from "@/assets/movie-default.png";
import {
  MovieCardContainer,
  MovieInfo,
  SubInfo,
  Poster,
  Title,
} from "@/components/MovieCard/MovieCardStyle";
import { Movie } from "@/types/movies";

const MovieCard: React.FC<Movie> = ({
  title,
  poster_path,
  release_date,
  vote_average,
  id,
}) => {
  const imgSrc = poster_path
    ? `https://image.tmdb.org/t/p/w500${poster_path}`
    : movieDefault;

  return (
    <Link to={`/movies/${id}`}>
      <MovieCardContainer>
        <Poster src={imgSrc} alt={title} />
        <MovieInfo>
          <Title>{title}</Title>
          <SubInfo>
            <span>{release_date}</span>
            <span>{vote_average?.toFixed(1)} ★</span>
          </SubInfo>
        </MovieInfo>
      </MovieCardContainer>
    </Link>
  );
};

export default MovieCard;
