import { useState } from "react";
import Pagination from "@/shared/ui/Pagination";
import { MoviesCards } from "@/pages/movies-list/ui/MovieCards";
import { useQueryMovieList } from "@/pages/movies-list/api/useQueryMovieList";
import { useDebouncedValue } from "@/shared/hooks/useDebouncedValue";
import { Box, Input } from "@chakra-ui/react";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import useQueryAccountFavoriteMovies from "@/entities/account/hooks/useQueryAccountFavoriteMovies";
import LoadingSpinner from "@/shared/ui/LoadingSpinner";

export function Index() {
  const [page, setPage] = useState("1");
  const [searchText, debouncedSearchText, updateValue] = useDebouncedValue("");

  const auth = useAuth();

  const { data: dataAccountFavoritesMovies, isLoading: isLoadingFavorites } =
    useQueryAccountFavoriteMovies({
      accountId: auth?.accountId ?? 0,
    });

  const favoritesMap = new Map<number, boolean>();

  (dataAccountFavoritesMovies?.results ?? []).forEach((favoriteItem) => {
    favoritesMap.set(favoriteItem.id, true);
  });

  const {
    isLoading: isLoadingMovies,
    data,
    total_results,
    total_pages,
  } = useQueryMovieList({
    page,
    query: debouncedSearchText,
  });
  const isLoading = [
    isLoadingMovies,
    isLoadingFavorites,
    debouncedSearchText !== searchText,
  ].some((a) => a);

  return (
    <Box px="2" height={"100%"}>
      <Input
        autoFocus
        type="search"
        value={searchText}
        onChange={updateValue}
        placeholder="Type to search..."
        style={{ width: "100%" }}
      />
      {isLoading ? (
        <LoadingSpinner />
      ) : (
        <Box py="4">
          <MoviesCards movieCardItems={data} favoritesMap={favoritesMap} />
        </Box>
      )}
      {total_results > 20 && (
        <Pagination
          page={page}
          pageSize={total_pages}
          onPageChange={(pageChangeDetails: any) => {
            setPage(pageChangeDetails.page);
          }}
        />
      )}
    </Box>
  );
}
