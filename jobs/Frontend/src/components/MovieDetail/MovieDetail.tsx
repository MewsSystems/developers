import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { RootState } from "@redux/store";
import { fetchMovieDetail } from "@redux/actions/movieActions";
import { useParams, useNavigate } from "react-router-dom";
import StarRatings from "react-star-ratings";
import {
  BackButton,
  DetailSection,
  MovieDetailWrapper,
  MoviePoster,
} from "./MovieDetails.styled";
import ErrorText from "@components/ErrorText/ErrorText";

// Define the type of the movie object returned from the API
interface Movie {
  title: string;
  release_date: string;
  runtime: number;
  vote_average: number | undefined;
  overview: string;
  poster_path: string;
}
const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  // Get the movie detail from the Redux store
  const movie: Movie = useSelector(
    (state: RootState) => state.movie.movieDetail
  );

  const dispatch = useDispatch();
  const navigate = useNavigate();

  // Fetch the movie detail from the API when the component mounts
  useEffect(() => {
    dispatch(fetchMovieDetail(id));
  }, [id, dispatch]);

  // Navigate back to the previous page when the BackButton is clicked
  const handleBackClick = () => {
    navigate(-1);
  };

  // Render the component
  const moviePosterUrl = `https://image.tmdb.org/t/p/w500${movie.poster_path}`;
  return (
    <MovieDetailWrapper>
      {movie ? (
        <>
          <MoviePoster src={moviePosterUrl} alt={`${movie.title} poster`} />
          <DetailSection>
            <h2>{movie.title}</h2>
            <p>Release Date: {movie.release_date}</p>
            <p>Runtime: {movie.runtime} minutes</p>
            <StarRatings
              rating={(movie.vote_average || 0) / 2}
              numberOfStars={5}
              starRatedColor="#f1c40f"
              starDimension="20px"
              starSpacing="2px"
            />
            <p>{movie.overview}</p>
            <BackButton onClick={handleBackClick}>Back to search</BackButton>
          </DetailSection>
        </>
      ) : (
        <ErrorText message="Error: Something went wrong!" />
      )}
    </MovieDetailWrapper>
  );

};

export default MovieDetail;
