import { Box, CircularProgress, Typography } from "@mui/material";
import ErrorMessage from "./common/ErrorMessage.tsx";
import MovieTile from "./MovieTile.tsx";
import { Movie } from "../hooks/movies/useSearchMovie.ts";

interface SearchMovieContentProps {
  isLoading?: boolean;
  isError?: boolean;
  results?: Movie[];
  handleClear: () => void;
}

const SearchMovieContent = ({
  isLoading,
  isError,
  results,
  handleClear,
}: SearchMovieContentProps) => {
  if (isLoading) {
    return <CircularProgress size={100} sx={{ my: 10 }} />;
  }

  if (isError) {
    return <ErrorMessage handleClear={handleClear} />;
  }

  if (results?.length === 0) {
    return (
      <Typography variant="body1" sx={{ my: 5 }}>
        Sorry we could not find anything for you.
      </Typography>
    );
  }

  return (
    !!results?.length && (
      <>
        <Box
          sx={{
            my: 5,
            display: "grid",
            gridTemplateColumns: {
              xs: "repeat(1, minmax(0, 1fr))",
              lg: "repeat(2, minmax(0, 1fr))",
            },
            gap: 2,
            justifyContent: "center",
          }}
        >
          {results.map((props) => (
            <MovieTile key={props.id} {...props} />
          ))}
        </Box>
      </>
    )
  );
};

export default SearchMovieContent;
