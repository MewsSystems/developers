"use client";

import { useState } from "react";
import styles from "./page.module.css";
import { buildMovieDBUrl } from "@/utils/buildMovieDBUrl";
import Movies from "@/components/Movies";
import { DebouncedSearchInput } from "@/components/DebouncedSearchInput";
import Trending from "@/components/Trending";
import Pagination from "@/components/Pagination";
import { useSearchParams } from "next/navigation";
import { Movie } from "@/types/Movie";
import Popular from "@/components/Popular";

type Results = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

export default function Home() {
  const [data, setData] = useState<Results | null>();
  const [currentPage, setCurrentPage] = useState(data?.page ?? 1);

  const searchParams = useSearchParams();

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(() => {
      handleSearch(searchParams.get("query") ?? "", pageNumber)
      console.log(pageNumber)
      return pageNumber;
    });
  };

  const handleSearch = async (query: string, pageNumber?: number) => {
    if (!query) {
      setData(null); // Clear data if query is empty
      return;
    }

    try {
      const url = buildMovieDBUrl("search/movie", query, pageNumber ?? currentPage);
      const options = {
        method: "GET",
        headers: { accept: "application/json" },
      };

      const response = await fetch(url, options);
      const data = await response.json();
      setData(data);

    } catch (error) {
      console.error("Error fetching data:", error);
      setData(null);
    }
  };

  return (
    <main className={styles.main}>
      <DebouncedSearchInput
        onSearchChange={handleSearch}
        delay={1500}
        placeholder="Search for a movie or a tv show..."
      />

      {!data && <Trending />}

      {!data && <Popular />}

      {data && data?.results.length > 0 && (
        <>
          <h2>Results({data.total_results})</h2>
          <Movies movies={data?.results} />
          <Pagination
            currentPage={currentPage}
            totalPages={data.total_pages}
            onPageChange={handlePageChange}
          />
        </>
      )}
    </main>
  );
}
