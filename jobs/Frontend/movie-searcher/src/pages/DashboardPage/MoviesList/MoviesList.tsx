import { useSelector } from "react-redux";
import { useMemo } from "react";
import { Card } from "antd";
import { selectMoviesListState } from "../../../store/moviesSearch/movieSearchReducer";
import { normalizeMoviesList } from "../normalize.util";
import { TMDB_IMAGES_URL } from "../../../constants";
import { ImageStyled, LinkStyled, MetaStyled, Wrapper } from "./MoviesList.styled";

const MoviesList = () => {
  const { isLoading, moviesList } = useSelector(selectMoviesListState);
  const normalizedMoviesList = useMemo(() => normalizeMoviesList(moviesList), [moviesList]);

  if (isLoading && !normalizedMoviesList.length) {
    return <div>Loading...</div>;
  }

  if (!isLoading && !normalizedMoviesList.length) {
    return <div>No posts found. Please try another request.</div>;
  }

  return (
    <Wrapper>
      {normalizedMoviesList.map(({ id, title, imgUrl }) => (
        <LinkStyled to={`movie-info/${id}`} key={id}>
          <Card hoverable>
            <ImageStyled alt={title} src={`${TMDB_IMAGES_URL}/${imgUrl}`} />
            <MetaStyled title={title} />
          </Card>
        </LinkStyled>
      ))}
    </Wrapper>
  );
};

export { MoviesList };
