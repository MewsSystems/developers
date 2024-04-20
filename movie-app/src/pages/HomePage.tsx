import { Pagination, TextField, Typography } from "@mui/material";
import { useSearchMovie } from "../hooks/movies/useSearchMovie.ts";
import SearchMovieContent from "../components/search/SearchMovieContent.tsx";

const HomePage = () => {
  const {
    page,
    totalPages,
    currentSearch,
    searchRef,
    results,
    handleSearch,
    handlePageChange,
    handleClear,
    isLoading,
    isError,
  } = useSearchMovie();

  return (
    <div>
      <Typography variant="h1">Lights, Camera, Search!</Typography>
      <Typography variant="body1" sx={{ my: 5 }}>
        Hunting for the perfect movie has never been easier! Simply enter
        keywords into the search bar and let our app work its magic. Start your
        journey now and unlock a universe of cinematic wonders
      </Typography>
      <TextField
        hiddenLabel
        fullWidth
        aria-label="Search movie"
        inputRef={searchRef}
        onChange={(event) => handleSearch(event.target.value)}
        helperText={
          currentSearch.length < 3
            ? "You need to enter at least 3 characters to start search"
            : ""
        }
      />
      <>
        <SearchMovieContent
          isLoading={isLoading}
          isError={isError}
          results={results}
          handleClear={handleClear}
        />

        {!!results?.length && (
          <Pagination
            color="primary"
            count={totalPages}
            onChange={handlePageChange}
            page={page}
          />
        )}
      </>
    </div>
  );
};

export default HomePage;
