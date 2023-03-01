import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AppDispatch } from '../Store/store';
import { useParams } from 'react-router-dom';
import { fetchMovieDetails, selectLoadingStatus, selectMovieDetails } from '../Store/moviesSlice';
import tmdbApi from '../API';
import { ImdbLink, UsdFormatter } from '../Helpers';
import {
  Header,
  LinkPill,
  Loading,
  MovieDetailTitle,
  PropertyList,
  PropertyTitle,
  PropertyValue
} from '../Components';

const MovieDetailsPage = () => {
  const { id } = useParams();
  const movieDetails = useSelector(selectMovieDetails);
  const loadingStatus = useSelector(selectLoadingStatus);
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    id && dispatch(fetchMovieDetails({
     movieId: id,
    }));
  }, [dispatch]);

  const movieMatchesWithId = id == movieDetails?.id;

  const headerTitle = () => {
    switch(loadingStatus) {
      case 'pending':
        return 'Loading';
      case 'fulfilled':
        return movieDetails?.title;
      case 'rejected':
        return 'There was an error';
    }
  }

  return (
    <>
      <Header 
        display="title_and_back"
        title="Movie details"
        subtitle={headerTitle()}
      />
      <Loading status={loadingStatus}>
        {(movieMatchesWithId && movieDetails) && <MovieDetailTitle 
          title={movieDetails.title}
          posterUrl={movieDetails.poster_path != null ? tmdbApi.getImageUrl(movieDetails.poster_path) : undefined}
          backdropUrl={movieDetails.backdrop_path != null ? tmdbApi.getImageUrl(movieDetails.backdrop_path) : undefined}
          tagline={movieDetails.tagline}
          overview={movieDetails.overview}
          genres={movieDetails.genres.map((g) => g.name)}
          runtime={movieDetails.runtime}
          releaseDate={movieDetails.release_date}
        />}
        <PropertyList padding={true}>
          {movieDetails?.production_companies && movieDetails?.production_companies.length > 0 ? <>
            <PropertyTitle>Production companies</PropertyTitle>
            <PropertyValue>{movieDetails?.production_companies.map((p) => p.name).join(', ')}</PropertyValue>
          </> : null}
          {movieDetails?.production_countries && movieDetails?.production_countries.length > 0 ? <>
            <PropertyTitle>Production countries</PropertyTitle>
            <PropertyValue>{movieDetails?.production_countries.map((p) => p.name).join(', ')}</PropertyValue>
          </> : null}
          {movieDetails?.spoken_languages && movieDetails?.spoken_languages.length > 0 ? <>
            <PropertyTitle>Spoken languages</PropertyTitle>
            <PropertyValue>{movieDetails?.spoken_languages.map((p) => p.english_name).join(', ')}</PropertyValue>
          </> : null}
          {movieDetails?.status ? <>
            <PropertyTitle>Status</PropertyTitle>
            <PropertyValue>{movieDetails?.status}</PropertyValue>
          </> : null}
          {movieDetails?.revenue ? <>
            <PropertyTitle>Revenue</PropertyTitle>
            <PropertyValue>{UsdFormatter.format(movieDetails?.revenue)}</PropertyValue>
          </> : null}
          {movieDetails?.budget ? <>
            <PropertyTitle>Budget</PropertyTitle>
            <PropertyValue>{UsdFormatter.format(movieDetails?.budget)}</PropertyValue>
          </> : null}
          {movieDetails?.imdb_id ? <>
            <PropertyTitle>Links</PropertyTitle>
            <PropertyValue>
              <LinkPill 
                href={ImdbLink(movieDetails.imdb_id)} 
                target="_blank" 
                title={`IMDB link for ${movieDetails.title}`}
              >
                IMDB
              </LinkPill>
            </PropertyValue>
          </> : null}
        </PropertyList>
      </Loading>
    </>
  );
}

export default MovieDetailsPage;
