import { keepPreviousData, useInfiniteQuery } from "@tanstack/react-query";
import { getMovies } from "../api";
import { useState } from "react";
import {
  AppBar,
  Button,
  InputAdornment,
  Snackbar,
  TextField,
  Toolbar,
  Typography,
} from "@mui/material";
import styled from "styled-components";
import { Search } from "@mui/icons-material";
import MovieList from "./MovieList";

function App() {
  const [searchTerm, setSearchTerm] = useState("");
  const isSearching = searchTerm !== "";

  const { data, fetchNextPage, status, isFetchingNextPage } = useInfiniteQuery({
    queryKey: ["movies", searchTerm],
    queryFn: (context) => getMovies(searchTerm, context),

    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.page + 1,

    enabled: isSearching,
    placeholderData: keepPreviousData,
  });

  const shownCount = data?.pages.flatMap((page) => page.results).length ?? 0;
  const totalCount = data?.pages[0].total_results ?? 0;

  return (
    <>
      <AppBar>
        <Toolbar>
          <Typography variant="h6">Movie Finder</Typography>
        </Toolbar>
      </AppBar>
      <CentredColumn>
        <Toolbar />
        <TextField
          value={searchTerm}
          onChange={(event) => setSearchTerm(event.target.value)}
          placeholder="Search all movies"
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <Search />
              </InputAdornment>
            ),
          }}
          sx={{ paddingBottom: "8px" }}
        />
        {isSearching && data !== undefined ? (
          <>
            <MovieList
              movies={data.pages.flatMap((page) => page.results)}
              shownCount={shownCount}
              totalCount={totalCount}
            />
            <Button
              sx={{
                visibility: shownCount === totalCount ? "hidden" : "visible",
              }}
              onClick={() => fetchNextPage()}
              disabled={isFetchingNextPage}
            >
              {isFetchingNextPage ? "Loading" : "Load More"}
            </Button>
          </>
        ) : (
          <Typography variant="body1" textAlign="center">
            Start a search to show movies
          </Typography>
        )}

        {status === "error" && (
          <Snackbar
            open={true}
            autoHideDuration={4000}
            message={"Something went wrong"}
          />
        )}
      </CentredColumn>
    </>
  );
}

const CentredColumn = styled.div`
  display: flex;
  flex-direction: column;
  align-items: stretch;

  width: 100%;
  height: 100vh;
  max-width: 600px;

  margin: auto;
  padding: 16px;
`;

export default App;
