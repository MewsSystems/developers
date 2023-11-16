import styled from "styled-components";
import { PageHeader } from "../components/PageHeader";
import { VStack } from "../components/shared/Stacks";
import { SearchInput } from "../components/movies/search/SearchInput";
import { SearchResults } from "../components/movies/search/SearchResults";
import { useState } from "react";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getMovies } from "../queries/movieQueries";
import { ErrorNotification } from "../components/shared/ErrorNotification";

const ContentWrapper = styled.div`
  width: 70vw;
  min-width: 1200px;
  max-width: 1800px;
  margin-left: auto;
  margin-right: auto;
  height: 1000px;
`;

export function MoviesSearch() {
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);

  const {
    data: moviesPage,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["movies", search, page],
    queryFn: () => getMovies(search, page),
    staleTime: Infinity,
    refetchOnWindowFocus: false,
    enabled: search.length > 3,
    placeholderData: keepPreviousData,
  });

  return (
    <ContentWrapper>
      <VStack>
        <PageHeader />
        <SearchInput setSearch={setSearch} />
        {isError ? (
          <ErrorNotification/>
        ) : (
          <SearchResults
            page={moviesPage}
            search={search}
            isLoading={isLoading}
            setPage={setPage}
          />
        )}
      </VStack>
    </ContentWrapper>
  );
}
