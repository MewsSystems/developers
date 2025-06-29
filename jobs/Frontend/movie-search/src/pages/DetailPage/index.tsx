import { useNavigate, useParams } from "react-router";
import { Header } from "../../components/Header/Header";
import { MainContent } from "../../components/Layout/MainContent";
import { Heading } from "../../components/Typography/Heading";
import { useMovieDetails } from "../../hooks/useMovieDetailsQuery";
import {
  ImageSection,
  MovieDetailsRow,
  MovieDetailsSection,
  MovieDetailsTagline,
  MovieDetailsTitle,
  MovieDetailsWrapper,
  MovieGenresBadge,
  MovieGenresList,
  MovieOverview,
} from "./DetailsPage.internal";
import { Image } from "../../components/Image/Image";
import {
  formatRuntime,
  getImageUrl,
  getTranslatedTitle,
  getYearFromDate,
} from "../HomePage/MovieCard.helpers";

export const DetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const movieId = Number(id);

  const { data, isLoading, isError, error } = useMovieDetails(movieId);

  if (isNaN(movieId) || movieId <= 0) {
    return (
      <>
        <Header>
          <button onClick={() => navigate(-1)}>back</button>
        </Header>
        <MainContent>
          <p>Invalid movie ID.</p>
        </MainContent>
      </>
    );
  }

  if (isLoading) {
    return (
      <>
        <Header>
          <button onClick={() => navigate(-1)}>← Back</button>
        </Header>
        <MainContent>
          <p>Loading movie details…</p>
        </MainContent>
      </>
    );
  }

  if (isError || !data) {
    return (
      <>
        <Header>
          <button onClick={() => navigate(-1)}>← Back</button>
        </Header>
        <MainContent>
          <p>
            Error loading movie details
            {error?.message ? `: ${error.message}` : "."}
          </p>
        </MainContent>
      </>
    );
  }

  const movieTitle = getTranslatedTitle(
    data.original_language === "en",
    data.original_title,
    data.title
  );

  return (
    <>
      <Header>
        <button onClick={() => navigate(-1)}>back</button>
      </Header>
      <MainContent>
        <MovieDetailsWrapper>
          <ImageSection>
            <Image src={getImageUrl(data.poster_path)} />
          </ImageSection>
          <MovieDetailsSection>
            <MovieDetailsRow>
              <Heading>
                {movieTitle} {`(${getYearFromDate(data.release_date)})`}
              </Heading>
              {data.tagline ? (
                <MovieDetailsTagline>{data.tagline}</MovieDetailsTagline>
              ) : null}
              <MovieGenresList>
                {data.genres &&
                  data.genres.map((genre) => (
                    <MovieGenresBadge key={genre.id}>
                      {genre.name}
                    </MovieGenresBadge>
                  ))}
                {formatRuntime(data.runtime)}
              </MovieGenresList>
            </MovieDetailsRow>
            <MovieDetailsRow>
              <MovieDetailsTitle>Vote</MovieDetailsTitle>
              {data.vote_average.toFixed(1)}
            </MovieDetailsRow>
            <MovieDetailsRow>
              <MovieDetailsTitle>Runtime</MovieDetailsTitle>
            </MovieDetailsRow>
            <MovieDetailsRow>
              <MovieDetailsTitle>Overview</MovieDetailsTitle>
              <MovieOverview>{data.overview}</MovieOverview>
            </MovieDetailsRow>
            <MovieDetailsRow>
              <MovieDetailsTitle>Production</MovieDetailsTitle>
              <MovieGenresList>
                {data.production_companies &&
                  data.production_companies.map((company) => (
                    <>
                      <MovieGenresBadge>{company.name}</MovieGenresBadge>
                    </>
                  ))}
              </MovieGenresList>
            </MovieDetailsRow>
          </MovieDetailsSection>
        </MovieDetailsWrapper>
      </MainContent>
    </>
  );
};
