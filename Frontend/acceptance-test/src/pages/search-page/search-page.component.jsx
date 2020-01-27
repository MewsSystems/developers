import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";

//Utils
import { perfomMovieSearch } from "../../redux/search-movie/search-movie.utils";

//Abort Controller
import { abortController } from "../../utils/abort-controller";

import Directory from "../../components/directory/directory.component";
import { Spinner } from "../../components/spinner/spinner.component";

const SearchPage = ({ match }) => {
  const { isLoading, searchMovie, errors } = useSelector(
    state => state.searchMovie
  );
  const dispatch = useDispatch();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const searchMyMovie = () => dispatch(perfomMovieSearch(match));

  useEffect(() => {
    searchMyMovie();
    return () => {
      abortController.abort();
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [match.params.movieName]);

  return (
    <>
      {isLoading ? (
        <Spinner />
      ) : (
        <Directory errors={errors} movies={searchMovie} />
      )}
    </>
  );
};

export default SearchPage;
