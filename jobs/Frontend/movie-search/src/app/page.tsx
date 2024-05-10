"use client";

import { useState } from "react";
import styles from "./page.module.css";
import { buildMovieDBUrl } from "@/utils/buildMovieDBUrl";
import Image from "next/image";
import { useRouter } from "next/router";
import Movies from "@/components/Movies";
import { DebouncedSearchInput } from "@/components/DebouncedSearchInput";

export type Movie = {
  id: number;
  original_title: string;
  title: string;
  overview: string;
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  popularity: number;
  poster_path: string;
  release_date: string;
};

export default function Home() {
  const [query, setQuery] = useState<string>("");
  const [data, setData] = useState<Movie[]>([]);

  const handleSearch = async (query: string) => {
    console.log("handleSearch");
    if (!query) return;

    const url = buildMovieDBUrl("search/movie", query);
    const options = { method: "GET", headers: { accept: "application/json" } };

    const response = await fetch(url, options);
    const data = await response.json();
    setData(data.results);
  };



  return (
    <main className={styles.main}>
      <DebouncedSearchInput
        onSearchChange={handleSearch}
        delay={1500}
        placeholder="Search for a movie or a tv show..." />

      {data && <Movies movies={data} />}
    </main>
  );
}
