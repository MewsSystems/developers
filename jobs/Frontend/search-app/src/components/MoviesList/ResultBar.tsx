import { useAppSelector } from "../../app/hooks";
import { Paginator } from "../Paginator/Paginator";
import { selectMoviesState } from "../../selectors/movies";
import { Bar, Total } from "./ResultBar.styled";

export const ResultBar = () => {
  const state = useAppSelector(selectMoviesState);
  return (
    <Bar>
      <Total>{state.results} Results</Total>
      {!!state.movies.length && <Paginator />}
    </Bar>
  );
};
