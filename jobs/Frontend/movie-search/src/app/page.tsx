"use client";

import { useEffect, useState } from "react";
import styles from "./page.module.css";
import { buildMovieDBUrl } from "@/utils/buildMovieDBUrl";
import Movies from "@/components/Movies";
import { DebouncedSearchInput } from "@/components/DebouncedSearchInput";
import Pagination from "@/components/Pagination";
import { useSearchParams } from "next/navigation";
import { Movie } from "@/types/Movie";
import dynamic from 'next/dynamic'


type Results = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

const DynamicSection = dynamic(() => import('@/components/Section'), {
  loading: () => <p>Loading...</p>,
})


const getPopular = async () => {
  const url = buildMovieDBUrl("movie/now_playing");
  const options = { method: "GET", headers: { accept: "application/json" } };

  const response = await fetch(url, options);
  const data = await response.json();
  return data.results as Movie[];
};


const getTrending = async () => {
  const url = buildMovieDBUrl("trending/movie/day");
  const options = { method: "GET", headers: { accept: "application/json" } };

  const response = await fetch(url, options);
  const data = await response.json();
  return data.results as Movie[];
};

export default function Home() {
  const [data, setData] = useState<Results | null>();
  const [currentPage, setCurrentPage] = useState(data?.page ?? 1);

  const searchParams = useSearchParams();

  useEffect(() => {
    if (!searchParams.get("query")) {
      setData(null);
    }
  }, [searchParams])

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(() => {
      handleSearch(searchParams.get("query") ?? "", pageNumber);
      console.log(pageNumber);
      return pageNumber;
    });
  };

  const handleSearch = async (query: string, pageNumber?: number) => {
    if (!searchParams.get("query") && !query) {
      setData(null); // Clear data if query is empty
      return;
    }

    const searchTerm = searchParams.get("query") || query
    console.log(searchTerm)

    try {
      const url = buildMovieDBUrl(
        "search/movie",
        searchTerm,
        pageNumber ?? currentPage
      );
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

      {!data && <DynamicSection title="Trending" getMovies={getTrending} />}

      {!data && <DynamicSection title="Popular" getMovies={getPopular} />}

      {data && data?.results.length > 0 && (
        <>
          <h2>
            Results for {searchParams.get("query")} ({data.total_results})
          </h2>
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
