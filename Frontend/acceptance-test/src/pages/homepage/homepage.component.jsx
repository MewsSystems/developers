import React, { useEffect, Fragment } from "react";
import { useSelector, useDispatch } from "react-redux";

//Utils
import { fetchTopRatedMovies } from "../../redux/directory/directory.utils";

//Abort Controller
import { abortController } from "../../utils/abort-controller";

//Styles
import "./homepage.styles.scss";

//Components
import Carousel from "../../components/carousel/carousel.components";
import Directory from "../../components/directory/directory.component";
import { Spinner } from "../../components/spinner/spinner.component";

function HomePage() {
  const { isLoading, errors, movies } = useSelector(state => state.directory);

  const dispatch = useDispatch();
  const getMovies = () => dispatch(fetchTopRatedMovies());

  useEffect(() => {
    getMovies();
    return function cleanup() {
      abortController.abort();
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className="App">
      {isLoading ? (
        <Spinner />
      ) : (
        <Fragment>
          <Carousel />
          <Directory movies={movies} errors={errors} />
        </Fragment>
      )}
    </div>
  );
}

export default HomePage;
