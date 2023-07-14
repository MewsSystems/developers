import { FC, useEffect, useState } from "react";
import { HorizontalCentered } from "src/components/HorizontalCentered";
import { useLazyGetMoviesQuery } from "src/store/slices/moviesSlice";
import { MovieType } from "src/store/types/MovieType";
import { ReduxLazyHookReturn } from "src/store/types/ReduxLazyHookReturn";
import { InputSearch } from "src/views/MovieSearch/components/InputSearch/InputSearch";
import { MovieList } from "src/views/MovieSearch/components/MovieList/MovieList";

export const MovieSearch: FC = () => {
  const [inputValue, setInputValue] = useState("");
  const [trigger, { isLoading }]: ReduxLazyHookReturn<MovieType[]> =
    useLazyGetMoviesQuery();

  useEffect(() => {
    if (inputValue === "") return;

    trigger(inputValue);
  }, [inputValue]);

  return (
    <HorizontalCentered>
      <InputSearch
        debounceTime={3000}
        onDebounce={(value) => setInputValue(value)}
        onEnter={(value) => setInputValue(value)}
        placeholder={"Search movies.."}
        loading={isLoading}
      />
      <MovieList inputValue={inputValue} />
    </HorizontalCentered>
  );
};
