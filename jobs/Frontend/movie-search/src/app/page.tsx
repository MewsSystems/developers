"use client";

import { useState } from "react";
import styles from "./page.module.css";
import { buildMovieDBUrl } from "@/utils/buildMovieDBUrl";
import Image from "next/image";
import { useRouter } from "next/router";
import Movies from "@/components/Movies";

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

  const handleSearch = async (e) => {
    e.preventDefault();
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
      <form>
        <input
          id="search"
          name="search"
          placeholder="Search for a movie or a tv show..."
          value={query}
          onChange={(e) => setQuery(e.currentTarget.value)}
        />
        <button onClick={handleSearch}>Search</button>
      </form>

      <Movies movies={data} />
    </main>
  );
}
