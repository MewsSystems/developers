import { useState } from "react";
import Pagination from "@/shared/ui/Pagination";
import { MoviesCards } from "@/pages/movies-list/ui/MovieCards";
import { useQueryMovieList } from "@/pages/movies-list/api/useQueryMovieList";
import { useDebouncedValue } from "@/shared/hooks/useDebouncedValue";

export function Index() {
  const [page, setPage] = useState("1");
  const [searchText, debouncedSearchText, updateValue] = useDebouncedValue("");

  const { isLoading, data, total_results, total_pages } = useQueryMovieList({
    page,
    query: debouncedSearchText,
  });

  return (
    <div className="p-2" style={{ paddingBottom: "40px" }}>
      <input
        autoFocus
        type="search"
        value={searchText}
        onChange={updateValue}
        placeholder="Type to search..."
        style={{ width: "100%" }}
      />
      {isLoading ? (
        "loading"
      ) : (
        <div>
          <MoviesCards movieCardItems={data} />
        </div>
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
    </div>
  );
}
