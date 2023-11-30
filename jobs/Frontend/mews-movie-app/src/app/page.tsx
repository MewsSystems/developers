"use client";

import { useGetMoviesQuery } from "@/store/movies/api";

export default function Home() {
    const { data, error, isLoading } = useGetMoviesQuery("test");

    return <main></main>;
}
