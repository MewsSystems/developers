import Box from "@mui/material/Box";
import { LoadMoreMovies } from "../../LoadMoreMovies";
import { MovieSearchLoading } from "./MovieSearchLoading";
import { SearchInput } from "./SearchInput";
import { SearchResults } from "./SearchResults";

export const MovieSearch = () => {
  return (
    <Box width="100%" p={4}>
      <Box display="flex" justifyContent="center">
        <SearchInput />
      </Box>
      <Box pt={2} pb={2}>
        <MovieSearchLoading />
        <SearchResults />
      </Box>
      <LoadMoreMovies />
    </Box>
  );
};
