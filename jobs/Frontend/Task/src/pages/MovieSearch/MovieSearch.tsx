import React, { useState } from "react";
import Box from "@mui/material/Box";
import { EmptyState } from "../../components/EmptyState/EmptyState";
import { LoadingState } from "../../components/LoadingState/LoadingState";
import { Pagination } from "../../components/Pagination/Pagination";
import { useMovieSearch } from "./hooks/useMovieSearch";
import { MovieGrid } from "./components/MovieGrid/MovieGrid";
import { MovieTable } from "./components/MovieTable/MovieTable";
import { SearchBar } from "./components/SearchBar/SearchBar";

type View = "table" | "grid";

export const MovieSearch = () => {
  const [ view, setView ] = useState<View>("table");

  const { 
    movieList, 
    page,
    pagesTotal,
    isLoading,
    isError,
    query,
    setPage, 
    setQuery,
  } = useMovieSearch();

  const getContent = () => {
    if (isLoading) {
      return <LoadingState />;
    }

    if (isError) {  
      return (
        <EmptyState 
          title="Something went wrong"
          description="Please try again later"
        />
      );
    }

    if (!query) {
      return (
        <EmptyState
          title="Are you looking for a movie?"
          description="Start by searching for a movie title"
        />
      );
    }

    if (movieList.length === 0) {
      return (
        <EmptyState
          title="No movies titles found..."
          description="Try changing your search input"
        />
      );
    }

    return (
      view === "table"
        ? <MovieTable data={movieList} />
        : <MovieGrid data={movieList} />
    );
  };

  return (
    <>
      <SearchBar 
        onSearch={(search) => {
          setQuery(search);
          setPage(1);
        }}
        onToggle={(viewType) => setView(viewType)}
        view={view}
      />

      <Box marginTop={4}>
        <Pagination
          count={pagesTotal} 
          onChange={(page) => setPage(page)}
          page={page} 
        />
      </Box>

      <Box paddingY={4} paddingX={4}>
        {getContent()}
      </Box>
    </>
  );
};
