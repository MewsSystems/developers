import { FC } from "react";
import Styled, { styled } from "styled-components";
import Spinner from "../../components/spinner/spinner";
import { useAppSelector } from "../../store/store";
const Title = Styled.h2`
font-size: 1.2em;
text-align: center;
color: #3f298d;

`;

// Create a Wrapper component that'll render a <section> tag with some styles
const MainSection = Styled.section`
padding: 2em;
background-color: #f3f3f3;
`;
const StyledMessage = Styled.section`
  color:#3f298d;
  padding: 2em;
  text-align:center;
  height: calc(100vh - 349px);
`;

const DetailContent = Styled.section`
  padding: 2em;
  border-radius:5px;
`;
const StyledImage = Styled.img`
width:400px;
margin:0 auto;
`;

const DetailData = styled.section`
  margin-top: 2vh;
  background-color: #ffffff;
  color: #3f298d;
  border-radius: 5px;
  padding: 2em;
  border: 1px solid#b94f08;
`;

const MovieDetail: FC<{}> = () => {
  const status = useAppSelector((state) => state.movies.statusMoviesPage);
  const movie = useAppSelector((state) => state.movies.selectedDetail);
  const baseImageUrl = useAppSelector((state) => state.conguration.imagesconfiguration?.secureBaseUrl);

  const loadPoster = () => {
    //https://placehold.co/342x513?text=movie%20poster

    if (movie?.posterPath !== null) {
      return (
        <StyledImage
          src={`${baseImageUrl}w780${movie?.posterPath}`}
          alt={`${movie} poster`}
        ></StyledImage>
      );
    }

    return (
      <StyledImage
        src='https://placehold.co/780x513?text=Poster%20Not%20Found'
        alt={`${movie} poster`}
      ></StyledImage>
    );
  };

  const loadContent = () => {
    switch (status) {
      case "init":
        return <StyledMessage></StyledMessage>;

      case "loading":
        return <Spinner></Spinner>;
      case "empty":
        return <StyledMessage>movie details not found</StyledMessage>;

      default:
        return (
          <DetailContent>
            <Title>{movie?.title}</Title>
            {loadPoster()}
            <DetailData>
              <div>Original Title: {movie?.originalTitle}</div>
              <div>Original Language: {movie?.originalLanguage}</div>
              <div>Genres: {movie?.genres}</div>
              <br></br>
              <div>
                <b>Overview</b>
              </div>
              <div>{movie?.overview}</div>
              <br></br>
              <div>Budget: ${movie?.budget.toLocaleString()}</div>
              <div>Revenue: ${movie?.revenue.toLocaleString()}</div>
              <div>Release Date: {movie?.releaseDate}</div>
            </DetailData>
          </DetailContent>
        );
    }
  };

  return <MainSection>{loadContent()}</MainSection>;
};

export default MovieDetail;
