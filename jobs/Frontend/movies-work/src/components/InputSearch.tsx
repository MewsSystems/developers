import { useEffect, useContext, useRef } from "react";
import { AppContext } from "../contexts/AppContext";
import SearchSVG from "../assets/search.svg";

export default function InputSearch() {
  const { searchMovieKeyword, changeSearchKeyword, searchMovies } =
    useContext(AppContext);

  const inputRef = useRef();

  useEffect(() => {
    // it does not make sense to search for an empty string
    if (searchMovieKeyword) {
      searchMovies();
    }
  }, [searchMovieKeyword, searchMovies]);

  // for debouncing the input change, if user stops typing, then the search will be triggered
  let timerIdChange: NodeJS.Timeout;

  const onInputChangeHandler = () => {
    if (timerIdChange) {
      clearTimeout(timerIdChange);
    }
    // do not update the state if the input is not available
    // updating status to an empty value would cause re-rendering and it is not necessary
    if (!inputRef.current) return;

    // changing the state will start searching for movies in useEffect
    timerIdChange = setTimeout(() => {
      const searchInputKeyword = inputRef.current.value;
      changeSearchKeyword(searchInputKeyword);
    }, 500);
  };

  const onSubmitHandler = (event: React.FormEvent) => {
    event.preventDefault();
  };

  return (
    <form onSubmit={onSubmitHandler}>
      <label
        htmlFor="default-search"
        className="mb-2 text-sm font-medium text-gray-900 sr-only dark:text-white"
      >
        Search
      </label>
      <div className="relative">
        <div className="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <img src={SearchSVG} className="w-4" alt="Search icon" />
        </div>
        <input
          onChange={onInputChangeHandler}
          ref={inputRef}
          type="search"
          id="default-search"
          className="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
          placeholder="Search movies..."
        />
      </div>
    </form>
  );
}
