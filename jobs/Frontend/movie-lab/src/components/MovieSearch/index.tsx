'use client';

import { useRouter, useSearchParams } from "next/navigation";
import { useState, ChangeEvent, KeyboardEvent, useEffect } from "react";
import { InputWrapper, SearchContainer, SearchIcon, SearchInput } from "@/src/components/MovieSearch/style";

export default function MovieSearch() {
  const router = useRouter();
  const searchParams = useSearchParams();

  const search = searchParams.get('q');
  const page = parseInt(searchParams.get("page") ?? "1", 10);
  const [inputValue, setInputValue] = useState<string>(search ?? "");

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setInputValue(event.target.value);
  };

  const handleSearch = () => {
    const queryParams = [];

    if (inputValue) {
      queryParams.push(`q=${encodeURIComponent(inputValue)}`);
    }

    if (page > 1) {
      queryParams.push(`page=${page}`);
    }

    const queryString = queryParams.length > 0 ? `?${queryParams.join('&')}` : '/';

    router.push(queryString, { scroll: false });
  };

  const handleKeyPress = (event: KeyboardEvent<HTMLInputElement>) => {
    if (event.key === "Enter") handleSearch();
  };

  useEffect(() => {
    const debounceTimer = setTimeout(() => {
      handleSearch();
    }, 500); // Adjust debounce delay as needed (e.g., 500 milliseconds)

    return () => {
      clearTimeout(debounceTimer);
    };
  }, [inputValue]); // Trigger effect when inputValue changes


  return (
    <SearchContainer>
      <InputWrapper>
        <SearchIcon>🔍</SearchIcon>
        <SearchInput
          type="text"
          placeholder="Search movies..."
          value={inputValue}
          onChange={handleChange}
          onKeyDown={handleKeyPress}
        />
      </InputWrapper>
    </SearchContainer>
  );
};
