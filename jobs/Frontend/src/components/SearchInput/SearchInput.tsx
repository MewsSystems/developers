import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import { fetchMovies } from "@redux/actions/movieActions";
import { SearchBar, SearchWrapper, ClearButton } from "./SearchInput.styled";

// Define a constant variable for storing search query in local storage
const SEARCH_QUERY_STORAGE_KEY = "searchQuery";

// Define and export the SearchInput component
export const SearchInput: React.FC = () => {

  // Define a state variable to store the search query entered by the user
  const [searchQuery, setSearchQuery] = useState(
    localStorage.getItem(SEARCH_QUERY_STORAGE_KEY) || ""
  );

  // Get the Redux store dispatch method
  const dispatch = useDispatch();

  // Store the search query in local storage when it changes
  useEffect(() => {
    localStorage.setItem(SEARCH_QUERY_STORAGE_KEY, searchQuery);
  }, [searchQuery]);

  // Handle input changes in the search input field
  const handleInput = (e: React.FormEvent<HTMLInputElement>) => {
    setSearchQuery(e.currentTarget.value);
  };

  // Dispatch an action to fetch movies from the server based on the search query
  useEffect(() => {
    dispatch(fetchMovies(searchQuery));
  }, [searchQuery, dispatch]);

  // Handle clearing the search query
  const clearSearchQuery = () => {
    setSearchQuery("");
  };

  // Render the search input field and a clear button if the search query is not empty
  return (
    <SearchWrapper>
      <SearchBar
        type="text"
        value={searchQuery}
        onInput={handleInput}
        placeholder="Enter your keywords..."
      />
      {searchQuery.length > 0 && (
        <ClearButton onClick={clearSearchQuery}>
          <span>×</span>
        </ClearButton>
      )}
    </SearchWrapper>
  );
};

// Export the SearchInput component as default
export default SearchInput;
