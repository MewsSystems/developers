import React, { useMemo } from "react";
import _ from "lodash";
import Box from "@mui/material/Box";
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import { AppBar } from "../../../../components/AppBar/AppBar";
import { TextField } from "../../../../components/TextField/TextField";

type SearchBarProps = {
  onSearch: (e: string) => void;
  onToggle: (e: "table" | "grid") => void;
  view: string;
}

export const SearchBar = ({ onSearch, onToggle, view }: SearchBarProps) => {
  const debouncedResults = useMemo(() => {
    return _.debounce(onSearch, 400);
  }, [onSearch]);

  return (
    <AppBar>
      <Box
        display="flex"
        gap={2}
        justifyContent="center"
        p={3}
      >
        <TextField 
          onChange={(event) => debouncedResults(event)}
          placeholder="Search for a movie title"
        />
        
        <ToggleButtonGroup
          exclusive
          onChange={(event, value) => onToggle(value)}
          value={view}
        >
          <ToggleButton value="table">Table</ToggleButton>
          <ToggleButton value="grid">Image</ToggleButton>
        </ToggleButtonGroup>
      </Box >
    </AppBar>
  );
};
