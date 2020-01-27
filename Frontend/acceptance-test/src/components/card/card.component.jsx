import React from "react";
import { Link } from "react-router-dom";
import { withRouter } from "react-router-dom";

//Styles
import { Cards, MovieTitle } from "./card.styles";

const Card = ({ movies }) => {
  return (
    <>
      {movies.map(movie => (
        <Link to={`/movie-details/${movie.id}`} key={movie.id}>
          <Cards
            imageBg={`https://image.tmdb.org/t/p/w185/${movie.poster_path}`}
            alt={movie.title.toUpperCase()}
          >
            <MovieTitle>{movie.title}</MovieTitle>
          </Cards>
        </Link>
      ))}
    </>
  );
};

export default withRouter(Card);
