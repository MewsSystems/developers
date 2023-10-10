import { useDispatch, useSelector } from "react-redux";
import { MovieDto } from "../../dtos/movieDto";
import "./MovieItem.css";
import { InitialState } from "../../models/initialState";
import { DispatchAction } from "../../models/dispatchAction";
import {
  SET_SELECTED_MOVIE,
  SET_SELECTED_MOVIE_GENRES
} from "../../models/actions";

const MovieItem: React.FC = () => {
  const dispatch = useDispatch();
  const selectedMovie = useSelector(
    (state: InitialState) => state.selectedMovie
  );
  const selectedMovieGenres = useSelector(
    (state: InitialState) => state.selectedMovieGenres
  );
  function onClose() {
    dispatch<DispatchAction>({
      type: SET_SELECTED_MOVIE_GENRES,
      value: []
    });
    dispatch<DispatchAction>({ type: SET_SELECTED_MOVIE, value: null });
  }

  return (
    <div className="movieItem">
      <div className="">
        <div className="mb-3 d-flex justify-content-between align-items-center">
          <h3 className="">{selectedMovie.title}</h3>
          <img id="closeButton" onClick={() => onClose()} src="./close.svg" />
        </div>
        <div id="movieItemContent" className="d-flex">
          <div className="me-4">
            <img
              id="movieItemImage"
              src={`https://image.tmdb.org/t/p/w500/${selectedMovie.poster_path}`}
              alt={selectedMovie.title}
            />
          </div>
          <div className="">
            <h4>Overview</h4>
            <p>{selectedMovie.overview}</p>
            <div className="d-flex">
              <div className="">
                <p className="mb-1">
                  <b>Release Date</b>
                </p>
                <i>{selectedMovie.release_date || "-"}</i>
              </div>
              <div className="ms-3">
                <p className="mb-1">
                  <b>Genres</b>
                </p>
                <i>{selectedMovieGenres.join(", ")}</i>
              </div>
              <div className="ms-3">
                <p className="mb-1">
                  <b>Vote</b>
                </p>
                <i>{selectedMovie.vote_average.toPrecision(2)}</i>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default MovieItem;
