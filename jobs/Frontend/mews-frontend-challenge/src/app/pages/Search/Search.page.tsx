import { Suspense } from "react";
import { getSearchRoute } from "@/app/AppRouter.utils";

import { LoadingScreen } from "@/app/components/LoadingScreen";
import { useSuspenseQuery } from "@tanstack/react-query";

import { SearchInput } from "./components/SearchInput";
import { SearchLanding } from "./components/SearchLanding";
import { SearchPagination } from "./components/SearchPagination";
import { SearchResults } from "./components/SearchResults";
import {
  emptyResult,
  getMovies,
  SearchPageFilters,
  useSearchFilters,
} from "./Search.data";

export function Search() {
  const { q, page } = useSearchFilters();

  return (
    <section className="flex flex-col p-4 pb-32 md:p-12 lg:p-24">
      <SearchLanding className="mb-6" />
      <SearchInput />
      {!!q && (
        <Suspense fallback={<LoadingScreen className="mt-4 h-[200px]" />}>
          <SearchContent q={q} page={page} />
        </Suspense>
      )}
    </section>
  );
}

function SearchContent({ q, page }: SearchPageFilters) {
  const { data = emptyResult } = useSuspenseQuery({
    queryKey: ["search", q, page],
    queryFn: () => getMovies({ q, page }),
  });

  return (
    <section className="py-6">
      <SearchResults data={data} searchQuery={q} />
      <SearchPagination
        data-testid="search-pagination"
        className="mt-8"
        linkBuilder={(page) => getSearchRoute({ q, page })}
        currentPage={data.pagination.page}
        totalPages={data.pagination.totalPages}
      />
    </section>
  );
}
