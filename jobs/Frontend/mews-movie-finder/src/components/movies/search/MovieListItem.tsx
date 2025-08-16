import { Movie } from "../../../types/movies";
import { Td, Tr } from "../../shared/Table";

interface IMovieListItem {
  movie: Movie;
  setSelected: React.Dispatch<React.SetStateAction<number>>;
}

export function MovieListItem(props: IMovieListItem) {
  return (
    <Tr>
      <Td $textAlign="left">{props.movie.id}</Td>
      <Td $textAlign="left">{props.movie.title}</Td>
      <Td>{props.movie.release_date}</Td>
      <Td>{`${props.movie.vote_average}/10`}</Td>
      <Td>
        <button onClick={() => props.setSelected(props.movie.id)}>
          Select
        </button>
      </Td>
    </Tr>
  );
}
