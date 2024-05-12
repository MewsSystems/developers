"use client";
import React, { useState } from "react";
import useDebounce from "@/hooks/UseDebounce";
import { useSearchNavigation } from "@/hooks/UseSearchNavigation";
import styled from "styled-components";

const SearchContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
`;

// Styled Input
const StyledInput = styled.input`
  width: 50%; // Responsive width
  padding: 10px 15px;
  font-size: 16px;
  border: 2px solid #ccc;
  border-radius: 8px;
  transition: border-color 0.3s ease-in-out, box-shadow 0.3s ease-in-out;

  &:hover,
  &:focus {
    border-color: #007bff;
    box-shadow: 0 2px 8px rgba(0, 123, 255, 0.2);
    outline: none;
  }
`;

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
      />
    </SearchContainer>
  );
}
