import { ChangeEvent, useState } from "react";
import { useStore } from "@/context";
import styles from "./searchField.module.css";
import { Spinner } from "../spinner/spinner";
import { IconButton, InputAdornment, TextField } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import ClearIcon from "@mui/icons-material/Clear";
import CircularProgress from "@mui/material/CircularProgress";

type TextFieldEndAdornment = {
  searchInProgress: boolean;
  showClear: boolean;
  handleClearInput: () => void;
};

const TextFieldEndAdornment: React.FC<TextFieldEndAdornment> = ({
  searchInProgress,
  showClear,
  handleClearInput,
}) => (
  <InputAdornment position="end">
    {searchInProgress && <CircularProgress />}
    {showClear && (
      <IconButton
        aria-label="clear search term"
        onClick={handleClearInput}
        edge="end"
      >
        <ClearIcon />
      </IconButton>
    )}
  </InputAdornment>
);

export const SearchField: React.FC = () => {
  const { searchTerm, setSearchTerm, searchInProgress } = useStore();

  const [showClear, setShowClear] = useState(false);

  const handleChange = (args: ChangeEvent<HTMLInputElement>) => {
    const term = args.target.value;
    if (term.length) {
      setShowClear(true);
    } else {
      setShowClear(false);
    }

    setSearchTerm(term);
  };

  const handleClearInput = () => {
    setSearchTerm("");
    setShowClear(false);
  };

  return (
    <>
      <TextField
        id="outlined-basic"
        label="Search movies..."
        variant="outlined"
        onChange={handleChange}
        value={searchTerm}
        autoFocus
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <SearchIcon />
            </InputAdornment>
          ),
          endAdornment: (
            <TextFieldEndAdornment
              showClear={showClear}
              searchInProgress={searchInProgress}
              handleClearInput={handleClearInput}
            />
          ),
        }}
        fullWidth
      />
    </>
  );
};
