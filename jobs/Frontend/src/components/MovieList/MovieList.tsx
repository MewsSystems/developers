import React, { useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { RootState } from "@redux/store";
import { useNavigate } from "react-router-dom";
import StarRatings from "react-star-ratings";
import {
  ListWrapper,
  MovieListItem,
  MovieListWrapper,
  RatingWrapper,
  ShowMoreButton,
} from "./MovieList.styled";
import ErrorText from "@components/ErrorText/ErrorText";
import { loadMoreMovies } from "@services/movieService";
import { FETCH_MOVIES } from "@redux/actions/movieActions";

// Define a functional component called MovieList
const MovieList: React.FC = () => {
  // Use useSelector hook to select a slice of state from the Redux store
  const selectedMovies = useSelector((state: RootState) => state.movie);
  // Use useNavigate hook to get access to the navigation object
  const navigate = useNavigate();
  // Use useDispatch hook to get access to the dispatch function of the Redux store
  const dispatch = useDispatch();
  // Use useState hook to create a state variable called currentPage, initialized to 1
  const [currentPage, setCurrentPage] = useState(1);

  // Define an async function called handleShowMore, which is called when the "Show More" button is clicked
  const handleShowMore = async () => {
    // Calculate the next page number
    const nextPage = currentPage + 1;
    // Call the loadMoreMovies function from the movieService module to load more movies from the API
    const response = await loadMoreMovies(selectedMovies.movies.query, nextPage);
    // Dispatch an action to update the movies state in the Redux store with the newly loaded movies
    dispatch({
      type: FETCH_MOVIES,
      payload: {
        movies: [...selectedMovies.movies.movies, ...response],
        query: selectedMovies.movies.query,
        page: nextPage,
        status: "success",
      },
    });
    // Update the currentPage state variable to the next page number
    setCurrentPage(nextPage);
  };

  // Define a function called handleMovieClick, which is called when a movie item is clicked
  const handleMovieClick = (id: number) => {
    // Navigate to the movie detail page for the clicked movie
    navigate(`/movie/${id}`);
  };

  // Return the JSX for the MovieList component
  return (
    <ListWrapper>
      {selectedMovies.movies.status !== "idle" && (
        <MovieListWrapper>
          {selectedMovies.movies.movies.map((movie: any) => (
            <MovieListItem
              key={movie.id}
              onClick={() => handleMovieClick(movie.id)}
            >
              <img
                src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                alt={`${movie.title} poster`}
              />
              <h3>{movie.title}</h3>
              <RatingWrapper>
                <StarRatings
                  rating={movie.vote_average / 2} // Divide by 2 because TMDB uses a 10-point scale, while react-star-ratings uses a 5-point scale
                  numberOfStars={5}
                  starRatedColor="#f1c40f"
                  starDimension="20px"
                  starSpacing="2px"
                />
              </RatingWrapper>
              <p>Release Date: {movie.release_date}</p>
            </MovieListItem>
          ))}
          {selectedMovies.movies.status === "error" && (
            <ErrorText message="Error: Movie not found!" />
          )}
        </MovieListWrapper>
      )}
      <ShowMoreButton>
        {selectedMovies.movies.movies.length >= 20 &&
          <button onClick={handleShowMore}>Show More</button>
        }
      </ShowMoreButton>
    </ListWrapper>
  );
};

export default MovieList;
