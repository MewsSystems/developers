import { Box, TextField } from "@mui/material";

export const SearchInput = () => {
  return (
    <Box sx={{ width: 300, textAlign: "center" }}>
      <TextField size="small" fullWidth placeholder="Search for a movie" />
    </Box>
  );
};
