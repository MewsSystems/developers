"use client";
import Grid from "@mui/material/Grid";
import { useEffect, useMemo } from "react";
import { SearchField } from "@/components/form/searchField";
import { useStore } from "@/context";
import { SearchResults } from "@/components/searchResults/searchResults";
import { movieService } from "@/services/movieService";
import { Pagination } from "@mui/material";
import Router from "next/router";
import { MINIMUM_SEARCH_TERM_LENGTH, SERVER_API_URL } from "@/constants";
import { MovieSearchResult } from "@/types";
import { GetServerSideProps } from "next";

export const getServerSideProps = (async (context) => {
  const searchTerm = context?.query?.query as string;
  const page = context?.query?.page as unknown as number;

  if (!searchTerm || page === undefined) {
    return {
      props: { searchTerm: "", searchResult: {} },
    };
  }

  const searchResult = await movieService.search(searchTerm, page, {
    baseUrl: SERVER_API_URL,
    apiKey: "03b8572954325680265531140190fd2a",
  });

  return {
    props: { searchTerm, searchResult: searchResult },
  };
}) satisfies GetServerSideProps<{
  searchTerm: string;
  searchResult: MovieSearchResult | {};
}>;

function updateRoute(searchTerm: string, page: number) {
  Router.push(
    {
      pathname: "/",
      query: { query: searchTerm, page },
    },
    undefined,
    { shallow: true }
  );
}

export default function Home() {
  const {
    searchTerm,
    searchResult,
    setSearchRequest,
    setSearchSuccess,
    hydrate,
    started,
  } = useStore();

  const search = async (term: string, num: number = 1) => {
    updateRoute(term, num);
    const response = await movieService.search(term, num);
    setSearchSuccess(response);
  };

  useEffect(() => {
    hydrate();
  }, []);

  const handleChange = (_: any, page: number) => search(searchTerm, page);

  useEffect(() => {
    if (searchTerm.length < MINIMUM_SEARCH_TERM_LENGTH || !started) {
      return;
    }

    setSearchRequest();
    search(searchTerm, 1);
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
