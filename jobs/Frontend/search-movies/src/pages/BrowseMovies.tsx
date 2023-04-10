import { useParams } from "react-router";
import styled from "styled-components";
import MovieList from "../components/MovieList";
import MovieListLoader from "../components/MovieListLoader";
import Pagination from "../components/Pagination";

const BrowseMoviesContainer = styled.div``;
const BrowseMovies = () => {
  const { pageId } = useParams();
  return (
    <>
      {pageId && (
        <BrowseMoviesContainer>
          <MovieList page={pageId}></MovieList>
          <MovieListLoader page={pageId}></MovieListLoader>
          <Pagination currentPage={pageId}></Pagination>
        </BrowseMoviesContainer>
      )}
    </>
  );
};

export default BrowseMovies;
