import { useNavigate, useParams } from "react-router";
import { Header } from "../../components/Header/Header";
import { MainContent } from "../../components/Layout/MainContent";
import { useMovieDetails } from "../../hooks/useMovieDetailsQuery";
import {
  MovieDetailsWrapper,
  MovieDetailsSection,
} from "./DetailPage.internal";
import { DetailPageError } from "./DetailPageError";
import { MovieHeader } from "./MovieHeader";
import { MoviePoster } from "./MoviePoster";
import { MovieContent } from "./MovieContent";
import Button from "../../components/Button/Button";
import { MoveLeft } from "lucide-react";

export const DetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const movieId = Number(id);

  const { data, isLoading, isError, error } = useMovieDetails(movieId);

  const handleBackClick = () => {
    navigate(-1);
  };

  if (isNaN(movieId) || movieId <= 0) {
    return <DetailPageError type="invalid" />;
  }

  if (isLoading) {
    return <DetailPageError type="loading" />;
  }

  if (isError || !data) {
    return <DetailPageError type="error" errorMessage={error?.message} />;
  }

  return (
    <>
      <Header>
        <Button title="Go back" $isCircle onClick={handleBackClick}>
          <MoveLeft size={12} color="#333" />
        </Button>
      </Header>
      <MainContent>
        <MovieDetailsWrapper>
          <MoviePoster movie={data} />
          <MovieDetailsSection>
            <MovieHeader movie={data} />
            <MovieContent movie={data} />
          </MovieDetailsSection>
        </MovieDetailsWrapper>
      </MainContent>
    </>
  );
};
