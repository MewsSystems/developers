import { SearchIcon } from "@/assets/icons";
import { Input } from "../ui/input";
import { ChangeEvent, useRef } from "react";
import debounce from "just-debounce-it";
import { useMoviesActions } from "@/hooks/useMoviesActions";

export default function Search() {
  const movieSearch = useRef<string | undefined>();

  const { userSearch } = useMoviesActions();
  const debounceGetMovies = debounce(
    () => userSearch(movieSearch.current),
    500
  );
  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    movieSearch.current = e.target.value;
    debounceGetMovies();
  };

  return (
    <>
      <form className="flex-1 ml-auto">
        <div className="relative">
          <SearchIcon className="absolute left-2.5 top-2.5 h-4 w-4 text-gray-500 dark:text-gray-400" />
          <Input
            className="pl-8 w-full"
            placeholder="Search for movies..."
            type="search"
            onChange={handleInputChange}
          />
        </div>
      </form>
    </>
  );
}
