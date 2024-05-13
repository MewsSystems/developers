"use client";
import React, { useState } from "react";
import useDebounce from "@/hooks/UseDebounce";
import { useSearchNavigation } from "@/hooks/UseSearchNavigation";
import {
  SearchContainer,
  StyledInput,
} from "./MovieSearchInputStyledComponents";

interface MovieSearchInputProps {
  delay?: number;
}

export default function MovieSearchInput({
  delay = 500,
}: MovieSearchInputProps) {
  const { movieSearch, searchterm: defaultQuery } = useSearchNavigation();
  const [searchTerm, setSearchTerm] = useState(defaultQuery || "");

  const [debouncedHandleMovieSearch] = useDebounce(movieSearch, delay);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    debouncedHandleMovieSearch(e.target.value);
  };

  return (
    <SearchContainer>
      <StyledInput
        type="search"
        placeholder="Search for a movie"
        value={searchTerm}
        onChange={handleChange}
        data-testid="MovieSearchInput"
      />
    </SearchContainer>
  );
}
