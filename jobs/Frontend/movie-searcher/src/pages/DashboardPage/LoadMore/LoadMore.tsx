import { useDispatch, useSelector } from "react-redux";
import { AnyAction } from "redux";
import { ThunkDispatch } from "redux-thunk";
import { Button } from "antd";
import {
  loadMoreMoviesList,
  selectMoviesListState,
} from "../../../store/moviesSearch/movieSearchReducer";
import { Wrapper } from "./LoadMore.styled";

const LoadMore = () => {
  const dispatch: ThunkDispatch<unknown, unknown, AnyAction> = useDispatch();
  const { inputValue, currentPage } = useSelector(selectMoviesListState);

  const onLoadMore = () => {
    dispatch(loadMoreMoviesList({ value: inputValue || "", page: currentPage + 1 }));
  };

  return (
    <Wrapper>
      <Button onClick={onLoadMore}>Load more</Button>
    </Wrapper>
  );
};

export { LoadMore };
