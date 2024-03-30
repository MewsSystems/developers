import Box from "@mui/material/Box";
import { SearchInput } from "./SearchInput";
import { SearchResults } from "./SearchResults";
import { LoadMoreMovies } from "../../LoadMoreMovies";
import { MovieSearchLoading } from "./MovieSearchLoading";

export const MovieSearch = () => {
  return (
    <Box width="100%" p={4}>
      <Box display="flex" justifyContent="center">
        <SearchInput />
      </Box>
      <Box pt={2}>
        <MovieSearchLoading />
        <SearchResults />
      </Box>
      <Box p={4} display="flex" justifyContent="center">
        <LoadMoreMovies />
      </Box>
    </Box>
  );
};
