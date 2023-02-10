import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/hooks";

import { debounce } from "../../utils/debounce";
import { selectMoviesState } from "../../selectors/movies";
import { fetchMovies } from "../../actions/fetchMovies";
import { clearMovies } from "../../reducers/moviesListSlice";
import { Form, Icon, Input } from "./SearchForm.styled";

export const SearchForm = () => {
  const dispatch = useAppDispatch();
  const state = useAppSelector(selectMoviesState);

  const [searchParams, setSearchParams] = useSearchParams();
  const searchKeyParamValue = searchParams.get("q");
  const [inputValue, setInputValue] = useState(state.searchKey);

  const onSearch = (value: string) => {
    if (value) {
      searchParams.set("q", value);
      const page = state.activePage || 1;
      searchParams.set("page", page.toString());

      dispatch(fetchMovies(value, page));
    } else {
      searchParams.delete("q");
      searchParams.delete("page");
      dispatch(clearMovies());
    }

    setSearchParams(searchParams);
  };

  const debouncedSearch = debounce(onSearch, 200);

  const onValueChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setInputValue(value);
    debouncedSearch(value);
  };

  useEffect(() => {
    if (searchKeyParamValue) {
      const pageParamValue = parseInt(searchParams.get("page") ?? "", 10) || 1;
      setInputValue(searchKeyParamValue);
      dispatch(fetchMovies(searchKeyParamValue, pageParamValue));
    }
  }, [searchKeyParamValue, searchParams, dispatch]);

  return (
    <Form onSubmit={e => e.preventDefault()}>
      <Icon className="fa fa-search" />
      <Input
        type="text"
        value={inputValue}
        onChange={event => onValueChange(event)}
        placeholder="Search..."
      />
    </Form>
  );
};
