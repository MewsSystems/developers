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
  MovieDetailsBadge,
  MovieDetailsList,
  MovieOverview,
  MovieDetailsVote,
} from "./DetailPage.internal";
import { Image } from "../../components/Image/Image";
import {
  formatRuntime,
  getImageUrl,
  getTranslatedTitle,
  getYearFromDate,
} from "../../utils/movieHelpers";
import ImagePlaceholder from "../../assets/no-image-placeholder.jpg";
import Button from "../../components/Button/Button";
import { Clock, MoveLeft, ThumbsUp } from "lucide-react";
import { SpinnerCircle } from "../../components/Spinner/Spinner.internal";

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
          <MovieDetailsWrapper>
            <SpinnerCircle />
            <p>Loading movie details…</p>
          </MovieDetailsWrapper>
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
          <MovieDetailsWrapper>
            <p>
              Error loading movie details
              {error?.message ? `: ${error.message}` : "."}
            </p>
          </MovieDetailsWrapper>
        </MainContent>
      </>
    );
  }

  const movieTitle = getTranslatedTitle(
    data.original_language === "en",
    data.original_title,
    data.title
  );

  const releaseYear = data.release_date
    ? `(${getYearFromDate(data.release_date)})`
    : "(No release date)";

  return (
    <>
      <Header>
        <Button title="Go back" $isCircle onClick={() => navigate(-1)}>
          <MoveLeft size={12} color="#333" />
        </Button>
      </Header>
      <MainContent>
        <MovieDetailsWrapper>
          <ImageSection>
            <Image
              src={
                data.poster_path
                  ? getImageUrl(data.poster_path)
                  : ImagePlaceholder
              }
              alt={
                data.poster_path
                  ? `Poster of ${data.original_title}`
                  : `No image for ${data.original_title}`
              }
              $width="20rem"
              loading="lazy"
            />
          </ImageSection>
          <MovieDetailsSection>
            <MovieDetailsRow>
              <Heading>
                {movieTitle} {releaseYear}
              </Heading>
              {data.tagline ? (
                <MovieDetailsTagline>{data.tagline}</MovieDetailsTagline>
              ) : null}
              {data.genres.length > 0 ? (
                <MovieDetailsList>
                  {data.genres.map((genre) => (
                    <MovieDetailsBadge key={genre.id}>
                      <span>{genre.name}</span>
                    </MovieDetailsBadge>
                  ))}
                  <MovieDetailsBadge>
                    <Clock size={12} color="#333" />
                    <span>
                      {data.runtime ? formatRuntime(data.runtime) : null}
                    </span>
                  </MovieDetailsBadge>
                </MovieDetailsList>
              ) : null}
            </MovieDetailsRow>
            <MovieDetailsRow>
              <MovieDetailsVote>
                <ThumbsUp size={24} color="#333" />
                {data.vote_average.toFixed(1)}
              </MovieDetailsVote>
            </MovieDetailsRow>

            <MovieDetailsRow>
              <MovieDetailsTitle>Overview</MovieDetailsTitle>
              <MovieOverview>{data.overview}</MovieOverview>
            </MovieDetailsRow>
            <MovieDetailsRow>
              {data.production_companies.length > 0 ? (
                <>
                  <MovieDetailsTitle>Production</MovieDetailsTitle>
                  <MovieDetailsList>
                    {data.production_companies.map((company) => (
                      <MovieDetailsBadge $isInverted key={company.id}>
                        <span>{company.name}</span>
                      </MovieDetailsBadge>
                    ))}
                  </MovieDetailsList>
                </>
              ) : null}
            </MovieDetailsRow>
          </MovieDetailsSection>
        </MovieDetailsWrapper>
      </MainContent>
    </>
  );
};
