import { useAppDispatch, useAppSelector } from "../../app/hooks";

import { useSearchParams } from "react-router-dom";
import { selectMoviesState } from "../../selectors/movies";
import { fetchMovies } from "../../actions/fetchMovies";
import { Button } from "./PaginatorButtons.styled";

type Props = {
  buttonNumbers: number[];
};

export const PaginatorButtons = ({ buttonNumbers }: Props) => {
  const state = useAppSelector(selectMoviesState);
  const dispatch = useAppDispatch();
  const currentPage = state.activePage;
  const totalPages = state.totalPages;

  const [searchParams, setSearchParams] = useSearchParams();

  const handlePageChange = (page: number) => {
    searchParams.set("page", page.toString());
    setSearchParams(searchParams);
    dispatch(fetchMovies(state.searchKey, page));
  };

  return (
    <>
      {currentPage > 1 && (
        <Button onClick={() => handlePageChange(currentPage - 1)}>Prev</Button>
      )}
      {buttonNumbers.map(number => (
        <Button
          key={number}
          onClick={() => handlePageChange(number)}
          className={number === currentPage ? "active" : ""}>
          {number}
        </Button>
      ))}
      {currentPage < totalPages && (
        <Button onClick={() => handlePageChange(currentPage + 1)}>Next</Button>
      )}
    </>
  );
};
