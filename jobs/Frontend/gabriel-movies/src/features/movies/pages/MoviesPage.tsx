import { useCallback } from "react";
import { useSearchParams } from "react-router-dom";
import { Page } from "@/shared/ui/atoms/Layout/Page";
import { Title } from "@/shared/ui/atoms/Typography/Title";
import { Loader } from "@/shared/ui/molecules/Loader";
import { EmptyState } from "@/shared/ui/molecules/EmptyState";
import { useInfiniteScroll } from "@/shared/hooks/useInfiniteScroll";
import { SearchInput } from "../components/SearchInput";
import { MovieList } from "../components/MovieList";
import { useSearchMovies } from "../hooks/useSearchMovies";

export default function MoviesPage() {
  const [params, setParams] = useSearchParams();
  const q = params.get("q") ?? "";

  const {
    data,
    isLoading,
    isFetchingNextPage,
    hasNextPage,
    fetchNextPage,
  } = useSearchMovies(q);

  const loadMoreRef = useInfiniteScroll({ hasNextPage, isFetchingNextPage, fetchNextPage });

  const handleChange = useCallback((value: string) => {
    if (value === q) return;
    setParams(value ? { q: value } : {}, { replace: true });
  }, [q, setParams]);

  const showNoResults = !!q && !isLoading && data.length === 0;

  return (
    <Page>
      <Title>Search Movies</Title>

      <SearchInput
        initialValue={q}
        onChange={handleChange}
      />

      {isLoading && <Loader label="Searching movies..." />}

      {showNoResults && (
        <EmptyState
          title="No results"
          subtitle={`We couldn't find anything for “${q.trim()}”.`}
        />
      )}

      {!isLoading && data.length > 0 && (
        <>
          <MovieList items={data} />
          {hasNextPage && <div ref={loadMoreRef} />}
        </>
      )}
    </Page>
  );
}
