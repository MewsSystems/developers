import { TableBody, TableRow, TableCell, Table } from "@/components/ui/table";

export type MovieRow = {
  id: number;
  originalTitle?: string;
  originalLanguage?: string;
  releaseDate?: string;
};

export type MovieListProps = {
  movies: Array<MovieRow>;
  onTableRowClick: (movieId: number) => void;
};

/**
 * Renders a table of movies with 3 columns. Click on a row executes a custom action.
 */
export function MovieList({ movies, onTableRowClick }: MovieListProps) {
  return (
    <Table>
      <TableBody>
        {movies.map((movie) => (
          <TableRow key={movie.id} onClick={() => onTableRowClick(movie.id)}>
            <TableCell>{movie.releaseDate}</TableCell>
            <TableCell>{movie.originalLanguage}</TableCell>
            <TableCell className="font-medium">{movie.originalTitle}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
