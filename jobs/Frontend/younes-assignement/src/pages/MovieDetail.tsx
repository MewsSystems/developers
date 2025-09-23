import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import Layout from "../components/Layout";
import Loader from "../components/Loader";
import { fetchMovieDetail } from "../api/tmdb";
import type { MovieDetailType } from "../types/movie";
import PosterImage from "../components/PosterImage";
import Placeholder from "../components/Placeholder";
import { MovieTextContainer } from "../styles/styles";

const MovieDetail = () => {
  const { id } = useParams<{ id: string }>();
  const [movie, setMovie] = useState<MovieDetailType | null>(null);

  useEffect(() => {
    const getMovie = async () => {
      if (!id) return;
      const data = await fetchMovieDetail(id);
      setMovie(data);
    };
    getMovie();
  }, [id]);

  if (!movie)
    return (
      <Layout>
        <Loader />
      </Layout>
    );

  return (
    <Layout>
      <h1>{movie.title}</h1>
      {movie.poster_path ? (
        <PosterImage
          posterPath={movie.poster_path}
          alt={movie.title}
          maxWidth="400px"
        />
      ) : (
        <Placeholder width="400px" height="600px" text="No Image Available" />
      )}
      <MovieTextContainer>
        <p>
          <strong>Overview:</strong>
        </p>
        <p>{movie.overview}</p>
        <p>
          <strong>Release Date:</strong> {movie.release_date}
        </p>
        <p>
          <strong>Rating:</strong> {`${movie.vote_average.toFixed(1)}`}
        </p>
      </MovieTextContainer>
      <Link to="/">Back to search</Link>
    </Layout>
  );
};

export default MovieDetail;
