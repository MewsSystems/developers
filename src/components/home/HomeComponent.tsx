// External Libraries
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

// Components
import MovieListItem from "../movieListItem/MovieListItem";
import MovieItem from "../movieItem/MovieItem";
import Alert from "../alert/Alert";

// Models and DTOs
import { SearchMoviesResponseDto } from "../../dtos/searchMoviesResponseDto";
import { InitialState } from "../../models/initialState";
import { SET_GENRES } from "../../models/actions";
import { DispatchAction } from "../../models/dispatchAction";

// Styles
import "./HomeComponent.css";

function HomeComponent() {
  const dispatch = useDispatch();
  const selectedMovie = useSelector(
    (state: InitialState) => state.selectedMovie
  );

  const [response, setResponse] = useState<SearchMoviesResponseDto>();
  const [currentQuery, setCurrentQuery] = useState<string>("");
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [alertMessage, setAlertMessage] = useState<string>(null);

  const BASE_URL = "https://api.themoviedb.org/3";
  const API_KEY = process.env.REACT_APP_API_KEY;

  async function fetchMovies(query: string, page: number) {
    setIsLoading(true);
    const url = `${BASE_URL}/search/movie?include_adult=false&query=${query}&page=${page}&api_key=${API_KEY}`;
    fetch(url)
      .then((res) => res.json())
      .then((res) => {
        if (!res?.results) return res;
        setResponse((prev) => {
          res.results =
            res.results.length > 0
              ? [
                  ...(prev?.results || []),
                  ...res.results.filter((x: any) => x.poster_path)
                ]
              : [];
          return res;
        });
      })
      .catch((err) => {
        setAlertMessage("An error has occurred while fetching movies.");
        console.error(err);
        setTimeout(() => setAlertMessage(null), 5000);
      })
      .finally(() => setIsLoading(false));
  }

  async function fetchGenres() {
    const url = `${BASE_URL}/genre/movie/list?api_key=${API_KEY}`;
    return await fetch(url)
      .then((res) => res.json())
      .then((res) =>
        dispatch<DispatchAction>({ type: SET_GENRES, value: res.genres })
      )
      .catch((err) => {
        console.error(err);
        setTimeout(() => setAlertMessage(null), 5000);
      });
  }

  useEffect(() => {
    fetchGenres();
  }, []);

  function onChangeInput(value: string) {
    setResponse(null);
    setCurrentQuery(value);
    setCurrentPage(1);
    fetchMovies(value, 1);
  }

  return (
    <main
      className={`d-flex flex-column align-items-center ${
        selectedMovie ? "disable-scroll" : ""
      }`}>
      <div className="mt-5">
        <h1>Search a movie</h1>
      </div>
      <div className="mt-5 d-flex align-items-center justify-content-center input">
        <div>
          <img src="./search.svg" alt="" />
          <input
            onChange={(e) => onChangeInput(e.target.value)}
            placeholder="Titanic"
            type="text"
          />
        </div>
        <div className={`spinner ${isLoading ? "" : "invisible"}`}></div>
      </div>
      <section className="d-flex flex-column align-items-center">
        <div id="movieListContainer">
          {response?.results?.map((movie, index) => (
            <MovieListItem key={`${movie.id}-${index}`} movie={movie} />
          ))}
        </div>
        <button
          onClick={() => {
            const nextPage = currentPage + 1;
            setCurrentPage(nextPage);
            fetchMovies(currentQuery, nextPage);
          }}
          className={`button ${
            response && response.total_pages > currentPage && !isLoading
              ? ""
              : "hidden"
          }`}>
          Load More
        </button>
        <div
          className={`spinner mb-5 ${
            !isLoading || currentPage === 1 ? "hidden" : ""
          }`}></div>
      </section>
      <h3
        className={`${
          response && response.results.length < 1 && currentQuery
            ? ""
            : "hidden"
        }`}>
        No results.
      </h3>
      {selectedMovie ? <MovieItem /> : ""}
      <Alert message={alertMessage} />
    </main>
  );
}

export default HomeComponent;
