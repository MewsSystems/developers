"use client";
import React, { useState } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import useDebounce from "@/hooks/UseDebounce";

interface MovieSearchInputProps {
  delay?: number;
}

export default function MovieSearchInput({
  delay = 500,
}: MovieSearchInputProps) {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();
  const [searchTerm, setSearchTerm] = useState("");

  // this function could be moved around to be more reusable but we could reuse this input elsewhere
  const handleMovieSearch = (searchTerm: string) => {
    const params = new URLSearchParams(searchParams);

    if (searchTerm) {
      params.set("searchterm", searchTerm);
      params.set("page", "1");
    } else {
      params.delete("searchterm");
      params.delete("page");
    }

    replace(`${pathname}?${params.toString()}`);
  };

  const [debouncedHandleMovieSearch] = useDebounce(handleMovieSearch, delay);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    debouncedHandleMovieSearch(e.target.value);
  };

  return (
    <input
      type="search"
      placeholder="Search for a movie"
      value={searchTerm}
      onChange={handleChange}
    />
  );
}
