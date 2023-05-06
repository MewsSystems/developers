import { CardStyled, ImageStyled, MetaStyled, Wrapper } from "./MoviesList.styled";
import { useSelector } from "react-redux";
import { selectMoviesListState } from "../../../store/moviesSearch/movieSearchReducer";
import { normalizeMoviesList } from "../normalize.util";
import { TMDB_IMAGES_URL } from "../../../constants";

const MoviesList = () => {
  const { isLoading, moviesFound } = useSelector(selectMoviesListState);

  console.log("moviesFound", moviesFound);

  const normalizedMoviesList = normalizeMoviesList(moviesFound.results);

  if (isLoading && !normalizedMoviesList.length) {
    return <div>Loading...</div>;
  }

  if (!isLoading && !normalizedMoviesList.length) {
    return <div>No posts found. Please try another request.</div>;
  }

  return (
    <Wrapper>
      {normalizedMoviesList.map(({ id, title, imgUrl }) => (
        <CardStyled key={id} hoverable>
          <ImageStyled alt={title} src={`${TMDB_IMAGES_URL}/${imgUrl}`} />
          <MetaStyled title={title} />
        </CardStyled>
      ))}
    </Wrapper>
  );
};

export { MoviesList };
