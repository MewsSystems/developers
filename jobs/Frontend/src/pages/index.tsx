"use client";
import Grid from "@mui/material/Grid";
import { ChangeEvent, useContext, useEffect, useState } from "react";
import { SearchField } from "@/components/form/searchField";
import { useStore } from "@/context";
import { SearchResults } from "@/components/searchResults/searchResults";
import { movieService } from "@/services/movieService";
import styles from "@/styles/Home.module.css";
import { Pagination } from "@mui/material";
import { Actions, State } from "@/context/types";

const MINIMUM_SEARCH_TERM_LENGTH = 3;

export default function Home() {
  const { searchTerm, searchResult, page, setSearchRequest, setSearchSuccess } =
    useStore();

  const search = async (num: number) => {
    const response = await movieService.search(searchTerm, num);
    setSearchSuccess(response);
  };

  const handleChange = (_: any, page: number) => search(page);

  useEffect(() => {
    if (searchTerm.length < MINIMUM_SEARCH_TERM_LENGTH) {
      return;
    }

    setSearchRequest();

    search(1);
  }, [searchTerm]);

  return (
    <Grid container justifyContent="center" spacing={2}>
      <Grid item xs={8} marginTop={9}>
        <SearchField />
      </Grid>
      <Grid item xs={8}>
        <SearchResults results={searchResult} />
      </Grid>
      {searchResult?.totalPage > 1 && (
        <Grid
          item
          xs={8}
          display="flex"
          justifyContent="center"
          alignItems="center"
          marginTop={6}
          marginBottom={6}
        >
          <Pagination
            count={searchResult.totalPage}
            page={searchResult.page}
            onChange={handleChange}
            variant="outlined"
            shape="rounded"
            showFirstButton
            showLastButton
          />
        </Grid>
      )}
    </Grid>
  );
}
