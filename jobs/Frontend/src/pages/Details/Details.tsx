import { useEffect, useMemo, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useSelector } from "react-redux";
import { Helmet } from "react-helmet";

import { getMovieDetail } from "../../api";
import { MovieDetails } from "../../containers/Movies";
import { RootState } from "../../store";
import { Loader } from "../../components/Loader";
import { styled } from "styled-components";
import { getPosterUrlFromPath } from "../../containers/Movies/utils";
import { handleImageLoadingError } from "../../utils";
import { Icon } from "../../components/Icon";
import { Card } from "../../components/Card";

const Main = styled(Card)`
  display: flex;
  align-items: flex-start;

  gap: 16px;
  padding: 16px;
`;

const Img = styled.img`
  width: 220px;
  aspect-ratio: 2/3;
  object-fit: cover;
`;

const LoaderContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
`;

const selector = (movieId?: string) => (s: RootState) => ({
  basicMovieInfo: s.movies.find((m) => String(m.id) === movieId),
});

export const Details = () => {
  const { movieId } = useParams<{ movieId: string }>();

  const movieSelector = useMemo(() => selector(movieId), [movieId]);

  const { basicMovieInfo } = useSelector(movieSelector);

  const [movieDetails, setMovieDetails] = useState<MovieDetails>();

  useEffect(() => {
    setMovieDetails(undefined);

    if (movieId) {
      getMovieDetail(movieId).then(setMovieDetails).catch(console.error);
    }
  }, [movieId]);

  if (!movieDetails && !basicMovieInfo) {
    return <Loader />;
  }

  const title = movieDetails?.title || basicMovieInfo?.title || "";

  return (
    <>
      <Helmet>
        <title>{title} - Your favorite movie!</title>
      </Helmet>
      <header>
        <Link to="/" aria-label="Back to search">
          <Icon.ArrowLeft />
        </Link>
        <h1>{title}</h1>
      </header>
      <Main as="main">
        <Img
          src={getPosterUrlFromPath(
            movieDetails?.poster_path || basicMovieInfo?.poster_path || ""
          )}
          alt={`${title} Banner`}
          onError={handleImageLoadingError}
        />
        <div style={{ flex: 1 }}>
          {movieDetails ? (
            <>
              {movieDetails.tagline && <h2>{movieDetails.tagline}</h2>}
              <p>{movieDetails.overview}</p>
            </>
          ) : (
            <LoaderContainer>
              <Loader color="lightgray" />
            </LoaderContainer>
          )}
        </div>
      </Main>
    </>
  );
};
