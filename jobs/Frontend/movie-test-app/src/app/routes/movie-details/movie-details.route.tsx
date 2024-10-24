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

const movieDetailsLoader =
  (queryClient: QueryClient) =>
  async ({ params }: LoaderFunctionArgs) => {
    const movieId = params.movieId as string;

    const query = getMovieDetailsQueryOptions(movieId);

    return queryClient.getQueryData(query.queryKey) ?? (await queryClient.fetchQuery(query));
  };

const MovieDetailsRoute = () => {
  const loaderData = useLoaderData() as { data: MovieDetailsResponse };
  const movieDetails = loaderData.data;
  const backdropImage = useImage({ imagePath: movieDetails?.backdrop_path, imageWidth: 500 });

  const isUltraLargeScreen = useMediaQuery('(min-width: 1400px)');

  return (
    <>
      <Header />
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
            {isUltraLargeScreen && (
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
        {!isUltraLargeScreen && (
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
