import { useCallback, useContext, useEffect, useState } from "react";
import { Link, useParams } from "react-router";
import ErrorContext from "@/providers/ErrorContext";
import { Movie } from "@/types/movies";
import fetchBase from "@/utils/fetchBase";
import {
  DetailsContainer,
  HeroSection,
  ContentSection,
  BackButton,
  Title,
  MetaInfo,
  Description,
} from "@/pages/MovieDetails/MovieDetailsStyle";
import SkeletonMovieDetails from "@/components/SkeletonMovieDetails/SkeletonMovieDetails";
import NoMovieFound from "@/components/NoMovieFound/NoMovieFound";
import movieBackdrop from "@/assets/movie-backdrop.jpg";

const MovieDetails: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [movie, setMovie] = useState<Movie | null>(null);
  const { setMessage } = useContext(ErrorContext);
  const { movieId } = useParams<{ movieId: string }>();
  const backdropSrc = movie?.backdrop_path
    ? `https://image.tmdb.org/t/p/original/${movie.backdrop_path}`
    : movieBackdrop;

  const fetchMovie = useCallback(async () => {
    setLoading(true);
    try {
      const response = await fetchBase(`/movie/${movieId}`);
      const movie = (await response.json()) as Movie;
      setMovie(movie);
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
    } catch (_err) {
      setMessage("Error occured during fetching movie details");
    } finally {
      setLoading(false);
    }
  }, [movieId, setMessage]);

  useEffect(() => {
    fetchMovie();
  }, [fetchMovie]);

  if (loading) return <SkeletonMovieDetails />;

  if (!movie || movie.success === false) return <NoMovieFound />;

  return (
    <DetailsContainer>
      <HeroSection backdrop={backdropSrc}></HeroSection>
      <ContentSection>
        <Link to="/">
          <BackButton>← Back to Search</BackButton>
        </Link>
        <Title>{movie.title}</Title>
        <MetaInfo>
          <span>{movie.release_date}</span>
          <span>{movie.vote_average} ★</span>
        </MetaInfo>
        <Description>{movie.overview}</Description>
      </ContentSection>
    </DetailsContainer>
  );
};

export default MovieDetails;
