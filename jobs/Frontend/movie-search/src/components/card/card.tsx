import { FC } from "react";
import { Link } from "react-router-dom";
import Styled from "styled-components";
import { MovieItem } from "../../models/movies.types";
import { getMovieDetailThunk } from "../../store/movie-thunks";
import { useAppDispatch, useAppSelector } from "../../store/store";

const CardContents = Styled(Link)`
  max-width:300px;
  text-decoration: none;
  background-color: rgb(245, 197, 24);
  padding: 2%;
  flex-grow: 1;
  flex-basis: 16%;
  border: 1px solid  #b94f08;
  border-radius: 8px;
  display: flex;
  flex-flow: column;
  cursor:pointer;
  &:hover {
    background-color:#f1eb97;
} 

`;
const Title = Styled.h3`
color: #3f298d;
text-transform: capitalize;

`;

const StyledImage = Styled.img`
width:200px;
margin:0 auto;

`;
const StyledDesctiption = Styled.div`
margin:12px auto;
font-size:1em;
color:  #3f298d;
.description-text{
    padding-top:4px
    text-transform: capitalize;
}
`;

const Card: FC<{ movie: MovieItem }> = ({ movie }) => {
  const dispatch = useAppDispatch();
  const baseImageUrl = useAppSelector((state) => state.conguration.imagesconfiguration?.secureBaseUrl);

  const handleClickViewDetail = () => {
    dispatch(getMovieDetailThunk({ movieId: movie.id }));
  };
  const loadPoster = () => {
    //https://placehold.co/342x513?text=movie%20poster

    if (movie?.posterPath !== null) {
      return (
        <StyledImage
          src={`${baseImageUrl}w342${movie?.posterPath}`}
          alt={`${movie} poster`}
        ></StyledImage>
      );
    }

    return (
      <StyledImage
        src='https://placehold.co/342x513?text=Poster%20Not%20Found'
        alt={`${movie} poster`}
      ></StyledImage>
    );
  };
  return (
    <CardContents onClick={handleClickViewDetail} to={`/movies/${movie.id}`}>
      <Title>{movie?.title}</Title>
      {loadPoster()}
      <StyledDesctiption>
        <div className='description-text'>
          Original Language: {movie?.orginalLanguage}
        </div>
        <div className='description-text'>
          Orginal Title: {movie?.originalTitle}
        </div>
        <div className='description-text'>
          Release Date: {movie?.releaseDate.toLocaleString()}
        </div>
      </StyledDesctiption>
    </CardContents>
  );
};

export default Card;
