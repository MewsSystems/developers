import React from "react";

//Styles
import "./movie-details.styles.scss";

//Components
import Errors from "../errors/errors.component";

const MovieDetails = ({ movieDetails, errors }) => {
  return (
    <>
      <ul className="movie-details__list">
        <li>
          <Errors errors={errors} />
        </li>
        <li>
          <strong className="movie-details__list-title">Title: </strong>
          <span className="movie-details__list-details">
            {movieDetails.title}
          </span>
        </li>
        <li>
          <strong className="movie-details__list-title">
            original title:{" "}
          </strong>
          <span className="movie-details__list-details">
            {movieDetails.original_title}
          </span>
        </li>
        <li>
          <strong className="movie-details__list-title">overview: </strong>
          <p className="movie-details__list-details">{movieDetails.overview}</p>
        </li>
        <li>
          <strong className="movie-details__list-title">release date: </strong>
          <span className="movie-details__list-details">
            {movieDetails.release_date}
          </span>
        </li>
        <li>
          <strong className="movie-details__list-title">status: </strong>
          <span className="movie-details__list-details">
            {movieDetails.status}
          </span>
        </li>

        <li>
          <strong className="movie-details__list-title">vote: </strong>
          <span className="movie-details__list-details">
            {movieDetails.vote_average}/10
          </span>
        </li>
        <li>
          <strong className="movie-details__list-title">
            number of votes:{" "}
          </strong>
          <span className="movie-details__list-details">
            {movieDetails.vote_count}
          </span>
        </li>
        <li>
          <strong className="movie-details__list-title">adult: </strong>
          <span className="movie-details__list-details">
            {movieDetails.adult ? "Yes" : "No"}
          </span>
        </li>
      </ul>
    </>
  );
};

export default MovieDetails;
