import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import styled from "styled-components";
import { AppDispatch, RootState } from "../../store";
import {
  fetchMovies,
  updateCurrentPage,
} from "../../store/reducers/BrowseMoviesReducer";
import { device } from "../../utils/theme";
import { Movie } from "../../utils/types";
import { shadow, shadowSm } from "../General";
import MovieImg from "../MovieImg";

const MovieListContainer = styled.div`
  margin-top: 30px;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
`;

const MovieContainer = styled.div`
  min-width: 9rem;
  margin: 10px;
  text-align: left;
  border: none;
  padding: 10px;
  border-radius: 10px;
  box-shadow: ${shadowSm};

  @media ${device.tablet} {
    min-width: 11rem;
  }
  &:hover {
    box-shadow: ${shadow};
  }
`;

interface MovieListProps {
  page: string;
}

const MovieList = (props: MovieListProps) => {
  const { page } = props;
  const browseMovies = useSelector((state: RootState) => state.browseMovies);
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    (async function () {
      if (page && !browseMovies.pages.hasOwnProperty(page)) {
        await dispatch(fetchMovies(page));
        await dispatch(updateCurrentPage(page));
      }
    })();
  }, [page]);

  return (
    <>
      <MovieListContainer>
        {Array.isArray(browseMovies.pages[page]) &&
          browseMovies.pages[page].map((movie: Movie) => (
            <MovieContainer>
              <Link
                to={`/${page}/movie/${movie.id}`}
                style={{ textDecoration: "none" }}
              >
                <MovieImg
                  movieName={movie.original_title}
                  banner={movie.poster_path}
                ></MovieImg>
              </Link>
            </MovieContainer>
          ))}
      </MovieListContainer>
    </>
  );
};

export default MovieList;
