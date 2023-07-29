import { FC } from "react";
import { Link } from "react-router-dom";
import Styled from "styled-components";
import { MovieItem } from "../../models/movies.types";
import { getMovieDetailThunk } from "../../store/movie-thunks";
import { useAppDispatch } from "../../store/store";

const CardContents = Styled.div`
  max-width:300px;

  background-color: rgb(245, 197, 24);
  padding: 2%;
  flex-grow: 1;
  flex-basis: 16%;
  border: 1px solid  #b94f08;
  border-radius: 8px;
  display: flex;
  flex-flow: column;

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

const StyledViewDetail = Styled(Link)`
display: block;
width: 100px;
min-height: 28px;
background-color: #ec701d;
border: 2px solid  #b94f08;
border-radius:5px;
color:  #3f298d;
margin:auto;
padding: 4px;
line-height: 1.7;
font-weight:700;
text-decoration: none;
&:hover {
    background-color: #f0b085;; // <Thing> when hovered
}


`;

const Card: FC<{ movie: MovieItem }> = ({ movie }) => {
  const dispatch = useAppDispatch();

  const handleClickViewDetail = ()=>{
    dispatch(getMovieDetailThunk({ movieId:movie.id }));
  }
  const loadPoster = () => {
    //https://placehold.co/342x513?text=movie%20poster

    if (movie?.posterPath !== null) {
      return (
        <StyledImage
          src={`https://image.tmdb.org/t/p/w342${movie?.posterPath}`}
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
    <CardContents>
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
      <StyledViewDetail onClick={handleClickViewDetail} to={`/movies/${movie.id}`}>
        View Detail
      </StyledViewDetail>
    </CardContents>
  );
};

export default Card;
