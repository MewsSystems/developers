import { Movie } from "../../../types/movies";
import { PaginatedResponse } from "../../../types/requests";
import { VStack, HStack } from "../../Stacks";
import {
  PaginationNumbering,
  Table,
  TableButton,
  THead,
  Tr,
} from "../../shared/Table";
import { MovieListItem } from "./MovieListItem";

interface IMovieList {
  page: PaginatedResponse<Movie>;
  setPage: React.Dispatch<React.SetStateAction<number>>;
  setSelected: React.Dispatch<React.SetStateAction<number>>;
}

export function MovieList(props: IMovieList) {
  return (
    <VStack>
      <VStack>
        <Table>
          <THead>
            <Tr>
              <td>ID</td>
              <td>Title</td>
              <td>Release Date</td>
              <td>Rating</td>
              <td>Actions</td>
            </Tr>
          </THead>
          <tbody>
            {props.page.results.map((m) => (
              <MovieListItem
                movie={m}
                key={m.id}
                setSelected={props.setSelected}
              />
            ))}
          </tbody>
        </Table>
      </VStack>
      <HStack $justifyContent="space-between">
        <TableButton
          disabled={props.page.page == 1}
          onClick={() => props.setPage(props.page.page - 1)}
        >
          {"<"}
        </TableButton>
        <PaginationNumbering>{`Page ${props.page.page}/${props.page.total_pages} of ${props.page.total_results} results`}</PaginationNumbering>
        <TableButton
          disabled={props.page.page == props.page.total_pages}
          onClick={() => props.setPage(props.page.page + 1)}
        >
          {">"}
        </TableButton>
      </HStack>
    </VStack>
  );
}
