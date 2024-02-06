import { ChangeEvent, useContext, useState } from "react";
import { AppContext } from "../contexts/AppContext";
import { URLSearchParamsInit, useSearchParams } from "react-router-dom";
import SearchSVG from "../assets/search.svg";

// for debouncing the input change, if user stops typing, then the search will be triggered
// only one element on the page is allowed
let timerId;

export default function InputSearch() {
  const { searchMovieKeyword, changeKeyword } = useContext(AppContext);
  const [searchParams, setSearchParams] = useSearchParams();
  // I use internal state for the input value, because I want to control the input value when returning back from the detail page but also have the ability to change the input value when typing - using useRef would not allow me to do that - would not keep the keyword when returning back from the detail page
  const [inputValue, setInputValue] = useState(searchMovieKeyword);

  const onInputChangeHandler = (event) => {
    const inputEventValue = event.target.value;

    // TODO implement search params loading
    // update URL in order to be able to share the link
    const params: URLSearchParamsInit = { movie: inputEventValue };
    setSearchParams(params);
    // input is controlled by the state
    setInputValue(inputEventValue);

    if (timerId) {
      clearTimeout(timerId);
    }
    // do not search for the movies if the input is empty
    if (!inputEventValue) return;

    timerId = setTimeout(() => {
      changeKeyword(inputEventValue);
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
          value={inputValue}
          type="search"
          id="default-search"
          className="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
          placeholder="Search movies..."
        />
      </div>
    </form>
  );
}
