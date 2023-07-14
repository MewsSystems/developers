import { FC } from "react";
import { HorizontalCentered } from "src/components/HorizontalCentered";
import { useGetMoviesQuery } from "src/store/slices/moviesSlice";
import { Movie } from "src/store/types/Movie";
import { ReduxHookReturn } from "src/store/types/ReduxHookReturn";
import { InputSearch } from "src/views/MovieSearch/components/InputSearch/InputSearch";
import styled from "styled-components";

export const MovieSearch: FC = () => {
  const { data, isFetching }: ReduxHookReturn<Movie[]> =
    useGetMoviesQuery("inter");

  console.log(data);
  console.log(isFetching);

  return (
    <>
      <HorizontalCentered>
        <InputSearch
          debounceTime={3000}
          onDebounce={(value) => console.log(value)}
          onEnter={(value) => console.log(value + "__enter")}
          placeholder={"Search movies.."}
          loading={isFetching}
        />
      </HorizontalCentered>
    </>
  );
};
