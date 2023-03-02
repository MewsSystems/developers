import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useSearchParams } from 'react-router-dom';
import { AppDispatch } from '../Store/store';
import { 
  selectMovieResults,
  selectSearchTerm,
  fetchMovies,
  selectNavigationDetails,
  selectLoadingStatus,
} from '../Store/moviesSlice';
import tmdbApi from '../API';
import {
  Header,
  Loading,
  MoviePreview,
  MoviePreviewGallery,
  NeutralLink,
  Pagination,
  StatusTitle,
} from '../Components';

const MovieListPage = () => {
  const movies = useSelector(selectMovieResults);
  const searchTerm = useSelector(selectSearchTerm);
  const navigationDetails = useSelector(selectNavigationDetails);
  const loadingStatus = useSelector(selectLoadingStatus);
  const dispatch = useDispatch<AppDispatch>();
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchTermInput, setSearchTermInput] = useState(searchParams.get('searchTerm') || undefined);

  const moviePreviews = movies.length > 0 && movies.map((movie) =>  
    <NeutralLink to={`/movies/${movie.id}`} key={movie.id}>
      <MoviePreview
        title={movie.title} 
        posterUrl={tmdbApi.getImageUrl(movie.poster_path)}
      />
    </NeutralLink>
  );

  const searchTermChangedHandler = (e: any) => {
    setSearchTermInput(e.target.value)
  };

  const inputCompleteHandler = (e: any) => {
    if(e.target.value !== searchParams.get('searchTerm')) {
      setSearchParams({
        searchTerm: e.target.value,
        page: '1',
      });
    }
  };

  const pageChanged = (page: string) => {
    const searchTerm = searchParams.get('searchTerm');
    if(searchTerm != null) {
      setSearchParams({
        searchTerm: searchTerm,
        page: page,
      });
      window.scrollTo(0, 0);
    }
  };

  useEffect(() => {
    const searchTerm = searchParams.get('searchTerm');
    const page = searchParams.get('page');

    if(searchTerm != null && page) {
      dispatch(fetchMovies({
        searchTerm: searchTerm,
        page: page,
      }));
    }
  }, [searchParams]);

  return <>
    <Header 
      display="search"
      searchTerm={searchTermInput}
      onSearchTermChanged={searchTermChangedHandler}
      onInputComplete={inputCompleteHandler}
    />
    <Loading status={loadingStatus}>
      {searchTerm !== '' && navigationDetails.totalResults === 0 
        ? <StatusTitle>No results</StatusTitle> : null}
      {searchTerm !== '' && navigationDetails.totalResults > 0 
        ? <MoviePreviewGallery>
            {moviePreviews}
          </MoviePreviewGallery> : null}
      {searchTerm !== '' && navigationDetails.totalPages > 1 
        ? <Pagination 
            page={navigationDetails.page}
            totalPages={navigationDetails.totalPages}
            onPageChanged={pageChanged}
          /> : null}
    </Loading>
  </>;
}

export default MovieListPage;
