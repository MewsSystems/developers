import { LoaderFunctionArgs, useLoaderData } from 'react-router-dom';
import { MovieDetailsResponse } from '../../../types/api.ts';
import { QueryClient } from '@tanstack/react-query';
import { getMovieDetailsQueryOptions } from '../../api/movie-details.ts';
import { useImage } from '../../api/images.ts';
import {
  ImageContainer,
  MovieDetailsContainer,
  OverviewContainer,
  DetailsContainer,
  MovieDetailGrid,
  RowCenteredContainer,
} from './movie-details.styles.tsx';
import { Header, HeaderPlaceholder } from '../../../components/header';
import useMediaQuery from '../../../hooks/useMediaQuery.ts';
import Spinner from '../../../components/spinner';
import useDebounce from '../../../hooks/useDebouncer.ts';
import { useNavigate } from 'react-router-dom';
import { useTheme } from 'styled-components';
import { useContext } from 'react';
import { GlobalContext } from '../../Provider.tsx';

const movieDetailsLoader =
  (queryClient: QueryClient) =>
  async ({ params }: LoaderFunctionArgs) => {
    const movieId = params.movieId as string;

    const query = getMovieDetailsQueryOptions(movieId);

    return queryClient.getQueryData(query.queryKey) ?? (await queryClient.fetchQuery(query));
  };

const MovieDetailsRoute = () => {
  const { searchQuery, setSearchQuery } = useContext(GlobalContext);

  const loaderData = useLoaderData() as { data: MovieDetailsResponse };
  const movieDetails = loaderData.data;
  const backdropImage = useImage({ imagePath: movieDetails?.backdrop_path, imageWidth: 500 });

  const navigate = useNavigate();
  const handleBackButton = () => navigate('/movies');
  const handleClickLogo = () => navigate('/movies');

  const theme = useTheme();
  const isMobile = useMediaQuery(`(max-width: ${theme.breakpoints.tablet})`);
  const isLargeScreen = useMediaQuery(`(max-width: ${theme.breakpoints.largeDesktop})`);

  const debouncedFunction = useDebounce((newSearchQuery: string) => {
    setSearchQuery(newSearchQuery);
    navigate('/movies');
  }, 300);

  if (backdropImage.isLoading) {
    return <Spinner />;
  }

  if (backdropImage.isError) {
    return <div>Something went wrong</div>;
  }

  return (
    <>
      <Header
        searchQuery={searchQuery}
        handleUpdateSearchQuery={debouncedFunction}
        isMobile={isMobile}
        handleClickLogo={handleClickLogo}
        hasBackButton
        handlePressBackButton={handleBackButton}
      />
      <MovieDetailsContainer>
        <HeaderPlaceholder /> {/* Placeholder for the header */}
        <RowCenteredContainer>
          <h1>{movieDetails.title}</h1>
        </RowCenteredContainer>
        <MovieDetailGrid>
          <ImageContainer src={backdropImage.data} alt={movieDetails.title} />
          <DetailsContainer>
            {!!movieDetails.release_date && (
              <RowCenteredContainer>
                <h4>Release Date: </h4>
                <div>{movieDetails.release_date}</div>
              </RowCenteredContainer>
            )}
            {!!movieDetails.vote_average && (
              <RowCenteredContainer>
                <h4>Rating: </h4>
                <div>{movieDetails.vote_average}</div>
              </RowCenteredContainer>
            )}
            {!!movieDetails.genres && (
              <RowCenteredContainer>
                <h4>Genres: </h4>
                <div>{movieDetails.genres.map((genre) => genre.name).join(', ')}</div>
              </RowCenteredContainer>
            )}
            {!!movieDetails.runtime && (
              <RowCenteredContainer>
                <h4>Runtime: </h4>
                <div>{movieDetails.runtime} minutes</div>
              </RowCenteredContainer>
            )}
            {!!movieDetails.budget && (
              <RowCenteredContainer>
                <h4>Budget: </h4>
                <div>${movieDetails.budget}</div>
              </RowCenteredContainer>
            )}
            {!!movieDetails.revenue && (
              <RowCenteredContainer>
                <h4>Revenue: </h4>
                <div>${movieDetails.revenue}</div>
              </RowCenteredContainer>
            )}
            {isLargeScreen && (
              <>
                <RowCenteredContainer>
                  <h4>Production: </h4>
                  <div>{movieDetails.production_companies.map((company) => company.name).join(', ')}</div>
                </RowCenteredContainer>
                {!!movieDetails.tagline && (
                  <RowCenteredContainer>
                    <h4>Tagline: </h4>
                    <div>{movieDetails.tagline}</div>
                  </RowCenteredContainer>
                )}
                <RowCenteredContainer>
                  <div>{movieDetails.overview}</div>
                </RowCenteredContainer>
              </>
            )}
          </DetailsContainer>
        </MovieDetailGrid>
        {!isLargeScreen && (
          <RowCenteredContainer>
            <OverviewContainer>
              <h2>Overview</h2>
              <p>{movieDetails.overview}</p>
              {!!movieDetails.production_companies && (
                <RowCenteredContainer>
                  <h4>Production Companies: </h4>
                  <div>{movieDetails.production_companies.map((company) => company.name).join(', ')}</div>
                </RowCenteredContainer>
              )}
            </OverviewContainer>
          </RowCenteredContainer>
        )}
      </MovieDetailsContainer>
    </>
  );
};

export { MovieDetailsRoute, movieDetailsLoader };
