import { useEffect, useState } from "react";
import { useDebounce } from "use-debounce";

import "../CSS/Search.css";

interface SearchProps {
  resultsHandler: (results: object) => void;
  page: number | undefined;
  resetPage: (page: number) => void;
  showLoading: (loading: boolean) => void;
  updateScore: (score: string) => void;
}

const Search: React.FC<SearchProps> = ({
  resultsHandler,
  page,
  resetPage,
  showLoading,
}) => {
  const [search, setSearch] = useState("");
  const [prevSearch, setPrevSearch] = useState("");
  const [value] = useDebounce(search, 1000);

  const apiToken = import.meta.env.VITE_API_TOKEN;

  // Call search API and handle reset action on debounce value change
  useEffect(() => {
    if (value && page) {
      callApi(value, page);
      showLoading(true);
    }
    if (value !== prevSearch) {
      resetPage(1);
    }
    // Update previous search value
    setPrevSearch(value);
  }, [value, page]);

  // Handle search input change
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  // Async API call
  const callApi = async (value: string, page: number) => {
    const options = {
      method: "GET",
      headers: {
        accept: "application/json",
        Authorization: "Bearer " + apiToken,
      },
    };

    try {
      const response = await fetch(
        "https://api.themoviedb.org/3/search/movie?query=" +
          value +
          "&include_adult=true&language=en-US&page=" +
          page,
        options
      );

      if (!response.ok) {
        throw new Error("Failed to fetch data");
      }

      // Wait for data
      const data = await response.json();
      resultsHandler(data);
    } catch (err) {
      console.error("Error fetching data:", err);
    }
  };

  return (
    <div className="search-container">
      <input
        type="search"
        placeholder="Start typing to search"
        value={search}
        onChange={handleChange}
      />
    </div>
  );
};

export default Search;
