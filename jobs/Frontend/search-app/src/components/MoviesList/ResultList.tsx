import { useAppSelector } from "../../app/hooks";

import { ResultBar } from "./ResultBar";
import { ResultListItem } from "./ResultListItem";
import { Spinner } from "../Spinner/Spinner";
import { selectMoviesState } from "../../selectors/movies";
import { List } from "./ResultList.styled";

export const ResultList = () => {
  const state = useAppSelector(selectMoviesState);

  return (
    <>
      <ResultBar />
      {state.isBusy ? (
        <Spinner />
      ) : (
        <List>
          {state.movies.map(movie => (
            <ResultListItem movie={movie} key={movie.id}></ResultListItem>
          ))}
        </List>
      )}
    </>
  );
};
