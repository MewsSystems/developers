import * as React from "react";
import { useState, useEffect, useRef } from "react";
import TextField from "@mui/material/TextField";
import Stack from "@mui/material/Stack";
import Autocomplete from "@mui/material/Autocomplete";
import { ImdbMovieResponse, Movie } from "../../../models/tmdbModels";
import SearchIcon from "@mui/icons-material/Search";
import { InputAdornment, Popper } from "@mui/material";
import { tmdbService } from "../../../services/tmdbServie";
import { hexToRgba } from "../../../utils/utils";
import { ItemOption } from "../../atoms/search/item-option";
import { useAppDispatch } from "../../../redux/reduxHooks";
import { setMovieSearched } from "../../../redux/searchEngine";
import styled from "styled-components";

export const AutocompleteSearcher: React.FC<{
  size: "small" | "large";
  color: string;
  placeholder: string;
}> = ({ size, color, placeholder }) => {
  const [items, setItems] = useState<Movie[]>([]);
  const [searcherQuery, setSearcherQuery] = useState<string>("");
  const [page, setPage] = useState<number>(1);
  const [errorMessage, setErrorMessage] = useState<null | string>(null);
  const searcherColor = hexToRgba(color);
  const searcherColorBlur = hexToRgba(color, "0.2");
  const inputRef = useRef<HTMLInputElement>(null);

  const dispatch = useAppDispatch();

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearcherQuery(event.target.value);
    dispatch(setMovieSearched(event.target.value));
    setPage(1);
    setItems([]);
    setErrorMessage(null);
  };

  const fetchMovies = async (currentPage: number) => {
    const queryToSend = searcherQuery.replaceAll(" ", "%20");
    try {
      const response = await tmdbService.get<ImdbMovieResponse<Movie[]>>(
        `/3/search/movie?query=${queryToSend}&language=en-US&page=${currentPage}`
      );
      if (response.results.length > 0) {
        setItems((prev) => [...prev, ...response.results.slice(0, 5)]);
      }
    } catch (error) {
      setErrorMessage("Failed to fetch movies. Please try again.");
    }
  };

  useEffect(() => {
    if (searcherQuery) {
      fetchMovies(page);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [searcherQuery, page]);

  return (
    <Stack spacing={2}>
      <Autocomplete
        freeSolo
        disableClearable
        clearIcon={null}
        id="customized-autocomplete"
        options={items}
        getOptionLabel={(option: string | Movie) =>
          typeof option === "string" ? option : option.title
        }
        sx={autocompleteStyle(searcherColorBlur, searcherColor, size)}
        slots={{
          listbox: (props) => (
            <Popper
              {...props}
              anchorEl={inputRef.current}
              open={true}
              sx={popperStyles(inputRef)}
            >
              <>{props.children}</>
            </Popper>
          ),
        }}
        renderOption={(params, option) => (
          <div key={option.id + Math.random()}>
            <ItemOption params={params} option={option} size={size} />
          </div>
        )}
        renderInput={(params) => (
          <TextField
            {...params}
            value={searcherQuery}
            onChange={handleInputChange}
            inputRef={inputRef}
            slotProps={{
              input: {
                ...params.InputProps,
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon sx={{ color: searcherColor }} />
                  </InputAdornment>
                ),
              },
            }}
            sx={renderInputTextFieldStyles(searcherColor)}
            placeholder={placeholder}
          />
        )}
      />
      <ErrorMessage>{errorMessage}</ErrorMessage>
    </Stack>
  );
};

const autocompleteStyle = (
  searcherColorBlur: string,
  searcherColor: string,
  size: "small" | "large"
) => ({
  "& .MuiOutlinedInput-root": {
    border: 1,
    borderColor: searcherColor,
    transition: "0.3s ease",
    width: size === "small" ? "80%" : "100%",
    alignSelf: "end",
    "&:hover": {
      backgroundColor: searcherColorBlur,
      transition: "width 0.3s ease",
      width: "100%",
    },
    borderRadius: 20,
    "&.Mui-focused": {
      border: 2,
      borderColor: searcherColor,
      backgroundColor: searcherColorBlur,
      width: "100%",
    },
  },
});

const popperStyles = (inputRef: React.RefObject<HTMLInputElement>) => ({
  backgroundColor: "rgba(196, 171, 156, 0.9)",
  color: "black",
  borderRadius: "10px",
  boxShadow: "0 2px 10px rgba(0, 0, 0, 0.15)",
  maxHeight: "200px",
  overflowY: "scroll",
  width: inputRef.current ? inputRef.current.clientWidth : "100%",
  zIndex: 9999,
  "&::-webkit-scrollbar": {
    width: "8px",
  },
  "&::-webkit-scrollbar-thumb": {
    backgroundColor: "#888",
    borderRadius: "4px",
  },
  "&::-webkit-scrollbar-thumb:hover": {
    backgroundColor: "#555",
  },
  "&::-webkit-scrollbar-track": {
    backgroundColor: "rgba(0,0,0,0.1)",
    borderRadius: "4px",
  },
});

const renderInputTextFieldStyles = (searcherColor: string) => ({
  "& .MuiOutlinedInput-notchedOutline": {
    border: "none",
  },
  "& .MuiInputBase-input::placeholder": {
    opacity: 1,
    color: searcherColor,
  },
  "& .MuiInputBase-input": {
    color: searcherColor,
  },
});

const ErrorMessage = styled.div`
  color: red;
  font-weight: bold;
`;
